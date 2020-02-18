using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlinePhotoAlbum.DAL.Models;
using OnlinePhotoAlbum.DAL.Identity;
using OnlinePhotoAlbum.DAL.Interfaces;

namespace OnlinePhotoAlbum.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserReposiroty<UserProfile> Users { get; }
        IRepository<Photo> Photos { get; }
        IRepository<Mark> Marks { get; }
        //IRepository<Role> Roles { get; }
        ApplicationUserManager UserManager { get; }
        IClientManager ClientManager { get; }
        ApplicationRoleManager RoleManager { get; }
        Task SaveAsync();
        void Save();
    }
}
