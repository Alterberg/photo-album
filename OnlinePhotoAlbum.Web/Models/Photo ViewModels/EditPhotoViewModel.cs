using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlinePhotoAlbum.Web.Models
{
    public class EditPhotoViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Photo name")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Min Lengs is 3 chars / Max length is 50 chars")]
        public string Name { get; set; }
        [MaxLength(800, ErrorMessage = "Max length is 800 chars")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string Path { get; set; }
        [Display(Name = "Author ID")]
        public string AuthorId { get; set; }
        [Display(Name = "Upload Time")]
        public DateTime UploadTime { get; set; }
    }
}