using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlinePhotoAlbum.BLL.Interfaces
{
    public interface IEntityService<T> : IDisposable where T : class
    {
        IEnumerable<T> GetAll();
        void Add(T item);
        void Edit(T item);
    }
}
