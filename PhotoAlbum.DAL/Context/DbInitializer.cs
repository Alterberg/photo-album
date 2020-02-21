using OnlinePhotoAlbum.DAL.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using OnlinePhotoAlbum.DAL.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlinePhotoAlbum.DAL.Repositories;
using Microsoft.AspNet.Identity;

namespace OnlinePhotoAlbum.DAL.Context
{
    internal class DbInitializer<TContext> : DropCreateDatabaseAlways<AlbumContext> where TContext : DbContext
    {
        protected override void Seed(AlbumContext db)
        {
            var roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ClientManager clientManager = new ClientManager(db);

            string[] roles = new string[] { "user", "admin", "moderator" };

            foreach (string role in roles)
            {
                if (!roleManager.RoleExists(role))
                {
                    roleManager.Create(new ApplicationRole(role));
                }
            }

            ApplicationUser admin = new ApplicationUser
            {
                Email = "admin@gmail.com",
                UserName = "admin",
                PasswordHash = userManager.PasswordHasher.HashPassword("adminadmin"),
            };
            userManager.Create(admin);
            userManager.AddToRole(admin.Id, "admin");
            UserProfile adminProfile = new UserProfile { Id = admin.Id, Name = "admin", UserName = admin.UserName, RegDate = DateTime.Now };
            clientManager.Create(adminProfile);
            db.SaveChanges();

            ApplicationUser user = new ApplicationUser
            {
                Email = "user@gmail.com",
                UserName = "user",
                PasswordHash = userManager.PasswordHasher.HashPassword("useruser"),
            };
            userManager.Create(user);
            userManager.AddToRole(user.Id, "user");
            UserProfile userProfile = new UserProfile { Id = user.Id, Name = "admin", UserName = user.UserName, RegDate = DateTime.Now };
            clientManager.Create(userProfile);
            db.SaveChanges();

            var photos = new Photo[]
            {
                new Photo { Name = "NokiaIMG", AuthorId = admin.Id, Description = "NokiaIMG description", Path = "/Files/NokiaIMG.jpg", UploadTime = DateTime.Now },
                new Photo { Name = "HuaweiIMG", AuthorId = admin.Id, Description = "HuaweiIMG description", Path = "/Files/HuaweiIMG.jpg", UploadTime = DateTime.Now },
                new Photo { Name = "IphoneIMG", AuthorId = admin.Id, Description = "IphoneIMG description", Path = "/Files/IphoneIMG.jpg", UploadTime = DateTime.Now }
            };

            foreach (Photo p in photos)
            {
                db.Photos.Add(p);
            }

            db.SaveChanges();

            var marks = new Mark[]
            {
                new Mark { AuthorId = user.Id, PictureId = photos.Single( i => i.Name == "HuaweiIMG").Id, Score = 5, Time = DateTime.Now },
                new Mark { AuthorId = user.Id, PictureId = photos.Single( i => i.Name == "IphoneIMG").Id, Score = 4, Time = DateTime.Now },
                new Mark { AuthorId = user.Id, PictureId = photos.Single( i => i.Name == "NokiaIMG").Id, Score = 3, Time = DateTime.Now },
                new Mark { AuthorId = user.Id, PictureId = photos.Single( i => i.Name == "IphoneIMG").Id, Score = 5, Time = DateTime.Now }
            };

            foreach (Mark m in marks)
            {
                db.Marks.Add(m);
            }
            db.SaveChanges();

        }
    }
}