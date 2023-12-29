using Domain.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Product:BaseId
    {
        [Required]
        public string? Name {  get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Selling Price must be greater than or equal to 0.")]
        public double? SellingPrice { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "Purchase Price must be greater than or equal to 0.")]
        public double? PurchasePrice { get; set; }
    }
}
