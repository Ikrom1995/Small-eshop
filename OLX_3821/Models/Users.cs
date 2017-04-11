using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OLX_3821.Models
{
    public class Users
    {
        public int userID { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(10)]
        public string UTitle { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(30)]
        public string firstName { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(40)]
        public string lastName { get; set; }
        [Required]
        [MinLength(1)]
        [MaxLength(12)]
        public string phone { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [MaxLength(60)]
        public string eMail { get; set; }
        public double balance { get; set; }
        [Required]
        [MinLength(6)]
        [MaxLength(20)]
        //Must Be Unique
        public string username { get; set; }
        [Required]
        [MinLength(6)]
        [MaxLength(16)]
        public string uPassword { get; set; }

        public string fullName
        {
            get { return $"{userID} {firstName} {lastName}"; }
        }

    }
}