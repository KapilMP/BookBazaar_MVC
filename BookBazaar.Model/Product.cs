﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookBazaar.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Range(1,1000), DisplayName("List Price")]
        public double ListPrice { get; set; }

        [Required]
        [Range(1, 1000), DisplayName("Price for 1-50")]
        public double Price { get; set; }
        [Required]
        [Range(1, 1000), DisplayName("Price for 50+")]
        public double Price50 { get; set; }
        [Required]
        [Range(1, 1000), DisplayName("Price for 100+")]
        public double Price100 { get; set; }

        //add foreign key
        public int CategoryId {  get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        [ValidateNever]
        public string ImgUrl { get; set; }
    }
}
