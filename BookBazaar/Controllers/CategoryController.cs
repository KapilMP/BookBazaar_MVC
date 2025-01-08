using BookBazaar.Data;
using BookBazaar.Model;
using Microsoft.AspNetCore.Mvc;

namespace BookBazaar.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;   
            
        }

        public IActionResult Index()
        {
            List<Category> objCategories = _db.categories.ToList();
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

                _db.categories.Add(obj);
                _db.SaveChanges();
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
            Category? category = _db.categories.FirstOrDefault(c => c.Id == id);
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

                _db.categories.Update(obj);
                _db.SaveChanges();
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
            Category? category = _db.categories.FirstOrDefault(c => c.Id == id);
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
            Category? category = _db.categories.FirstOrDefault(u => u.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            _db.categories.Remove(category);
            _db.SaveChanges();
            TempData["Success"] = "Category deleted sucessfully!";
            return RedirectToAction("Index");
          
        }
    }
}
