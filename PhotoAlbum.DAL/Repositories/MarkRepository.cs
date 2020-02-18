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
    class MarkRepository : IRepository<Mark>
    {
        private AlbumContext db;

        public MarkRepository(AlbumContext context)
        {
            this.db = context;
        }

        public IEnumerable<Mark> GetAll()
        {
            return db.Marks;
        }

        public Mark Get(int id)
        {
            return db.Marks.Find(id);
        }

        public void Create(Mark Mark)
        {
            db.Marks.Add(Mark);
        }

        public void Update(Mark Mark)
        {
            db.Entry(Mark).State = EntityState.Modified;
        }

        public IEnumerable<Mark> Find(Func<Mark, Boolean> predicate)
        {
            return db.Marks.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Mark mark = db.Marks.Find(id);
            if (mark != null)
                db.Marks.Remove(mark);
        }
    }
}
