using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePhotoAlbum.DAL.Interfaces;
using OnlinePhotoAlbum.DAL.Models;
using OnlinePhotoAlbum.DAL.Context;
using System.Data.Entity;

namespace OnlinePhotoAlbum.DAL.Repositories
{
    class PhotoRepository : IRepository<Photo>
    {
        private AlbumContext context;

        public PhotoRepository(AlbumContext context)
        {
            this.context = context;
        }
        public IEnumerable<Photo> GetAll()
        {
            return context.Photos;
        }

        public Photo Get(int id)
        {
            return context.Photos.Find(id);
        }

        public void Create(Photo item)
        {
            context.Photos.Add(item);
        }

        public void Update(Photo item)
        {
            context.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<Photo> Find(Func<Photo, bool> predicate)
        {
            return context.Photos.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Photo photo = context.Photos.Find(id);
            if (photo != null)
                context.Photos.Remove(photo);
        }
    }
}
