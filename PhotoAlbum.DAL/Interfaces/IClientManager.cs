using OnlinePhotoAlbum.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePhotoAlbum.DAL.Interfaces
{
    public interface IClientManager : IDisposable
    {
        void Create(UserProfile item);
    }
}
