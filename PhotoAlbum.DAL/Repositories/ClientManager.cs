using OnlinePhotoAlbum.DAL.Context;
using OnlinePhotoAlbum.DAL.Models;
using OnlinePhotoAlbum.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePhotoAlbum.DAL.Repositories
{
    public class ClientManager : IClientManager
    {
        public AlbumContext Database { get; set; }
        public ClientManager(AlbumContext db)
        {
            Database = db;
        }

        public void Create(UserProfile item)
        {
            Database.UserProfiles.Add(item);
            Database.SaveChanges();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
