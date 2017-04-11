using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OLX_3821.Models
{
    public class Report
    {
        public int userID { get; set; }

        public string UTitle { get; set; }

        public string firstName { get; set; }

        public string lastName { get; set; }

        public string category { get; set; }
        
        public double totalSpent { get; set; }
    }
}