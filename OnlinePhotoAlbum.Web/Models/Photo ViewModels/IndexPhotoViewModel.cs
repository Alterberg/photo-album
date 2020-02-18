using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlinePhotoAlbum.Web.Models
{
    public class IndexPhotoViewModel
    {
        public int Id { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string Path { get; set; }
        [Range(0, 5)]
        public double? Mark { get; set; }
        [Display(Name = "Author")]
        public string AuthorUsername { get; set; }
    }
}