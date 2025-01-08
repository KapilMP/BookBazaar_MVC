﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookBazaar.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "Max length is 100"), DisplayName("Category Name")]
        public string Name { get; set; }
        [Range(1, 100), DisplayName("Display Order")]

        public int DisplayOrder { get; set; }
    }
}
