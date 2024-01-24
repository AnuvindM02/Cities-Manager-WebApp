using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitiesManager.Core.DTO
{
    public class RegisterDTO
    {
        [Required(ErrorMessage ="Person name can't be blank")]
        public string PersonName {  get; set; } = string.Empty;


        [Required(ErrorMessage = "Email can't be blank")]
        [EmailAddress(ErrorMessage = "Invalid Email id")]
        [Remote(action: "isEmailRegistered", controller: "Account", ErrorMessage = "Account is already in use")]
        public string Email { get; set; } = string.Empty;


        [Required(ErrorMessage = "Phone number can't be blank")]
        [RegularExpression("^[0-9]*$",ErrorMessage = "Phone number should contain digits only")]
        public string PhoneNumber { get; set; } = string.Empty;


        [Required(ErrorMessage ="Password can't be blank")]
        public string Password { get; set; } = string.Empty;


        [Required(ErrorMessage = "Confirm password can't be blank")]
        [Compare("Password",ErrorMessage ="Password and confirm password doesn't match")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
