using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePhotoAlbum.BLL.DTO;

namespace OnlinePhotoAlbum.BLL.Interfaces
{
    public interface IPhotoService : IEntityService<PhotoDTO>
    {
        PhotoDTO GetById(int? id);
        void Delete(int? id);
        IEnumerable<PhotoDTO> SearchAllByName(string name);
        void AddMark(MarkDTO markDto);
        bool HaveMark(string userId, int photoId);

    }
}
