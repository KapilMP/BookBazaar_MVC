using BookBazaar.DataAccess.Repository.IRepository;
using BookBazaar.Model;
using BookBazaar.Model.ViewModels;
using BookBazaar.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Win32.SafeHandles;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace BookBazaar.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                OrderHeader = new()
            };
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.price * cart.Count);
            }

            return View(ShoppingCartVM);
        }
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new ShoppingCartVM()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            //first we need to retrieve the application user property and assign it to orderheader's application user
            //which is fk

            //and then manually assign the properties of orderheader from the application user of orderheader.
            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.StreetName = ShoppingCartVM.OrderHeader.ApplicationUser.StreetName;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.price * cart.Count);
            }

            return View(ShoppingCartVM);

        }
        [HttpPost]
        [ActionName(nameof(Summary))]

        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product"); //while post we might lose details becuse we dont have everything in input field.

            ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

            //ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            //problem:
            //this create exception as i am adding orderheader this line will also add all corresponding navigation property
            //ef core will think you aret trying to create new entity
            //voilation of primary constraint in aspnetuser
            //key take we should never populate navigation property when we are trying to insert a record in ef core.

            //solution:
            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);
            //we dont need mapping because it will be automatically populated


            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.price * cart.Count);
            }
            //solution:
            //change ShoppingCartVM.OrderHeader.ApplicationUser to applicationUser

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                //it is regular customer
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            }
            else
            {
                //it is company customer
                ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
            }
            //create order header
            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            //create order details
            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    price = cart.price,
                    Count = cart.Count

                };

                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }
            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                //it is regular customer we will capture stripe payment
                var domain = "https://localhost:7116/";

                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = domain+$"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                    CancelUrl = domain+"customer/cart/index",
                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),

              //  {
              //Lineitems will have all the product detail
              // rather than clustering here will be creating new list 
              
              //below we have configure for this
                //    new Stripe.Checkout.SessionLineItemOptions
                //    {
                //        Price = "price_1MotwRLkdIwHu7ixYcPLm5uZ",
                //        Quantity = 2, /
                //    },
                //},
                                Mode = "payment", //default
                    };

                //configuration is here
                foreach(var item in ShoppingCartVM.ShoppingCartList)
                {
                    var sessionLineItems = new SessionLineItemOptions //build in class
                    {
                        //data to create in price object
                        PriceData = new SessionLineItemPriceDataOptions
                        { 
                            UnitAmount = (long)(item.price * 100), //22.50 = 2250
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                //long class but here we add things like images, description and so on of each item
                                Name = item.Product.Title
                            }
                        },//generation price data for one item
                        Quantity = item.Count //the quantity of item like (1 book count is 10) each iteration it changes
                        //and base on that stiper will automatically determine the final price

                    };//this looks good for one item, so we need the add all the item in lineItems.
                    options.LineItems.Add(sessionLineItems);
                }

                           var service = new Stripe.Checkout.SessionService();
                          Session session =  service.Create(options);
                //this session will have sessionid and payment intent id
                _unitOfWork.OrderHeader.UpdateStripePaymentId(ShoppingCartVM.OrderHeader.Id,session.Id, session.PaymentIntentId);
                //payment intent id is null now, this in session object will only be populated once the payment is sucessfull.

                _unitOfWork.Save();

                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);//redirecting to new url provided above
                        }
            else
            {
                //it is company account
            }

            return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.OrderHeader.Id });
            //this will pass id to OrderConfirmation method

        }
        public IActionResult OrderConfirmation(int id)
        {
            //order header consist of payment status, order status, application user
            OrderHeader orderHeader = _unitOfWork.OrderHeader.Get(u => u.Id == id, includeProperties: "ApplicationUser");

            if(orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
            {
                //this is regular customer
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if(session.PaymentStatus.ToLower() == "paid")
                {
                    _unitOfWork.OrderHeader.UpdateStripePaymentId(id, session.Id, session.PaymentIntentId);
                    _unitOfWork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _unitOfWork.Save();
                }
            }
            List<ShoppingCart> shoppingCarts = _unitOfWork.ShoppingCart.GetAll
                (
                u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
            _unitOfWork.ShoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Save();

            return View(id);//and that above id is return back the view
        }

        public IActionResult Plus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            cartFromDb.Count += 1;
            _unitOfWork.ShoppingCart.Update(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Minus(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (cartFromDb.Count <= 1)
            {
                _unitOfWork.ShoppingCart.Remove(cartFromDb);
            }
            else
            {
                cartFromDb.Count -= 1;
                _unitOfWork.ShoppingCart.Update(cartFromDb);

            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Remove(int cartId)
        {
            var cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            _unitOfWork.ShoppingCart.Remove(cartFromDb);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));

        }
        public double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else
            {
                if (shoppingCart.Count <= 100)
                {
                    return shoppingCart.Product.Price50;
                }
                else
                {
                    return shoppingCart.Product.Price100;
                }
            }
        }
    }
}
