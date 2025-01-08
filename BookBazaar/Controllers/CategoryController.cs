using BookBazaar.Data;
using BookBazaar.DataAccess.Repository.IRepository;
using BookBazaar.Model;
using Microsoft.AspNetCore.Mvc;

namespace BookBazaar.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _category;
        public CategoryController(ICategoryRepository db)
        {
            _category = db;   
            
        }

        public IActionResult Index()
        {
            List<Category> objCategories = _category.GetAll().ToList();
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

                _category.Add(obj);
                _category.Save();
                TempData["Success"] = "Category created sucessfully!";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            Category? category = _category.Get(u => u.Id == id);
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
            if (ModelState.IsValid)//modelstate is obj accoring
                                   //to data annotation provided in model
            {

                _category.Update(obj);
                _category.Save();
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
            Category? category = _category.Get(u => u.Id==id);
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
            Category? category = _category.Get(u => u.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            _category.Remove(category);
           _category.Save();
            TempData["Success"] = "Category deleted sucessfully!";
            return RedirectToAction("Index");
          
        }
    }
}
