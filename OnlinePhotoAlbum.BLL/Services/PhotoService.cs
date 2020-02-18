using AutoMapper;
using OnlinePhotoAlbum.BLL.DTO;
using OnlinePhotoAlbum.BLL.Infrastructure;
using OnlinePhotoAlbum.BLL.Interfaces;
using OnlinePhotoAlbum.DAL.Interfaces;
using OnlinePhotoAlbum.DAL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OnlinePhotoAlbum.BLL.Services
{
    public class PhotoService : IPhotoService
    {
        IUnitOfWork Database { get; set; }

        public PhotoService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public PhotoDTO GetById(int? id)
        {
            if (id == null)
                throw new ValidationException("ID фотографии не установлено", "");
            var photo = Database.Photos.Get(id.Value);
            if (photo == null)
                throw new ValidationException("Фотография не найдена", "");
            IEnumerable<Mark> marks = Database.Marks.Find(m => m.PictureId == id);

            return new PhotoDTO
            {
                Id = photo.Id,
                Description = photo.Description == null ? "Description is empty..." : photo.Description,
                Mark = marks.Count() <= 0 ? 0 : @Math.Round(marks.Select(m => m.Score).Average(x => x), 2),
                Name = photo.Name,
                Path = photo.Path,
                AuthorUsername = photo.AuthorId == null ? "Anonimus" : Database.Users.Get(photo.AuthorId).UserName,
                AuthorId = photo.AuthorId,
                UploadTime = photo.UploadTime
            };
        }

        public IEnumerable<PhotoDTO> GetAll()
        {
            //var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Photo, PhotoDTO>()).CreateMapper();
            //return mapper.Map<IEnumerable<Photo>, List<PhotoDTO>>(Database.Photos.GetAll());
            var photos = Database.Photos.GetAll().ToList();
            IList<PhotoDTO> photoDtos = new List<PhotoDTO>();

            if (photos.Count > 0)
            {
                foreach (Photo p in photos)
                {
                    IEnumerable<Mark> marks = Database.Marks.Find(m => m.PictureId == p.Id);
                    photoDtos.Add(new PhotoDTO
                    {
                        Id = p.Id,
                        Description = p.Description ?? "Description is empty...",
                        Mark = marks.Count() <= 0 ? 0 : @Math.Round(marks.Select(m => m.Score).Average(x => x), 2),
                        Name = p.Name,
                        Path = p.Path,
                        AuthorUsername = p.AuthorId == null ? "Anonimus" : Database.Users.Get(p.AuthorId).UserName,
                        AuthorId = p.AuthorId,
                        UploadTime = p.UploadTime
                    });
                }
            }
            return photoDtos;
        }

        public IEnumerable<PhotoDTO> SearchAllByName(string name)
        {
            if (name == null) throw new ValidationException("условие для поиска не задано", "");

            IEnumerable<Photo> photos = Database.Photos.GetAll().Where(p => p.Name.Contains(name));
            if (photos.Count() <= 0) throw new ValidationException("Nothing found", "");

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Photo, PhotoDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Photo>, List<PhotoDTO>>(photos);
        }

        public void Add(PhotoDTO photoDto)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<PhotoDTO, Photo>()).CreateMapper();
            Photo photo = mapper.Map<PhotoDTO, Photo>(photoDto);

            Database.Photos.Create(photo);
            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public void Delete(int? id)
        {
            if (id == null)
                throw new ValidationException("ID фотографии не установлено", "");
            var photo = Database.Photos.Get(id.Value);
            if (photo == null)
                throw new ValidationException("Фотография не найдена", "");

            Database.Photos.Delete(id.Value);
            Database.Save();
        }

        public void Edit(PhotoDTO photoDto)
        {
            if (photoDto != null)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<PhotoDTO, Photo>()).CreateMapper();
                Photo photo = mapper.Map<PhotoDTO, Photo>(photoDto);

                Database.Photos.Update(photo);
                Database.Save();
            }
            else
            {
                throw new ValidationException("Данные заполнены неверно", "");
            }
        }

        public void AddMark(MarkDTO markDto)
        {
            Photo photo = Database.Photos.Get(markDto.PictureId);
            if (photo != null)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<MarkDTO, Mark>()).CreateMapper();
                Mark mark = mapper.Map<MarkDTO, Mark>(markDto);
                //Mark mark = new Mark
                //{
                //    Picture = photo,
                //    Score = markDto.Score,
                //    Time = markDto.Time,
                //    AuthorId = markDto.AuthorId,
                //};
                Database.Marks.Create(mark);
                Database.Save();

            }
            else throw new ValidationException("Photo can not be found", "");
        }

        public bool HaveMark(string userId, int photoId)
        {
            var marks = Database.Photos.Get(photoId).Marks;
            if (marks != null)
            {
                if (marks.Where(m => m.AuthorId == userId).Count() == 0) return false;
            }
            return true;
        }
    }
}
