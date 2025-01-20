using BookBazaar.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookBazaar.Model.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; } //add data using instance of product
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }   
        //to retrive category data use projection to convert Ienumerable of category into selectlistitem
    }
}
