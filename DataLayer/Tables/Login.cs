using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Tables
{
    public class Login
    {
        public int UserId { get; set; }
        public string UserLogin { get; set; }
        public string HashedPassword { get; set; }
    }
}
