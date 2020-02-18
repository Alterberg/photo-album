using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlinePhotoAlbum.DAL.Models
{
    public class Mark
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public DateTime Time { get; set; }
        public string AuthorId { get; set; }
        public UserProfile Author { get; set; }
        public int? PictureId { get; set; }
        public Photo Picture { get; set; }
    }
}