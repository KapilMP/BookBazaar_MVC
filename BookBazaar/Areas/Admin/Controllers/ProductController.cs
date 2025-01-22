using BookBazaar.DataAccess.Repository.IRepository;
using BookBazaar.Model;
using BookBazaar.Model.ViewModels;
using BookBazaar.Utility;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.ComponentModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookBazaar.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        //capture the file and save that in images\product
        //this is provided by default
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Product> objProducts = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();

            return View(objProducts);
        }

        public IActionResult Upsert(int? id)
        {

            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll()
              //projection to convert IEnumerable of category to selectlistitem
              .Select(u => new SelectListItem
              {
                  Text = u.Name,
                  Value = u.Id.ToString()
              }
              );
            // ViewBag.CategoryList = CategoryList;
            // ViewBag["KeyName"] = CategoryList;
            ProductVM productVM = new()
            {
                //see prouctvm in BookBazzar.Model/ViewModels
                CategoryList = CategoryList, //this variable is from above
                Product = new Product()//creating product instance and keeping it in our new variable product.
            };

            if(id == null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u=>u.Id == id);
                return View(productVM);
            }
           

        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            //in para convert product to productvm because we moved product implementation to productvm
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;//this will be wwwroot folder
                if(file != null)
                {                    //random name to our file
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");//folder to actual upload the file

                    if (!string.IsNullOrEmpty(productVM.Product.ImgUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImgUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImgUrl = @"\images\product\" + fileName;
                }
                if(productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                _unitOfWork.Save();
                TempData["Success"] = "Product Added Successfully";
                return RedirectToAction("Index");
            }
            //The dropdown list is populated in the GET request when the view is first rendered.
            //During the POST request, if there is a validation error and you need to re-display
            //the form,the dropdown list is not automatically re - populated.
            //To fix this, you need to re-populate the CategoryList in the else
            //block before returning the view.
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll()
                //projection to convert IEnumerable of category to selectlistitem
                          .Select(u => new SelectListItem
                          {
                              Text = u.Name,
                              Value = u.Id.ToString()
                          });
                return View(productVM);
            }
        }
        //public IActionResult Edit(int? id)
        //{
        //    if (id == 0 || id == null)
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    Product products = _unitOfWork.Product.Get(u => u.Id == id);
        //    if (products == null)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    return View(products);
        //}


        //[HttpPost]
        //public IActionResult Edit(Product obj)
        //{
        //    if (ModelState.IsValid)//obj is valid to the data annotation provided in model
        //    {
        //        _unitOfWork.Product.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["Success"] = "Product edited successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}
        //public IActionResult Delete(int? id)
        //{
        //    if (id == 0 || id == null)
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    Product products = _unitOfWork.Product.Get(u => u.Id == id);
        //    if (products == null)
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    return View(products);
        //}


        //[HttpPost]
        //[ActionName("Delete")]
        //public IActionResult DeletePost(int? id)
        //{
        //    Product obj = _unitOfWork.Product.Get(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Product.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["Success"] = "Product edited successfully";
        //    return RedirectToAction("Index");

        //}


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProducts = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new {data = objProducts});
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if (productToBeDeleted == null)
            {
                return Json(new { success = false , message = "Error while deleting"});

            }
            //delete image
            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, productToBeDeleted.ImgUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();


            return Json(new { success=true, message = "Delete Sucessful" });
        }


        #endregion


    }
}
