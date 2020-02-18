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
    class UserRepository : IUserReposiroty<UserProfile>
    {
        private AlbumContext context;

        public UserRepository(AlbumContext context)
        {
            this.context = context;
        }
        public IEnumerable<UserProfile> GetAll()
        {
            return context.UserProfiles;
        }

        public UserProfile Get(string id)
        {
            return context.UserProfiles.Find(id);
        }

        public void Create(UserProfile item)
        {
            context.UserProfiles.Add(item);
        }

        public void Update(UserProfile item)
        {
            context.Entry(item).State = EntityState.Modified;
        }

        public IEnumerable<UserProfile> Find(Func<UserProfile, bool> predicate)
        {
            return context.UserProfiles.Where(predicate).ToList();
        }

        public void Delete(string id)
        {
            UserProfile user = context.UserProfiles.Find(id);
            if (user != null)
                context.UserProfiles.Remove(user);
        }
    }
}
