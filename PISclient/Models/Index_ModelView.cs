using Microsoft.AspNetCore.Http;
using PISclient.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PISclient.Models
{
    public class Index_ModelView
    {
        [Required(ErrorMessage = "This field is required.")]
        public string fileOwner { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string fileType { get; set; }

        [Required(ErrorMessage = "Please select a file.")]
        [MaxFileSize(20)]//u megabajtima
        [AllowedExtensions(new string[] { ".pdf", ".xml", ".docx", ".zip" })]
        public IFormFile fajl { get; set; }
    }
}
