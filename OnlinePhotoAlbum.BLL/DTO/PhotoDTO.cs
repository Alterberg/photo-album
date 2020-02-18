using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlinePhotoAlbum.BLL.DTO
{
    public class PhotoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Path { get; set; }
        public DateTime UploadTime { get; set; }
        public string AuthorUsername { get; set; }
        public string AuthorId { get; set; }
        public double? Mark { get; set; }
    }
}