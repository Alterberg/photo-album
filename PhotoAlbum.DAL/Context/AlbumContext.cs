using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlinePhotoAlbum.DAL.Models;

namespace OnlinePhotoAlbum.DAL.Context
{
    public class AlbumContext : IdentityDbContext<ApplicationUser>
    {
        static AlbumContext()
        {
            Database.SetInitializer<AlbumContext>(new DbInitializer<AlbumContext>());
        }

        public AlbumContext()
        : base("AlbumConnection")
        { }

        public AlbumContext(string connectionString)
        : base(connectionString)
        { }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Mark> Marks { get; set; }
        //public DbSet<Role> Roles { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)

        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserProfile>()
            .HasMany(e => e.Photos)
            .WithOptional(e => e.Author)//WithRequired
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserProfile>()
            .HasMany(e => e.Marks)
            .WithOptional(e => e.Author)
            .WillCascadeOnDelete(true);

            modelBuilder.Entity<Photo>()
            .HasMany(e => e.Marks)
            .WithOptional(e => e.Picture)
            .WillCascadeOnDelete(true);
        }
    }
}