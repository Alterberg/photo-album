using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OnlinePhotoAlbum.BLL.DTO
{
    public class MarkDTO
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public DateTime Time { get; set; }
        public string AuthorId { get; set; }
        public int PictureId { get; set; }
    }
}