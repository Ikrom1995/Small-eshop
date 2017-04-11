using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OLX_3821.Models
{
    public class Posters
    {
        public int posterID { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string category { get; set; }
        [Required]
        public string subcategory { get; set; }
        [Required]
        public double price { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public string address { get; set; }
        public DateTime startDate { get; set; }
        public DateTime cancelDate { get; set; }
        [Required]
        public int quantity { get; set; }
        public int userID { get; set; }
    }
}