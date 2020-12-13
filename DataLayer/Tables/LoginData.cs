using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Tables
{
    public class LoginData
    { 
        [Key]
        public string Login { get; set; }
        public int UserId { get; set; }
        public string HashedPassword { get; set; }
    }
}
