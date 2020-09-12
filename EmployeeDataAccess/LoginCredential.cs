//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeDataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class LoginCredential
    {
        public int UserId { get; set; }
        
        [DisplayName("User Name")]
        [Required(ErrorMessage = "This Field is Required!")]
        public string UserName { get; set; }
        
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "This Field is Required!")]
        public string Password { get; set; }
        
        public string LoginErrorMessage { get; set; }
    }
}
