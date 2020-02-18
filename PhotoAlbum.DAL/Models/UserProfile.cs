using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OnlinePhotoAlbum.DAL.Models;
using System.Linq;
using System.Web;


namespace OnlinePhotoAlbum.DAL.Models
{
    public class UserProfile
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public DateTime RegDate { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public ICollection<Photo> Photos { get; set; }
        public ICollection<Mark> Marks { get; set; }
    }
}