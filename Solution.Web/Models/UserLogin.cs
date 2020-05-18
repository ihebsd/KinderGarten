﻿using Solution.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Solution.Web.Models

{
   
    public class UserLogin
    {
        [Key]
        public int idUser { get; set; }
        [Required(ErrorMessage = "Champs obligatoire")]
        [MaxLength(30)]
        public string nom { get; set; }
        [Required(ErrorMessage = "Champs obligatoire")]
        [MaxLength(30)]
        public string prenom { get; set; }
        [Required(ErrorMessage = "Champs obligatoire")]
        [MaxLength(30)]
        public string login { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Champs obligatoire")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Minimum 6 characters required")]
        public string password { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Champs obligatoire")]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "not the same pasword")]
        public string Confirmpassword { get; set; }
        public bool IsEmailVerified { get; set; }


        public role role { get; set; }



        [Display(Name ="Remember me")]
        public bool RememberMe { get; set; }
    }
}