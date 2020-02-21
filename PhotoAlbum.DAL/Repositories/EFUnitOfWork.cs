using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePhotoAlbum.DAL.Interfaces;
using OnlinePhotoAlbum.DAL.Context;
using OnlinePhotoAlbum.DAL.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlinePhotoAlbum.DAL.Identity;
using OnlinePhotoAlbum.DAL.Repositories;

namespace OnlinePhotoAlbum.DAL.Repositories
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly AlbumContext db;

        private PhotoRepository photoRepository;
        private UserRepository userRepository;
        private MarkRepository markRepository;
        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;
        private IClientManager clientManager;

        public EFUnitOfWork(string connectionString)
        {
            db = new AlbumContext(connectionString);
            userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
            clientManager = new ClientManager(db);
        }

        public IRepository<Photo> Photos
        {
            get
            {
                if (photoRepository == null)
                    photoRepository = new PhotoRepository(db);
                return photoRepository;
            }
        }

        public IUserReposiroty<UserProfile> Users
        {
            get
            {
                if (userRepository == null)
                    userRepository = new UserRepository(db);
                return userRepository;
            }
        }

        public IRepository<Mark> Marks
        {
            get
            {
                if (markRepository == null)
                    markRepository = new MarkRepository(db);
                return markRepository;
            }
        }

        public ApplicationUserManager UserManager
        {
            get { return userManager; }
        }

        public IClientManager ClientManager
        {
            get { return clientManager; }
        }

        public ApplicationRoleManager RoleManager
        {
            get 
            {
                if (roleManager == null)
                    roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
                return roleManager;
            }
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }

        public void Save()
        {
             db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                    userManager.Dispose();
                    roleManager.Dispose();
                    clientManager.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
