﻿using ProgettoIndividuale.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProgettoIndividuale.Models
{
    public class ProductViewModel
    {
        [Required]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public string ProductName { get; set; }
        [Required]
        public int? CategoryId { get; set; }
        
        [StringLength(15)]
        public string CategoryName { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public decimal? UnitPrice { get; set; }
        //[Required(ErrorMessage = "Campo obligatorio")]
        public short? UnitsInStock { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public bool Discontinued { get; set; }
        //[Required(ErrorMessage = "Campo obligatorio")]
        public string QuantityPerUnit { get; set; }

    }
}
