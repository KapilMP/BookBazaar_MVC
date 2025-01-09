using BookBazaar.DataAccess.Repository.IRepository;
using BookBazaar.Model;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace BookBazaar.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public IActionResult Index()
        {
            List<Product> objProducts = _unitOfWork.Product.GetAll().ToList();
            return View(objProducts);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Product Added Successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == 0 || id == null)
            {
                return RedirectToAction("Index");
            }

            Product products = _unitOfWork.Product.Get(u => u.Id == id);
            if (products == null)
            {
                return RedirectToAction("Index");
            }
            return View(products);
        }


        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)//obj is valid to the data annotation provided in model
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Product edited successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
            {
                return RedirectToAction("Index");
            }

            Product products = _unitOfWork.Product.Get(u => u.Id == id);
            if (products == null)
            {
                return RedirectToAction("Index");
            }
            return View(products);
        }


        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Product obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Product edited successfully";
            return RedirectToAction("Index");

        }


    }
}
