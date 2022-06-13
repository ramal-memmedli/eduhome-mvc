using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Models
{
    public class AppUser : IdentityUser
    {
        [Required, MaxLength(128)]
        public string Firstname { get; set; }
        [Required, MaxLength(128)]
        public string Lastname { get; set; }
    }
}
