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
    internal class DbInitializer<TContext> : DropCreateDatabaseIfModelChanges<AlbumContext> where TContext : DbContext
    {
        protected override void Seed(AlbumContext db)
        {
            var roleManager = new ApplicationRoleManager(new RoleStore<ApplicationRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ClientManager clientManager = new ClientManager(db);

            string[] roles = new string[] { "user", "admin", "moderator" };

            //foreach (string role in roles)
            //{
            //    if (!db.Roles.Any(r => r.Name == role))
            //    {
            //        db.Roles.Add(new IdentityRole(role));
            //    }
            //}

            foreach (string role in roles)
            {
                if (!roleManager.RoleExists(role))
                {
                    roleManager.Create(new ApplicationRole(role));
                }
            }

            ApplicationUser user = new ApplicationUser
            {
                Email = "admin@gmail.com",
                UserName = "admin",
                PasswordHash = userManager.PasswordHasher.HashPassword("adminadmin"),
            };
            userManager.Create(user);
            // добавляем роль
            userManager.AddToRole(user.Id, "admin");
            // создаем профиль клиента
            UserProfile clientProfile = new UserProfile { Id = user.Id, Name = "admin", UserName = user.UserName, RegDate = DateTime.Now };
            clientManager.Create(clientProfile);
            db.SaveChanges();

            //var users = new UserProfile[]
            //{
            //    new UserProfile { Name = "Vasia"/*, RoleId = roles.Single( i => i.Name == "User").Id*/, UserName = "Vasia1", RegDate = DateTime.Now },
            //    new UserProfile { Name = "Misha"/*, RoleId = roles.Single( i => i.Name == "User").Id*/, UserName = "Misha2", RegDate = DateTime.Now },
            //    new UserProfile { Name = "Stepka"/*, RoleId = roles.Single( i => i.Name == "Admin").Id*/, UserName = "Stepka3", RegDate = DateTime.Now }
            //};

            //foreach (UserProfile u in users)
            //{
            //    db.UserProfiles.Add(u);
            //}
            //db.SaveChanges();

            var photos = new Photo[]
            {
                new Photo { Name = "NokiaIMG", /*AuthorId = users.Single( i => i.UserName == "Vasia1").Id,*/ Description = "NokiaIMG description", Path = "/Files/NokiaIMG.jpg", UploadTime = DateTime.Now },
                new Photo { Name = "HuaweiIMG", /*AuthorId = users.Single( i => i.UserName == "Misha2").Id,*/ Description = "HuaweiIMG description", Path = "/Files/HuaweiIMG.jpg", UploadTime = DateTime.Now },
                new Photo { Name = "IphoneIMG", /*AuthorId = users.Single( i => i.UserName == "Stepka3").Id,*/ Description = "IphoneIMG description", Path = "/Files/IphoneIMG.jpg", UploadTime = DateTime.Now }
            };

            foreach (Photo p in photos)
            {
                db.Photos.Add(p);
            }

            db.SaveChanges();

            var marks = new Mark[]
            {
                new Mark { /*AuthorId = users.Single( i => i.UserName == "Vasia1").Id,*/ PictureId = photos.Single( i => i.Name == "HuaweiIMG").Id, Score = 5, Time = DateTime.Now },
                new Mark { /*AuthorId = users.Single( i => i.UserName == "Misha2").Id,*/ PictureId = photos.Single( i => i.Name == "IphoneIMG").Id, Score = 4, Time = DateTime.Now },
                new Mark { /*AuthorId = users.Single( i => i.UserName == "Stepka3").Id,*/ PictureId = photos.Single( i => i.Name == "NokiaIMG").Id, Score = 3, Time = DateTime.Now },
                new Mark { /*AuthorId = users.Single( i => i.UserName == "Vasia1").Id,*/ PictureId = photos.Single( i => i.Name == "IphoneIMG").Id, Score = 5, Time = DateTime.Now }
            };

            foreach (Mark m in marks)
            {
                db.Marks.Add(m);
            }
            db.SaveChanges();

        }
    }
}