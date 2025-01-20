using BookBazaar.Data;
using BookBazaar.DataAccess.Repository.IRepository;
using BookBazaar.Model;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookBazaar.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller

    {
       
        //asking for implementation of DbContext that is present in program.cs (service container)
        //by creating constructor with parameter that provides
        //its implemenation (Data/ApplicationDbContext.cs)
        // private readonly ICategoryRepository _category;
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;//what implementation we get pass to our local variable
        }

        public IActionResult Index()
        {
            //_db can access all the DbSet<T> we added
            List<Category> objCategories = _unitOfWork.Category.GetAll().ToList();
            //_db.Categories.ToList(): this will run select * from categories and
            //provide it to objCategoryList
            return View(objCategories);

        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            // Check if the name contains only numbers
            if (int.TryParse(obj.Name, out _))
            {
                // Add an error to the ModelState
                ModelState.AddModelError("", "The Name cannot be a number.");
            }
            if (ModelState.IsValid)//modelstate is obj accoring
                                   //to data annotation provided in model
            {

                _unitOfWork.Category.Add(obj);//insert the value to the db using .net core
                                              //without needing of insert sql command or statement
                _unitOfWork.Save();//execute statement
                TempData["Success"] = "Category created sucessfully!";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? category = _unitOfWork.Category.Get(u => u.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            // Check if the name contains only numbers
            if (int.TryParse(obj.Name, out _))
            {
                // Add an error to the ModelState
                ModelState.AddModelError("", "The Name cannot be a number.");
            }
            if (ModelState.IsValid)//modelstate is obj according
                                   //to data annotation provided in model
            {

                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Category updated sucessfully!";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? category = _unitOfWork.Category.Get(u => u.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category? category = _unitOfWork.Category.Get(u => u.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();
            TempData["Success"] = "Category deleted sucessfully!";
            return RedirectToAction("Index");

        }
    }
}
