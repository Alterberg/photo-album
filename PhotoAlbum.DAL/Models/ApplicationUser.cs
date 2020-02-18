using Microsoft.AspNet.Identity.EntityFramework;
using OnlinePhotoAlbum.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePhotoAlbum.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public UserProfile UserProfile { get; set; }
    }
}
