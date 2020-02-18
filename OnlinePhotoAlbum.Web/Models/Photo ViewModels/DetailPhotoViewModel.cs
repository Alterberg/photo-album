using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlinePhotoAlbum.Web.Models
{
    public class DetailPhotoViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Photo Name")]
        public string Name { get; set; }
        [MaxLength(800)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string Path { get; set; }
        [Display(Name = "Upload Time")]
        public DateTime UploadTime { get; set; }
        [Display(Name = "Author Username")]
        public string AuthorUsername { get; set; }
        [Range(1,5)]
        [Display(Name = "Average Mark")]
        public double? Mark { get; set; }
    }
}