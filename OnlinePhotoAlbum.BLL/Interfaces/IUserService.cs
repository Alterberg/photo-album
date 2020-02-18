using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using OnlinePhotoAlbum.BLL.DTO;
using OnlinePhotoAlbum.BLL.Infrastructure;
using OnlinePhotoAlbum.DAL.Models;

namespace OnlinePhotoAlbum.BLL.Interfaces
{
    public interface IUserService : IEntityService<UserDTO>
    {
        UserDTO GetById(string id);
        void Delete(string id);
        Task<OperationDetails> Create(UserDTO userDto);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        Task SetInitialData(UserDTO adminDto, List<string> roles);
        List<string> GetRoles();
        IEnumerable<UserDTO> SearchAllByName(string name);
        IEnumerable<UserDTO> SearchAllByUsername(string username);
        IEnumerable<UserDTO> SearchByUsername(IEnumerable<UserDTO> users, string username);
        IEnumerable<UserDTO> SearchByRole(IEnumerable<UserDTO> users, string role);

    }
}
