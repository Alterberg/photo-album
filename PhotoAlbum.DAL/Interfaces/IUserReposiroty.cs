using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePhotoAlbum.DAL.Interfaces
{
    public interface IUserReposiroty<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(string id);
        IEnumerable<T> Find(Func<T, Boolean> predicate);
        void Create(T item);
        void Update(T item);
        void Delete(string id);
    }
}
