using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxLienTracker4.Models
{
    public class SignupViewModel
    {
        
            public string Username { get; set; }
            public string Password { get; set; }
            public string ErrorMessage { get; set; }
            public string SystemPassword { get; set; }
        
    }
}