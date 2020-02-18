using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePhotoAlbum.Web.Models
{
    public class PhotoPageViewModel
    {
        public IEnumerable<IndexPhotoViewModel> Photos { get; set; }
        public PageInfo PageInfo { get; set; }
    }
}