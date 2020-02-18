using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace OnlinePhotoAlbum.Web.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        [MaxLength(50, ErrorMessage = "Max length is 50 chars")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Username")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Min Lengs is 3 chars / Max length is 50 chars")]
        public string Username { get; set; }
        [Required]
        [Display(Name = "Password")]
        [StringLength(24, MinimumLength = 6, ErrorMessage = "Min Lengs is 6 chars / Max length is 24 chars")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Status")]
        [Required]
        public string Role { get; set; }
        [Display(Name = "Registration Date")]
        public DateTime RegDate { get; set; }

    }
}