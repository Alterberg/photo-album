
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlinePhotoAlbum.DAL.Interfaces;
using OnlinePhotoAlbum.DAL.Models;
using OnlinePhotoAlbum.BLL.Interfaces;
using OnlinePhotoAlbum.BLL.DTO;
using OnlinePhotoAlbum.BLL.Infrastructure;
using AutoMapper;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using OnlinePhotoAlbum.DAL.Repositories;
using Microsoft.AspNet.Identity.EntityFramework;

namespace OnlinePhotoAlbum.BLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }

        public UserService()
        {
            Database = new EFUnitOfWork("AlbumConnection");
        }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            ApplicationUser user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = new ApplicationUser { Email = userDto.Email, UserName = userDto.UserName };
                var result = await Database.UserManager.CreateAsync(user, userDto.Password);
                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                // добавляем роль
                await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                // создаем профиль клиента
                UserProfile clientProfile = new UserProfile { Id = user.Id, Name = userDto.Name, UserName = userDto.UserName, RegDate = userDto.RegDate };
                Database.ClientManager.Create(clientProfile);
                await Database.SaveAsync();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
        }

        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            // находим пользователя
            ApplicationUser user = await Database.UserManager.FindAsync(userDto.UserName, userDto.Password);
            // авторизуем его и возвращаем объект ClaimsIdentity
            if (user != null)
                claim = await Database.UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }

        // начальная инициализация бд
        public async Task SetInitialData(UserDTO adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new ApplicationRole { Name = roleName };
                    await Database.RoleManager.CreateAsync(role);
                }
            }
            await Create(adminDto);
        }

        public UserDTO GetById(string id)
        {
            if (id == null)
                throw new ValidationException("ID пользователя не установлено", "");
            var user = Database.Users.Get(id);
            if (user == null)
                throw new ValidationException("Пользователь не найден", "");

            return new UserDTO
            {
                Id = user.Id,
                Role = Database.UserManager.GetRoles(user.Id).FirstOrDefault(),
                Name = user.Name,
                UserName = user.UserName,
                //Password = user.Password,
                RegDate = user.RegDate
            };
        }

        public IEnumerable<UserDTO> GetAll()
        {
            //var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserProfile, UserDTO>()).CreateMapper();
            //return mapper.Map<IEnumerable<UserProfile>, List<UserDTO>>(Database.Users.GetAll());

            var users = Database.Users.GetAll().ToList();
            IList<UserDTO> userDtos = new List<UserDTO>();

            if (users.Count > 0)
            {
                foreach (UserProfile u in users)
                {
                    userDtos.Add(new UserDTO
                    {
                        Id = u.Id,
                        Name = u.Name,
                        UserName = u.UserName,
                        Role = Database.UserManager.GetRoles(u.Id).FirstOrDefault(),
                        RegDate = u.RegDate
                    });
                }
                return userDtos;
            }
            throw new ValidationException("Nothing found", "");
        }

        public IEnumerable<UserDTO> SearchAllByName(string name)
        {
            IEnumerable<UserProfile> users = Database.Users.GetAll().Where(u => u.Name.Contains(name));

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserProfile, UserDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<UserProfile>, List<UserDTO>>(users);
        }

        public IEnumerable<UserDTO> SearchAllByUsername(string username)
        {
            IEnumerable<UserProfile> users = Database.Users.GetAll().Where(u => u.UserName.Contains(username));

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserProfile, UserDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<UserProfile>, List<UserDTO>>(users);
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public void Add(UserDTO userDto)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            if (id == null)
                throw new ValidationException("ID пользователя не установлено", "");
            var user = Database.Users.Get(id);
            if (user == null)
                throw new ValidationException("Пользователь не найден", "Id");

            var userPhotoIds = Database.Photos.GetAll().Where(p => p.AuthorId == id).Select(p => p.Id);

            foreach (int photoId in userPhotoIds)
            {
                Database.Photos.Delete(photoId);
            }

            Database.Users.Delete(id);
            Database.Save();
        }

        public void Edit(UserDTO userDto)
        {
            Database.UserManager.RemoveFromRole(userDto.Id, Database.UserManager.GetRoles(userDto.Id).FirstOrDefault());
            Database.UserManager.AddToRole(userDto.Id, userDto.Role);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserProfile>()).CreateMapper();
            var user =  mapper.Map<UserDTO, UserProfile>(userDto);
            Database.Users.Update(user);
        }

        public List<string> GetRoles()
        {
            var roles = Database.RoleManager.Roles.Select(x => x.Name).ToList();
            return roles;
        }

        public IEnumerable<UserDTO> SearchByUsername(IEnumerable<UserDTO> users, string username)
        {
            //return users.Where(u => u.UserName.ToLower().Contains(username.ToLower()));
            return SearchAllByUsername(username);
        }

        public IEnumerable<UserDTO> SearchByRole(IEnumerable<UserDTO> users, string role)
        {
            return users.Where(u => u.Role == role);
        }
    }
}
