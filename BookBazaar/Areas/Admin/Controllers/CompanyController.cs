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
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
       
        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
          
        }
        public IActionResult Index()
        {
            List<Company> objCompanies = _unitOfWork.Company.GetAll().ToList();

            return View(objCompanies);
        }

        public IActionResult Upsert(int? id)
        {
            if(id == null || id == 0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
               Company company = _unitOfWork.Company.Get(u=>u.Id == id);
                return View(company);
            }
           

        }
        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {
          
            if (ModelState.IsValid)
            {
              
                if(CompanyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(CompanyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(CompanyObj);
                }
                _unitOfWork.Save();
                TempData["Success"] = "Company Added Successfully";
                return RedirectToAction("Index");
            }
          
            else
            {
            
                return View(CompanyObj);
            }
        }
       


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanys = _unitOfWork.Company.GetAll().ToList();
            return Json(new {data = objCompanys});
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new { success = false , message = "Error while deleting"});

            }
           

            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();


            return Json(new { success=true, message = "Delete Sucessful" });
        }


        #endregion


    }
}
