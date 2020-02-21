using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlinePhotoAlbum.BLL.DTO;
using OnlinePhotoAlbum.BLL.Services;
using OnlinePhotoAlbum.BLL.Interfaces;
using OnlinePhotoAlbum.BLL.Infrastructure;
using OnlinePhotoAlbum.Web.Models;
using System.Data.Entity;
using System.IO;
using AutoMapper;
using Microsoft.AspNet.Identity;
using PagedList;

namespace OnlinePhotoAlbum.Web.Controllers
{
    public class PhotoController : Controller
    {
        readonly IPhotoService photoService;
        readonly IUserService userService;
        
        public PhotoController(IPhotoService photoService, IUserService userService)
        {
            this.photoService = photoService;
            this.userService = userService;
        }

        public ActionResult Index(string selPhotoName, string selUsername, double? markFrom, double? markTo, int page = 1, string selPageSize = "3")
        {
            ViewBag.Title = "Last added photos:";
            ViewBag.PageSize = selPageSize;
            ViewBag.PhotoName = selPhotoName;
            ViewBag.Username = selUsername;
            ViewBag.From = markFrom;
            ViewBag.To = markTo;

            int pageSize = int.Parse(selPageSize); 
            IEnumerable<PhotoDTO> photoDtos = photoService.GetAll().OrderByDescending(p => p.UploadTime);

            if (!string.IsNullOrWhiteSpace(selPhotoName))
                photoDtos = photoDtos.Where(e => e.Name.ToLower().Contains(selPhotoName.ToLower()));
            if (!string.IsNullOrWhiteSpace(selUsername))
                photoDtos = photoDtos.Where(e => e.AuthorUsername.ToLower().Contains(selUsername.ToLower()));
            if (markFrom.HasValue)
                photoDtos = photoDtos.Where(e => e.Mark >= markFrom.Value);
            if (markTo.HasValue)
                photoDtos = photoDtos.Where(e => e.Mark <= markTo.Value);

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<PhotoDTO, IndexPhotoViewModel>()).CreateMapper();
            var photos = mapper.Map<IEnumerable<PhotoDTO>, List<IndexPhotoViewModel>>(photoDtos);

            IEnumerable<IndexPhotoViewModel> phonesPerPages = photos.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = photos.Count };
            PhotoPageViewModel ppvm = new PhotoPageViewModel { PageInfo = pageInfo, Photos = phonesPerPages };

            ViewBag.SelPageSize = CreatePageSizeSelectList();
            return View(ppvm);
        }

        public PartialViewResult _PhotoPartialView(string selPhotoName, string selUsername, double? markFrom, double? markTo, int page = 1, string selPageSize = "3")
        {
            ViewBag.PageSize = selPageSize;
            ViewBag.PhotoName = selPhotoName;
            ViewBag.Username = selUsername;
            ViewBag.From = markFrom;
            ViewBag.To = markTo;

            int pageSize = int.Parse(selPageSize);
            IEnumerable<PhotoDTO> photoDtos = photoService.GetAll().OrderByDescending(p => p.UploadTime);

            if (!string.IsNullOrWhiteSpace(selPhotoName))
                photoDtos = photoDtos.Where(e => e.Name.ToLower().Contains(selPhotoName.ToLower()));
            if (!string.IsNullOrWhiteSpace(selUsername))
                photoDtos = photoDtos.Where(e => e.AuthorUsername.ToLower().Contains(selUsername.ToLower()));
            if (markFrom.HasValue)
                photoDtos = photoDtos.Where(e => e.Mark >= markFrom.Value);
            if (markTo.HasValue)
                photoDtos = photoDtos.Where(e => e.Mark <= markTo.Value);

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<PhotoDTO, IndexPhotoViewModel>()).CreateMapper();
            var photos = mapper.Map<IEnumerable<PhotoDTO>, List<IndexPhotoViewModel>>(photoDtos);

            IEnumerable<IndexPhotoViewModel> phonesPerPages = photos.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = photos.Count };
            PhotoPageViewModel ppvm = new PhotoPageViewModel { PageInfo = pageInfo, Photos = phonesPerPages };

            ViewBag.SelPageSize = CreatePageSizeSelectList();
            return PartialView("_PhotoPartialView", ppvm);
        }

        List<SelectListItem> CreatePageSizeSelectList()
        {
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem
                {
                    Text = "3", Value = "3"
                },
                new SelectListItem
                {
                    Text = "9", Value = "9"
                },
                new SelectListItem
                {
                    Text = "27", Value = "27"
                },
                new SelectListItem
                {
                    Text = "99", Value = "99"
                }
            };
            return list;
        }

        [Authorize]
        public ActionResult MyAlbum()
        {
            IEnumerable<PhotoDTO> photoDtos = photoService.GetAll()
                .Where(p => p.AuthorId == User.Identity.GetUserId()).OrderByDescending(p => p.UploadTime);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<PhotoDTO, IndexPhotoViewModel>()).CreateMapper();
            var photos = mapper.Map<IEnumerable<PhotoDTO>, List<IndexPhotoViewModel>>(photoDtos);
            return View(photos);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult Create(DetailPhotoViewModel pic, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null && uploadImage.ContentLength <= 2000000 && uploadImage.ContentType.Contains("image"))
            {
                string fileName = System.IO.Path.GetFileName(uploadImage.FileName);

                uploadImage.SaveAs(Server.MapPath("~/Files/" + fileName));

                PhotoDTO photo = new PhotoDTO { Description = pic.Description, Name = fileName, UploadTime = DateTime.Now, 
                    Path = "/Files/" + fileName, AuthorId = User.Identity.GetUserId() };
                photoService.Add(photo);

                return RedirectToAction("Index");
            }
            return View("Error");
        }

        [Authorize]
        public ActionResult Delete(int? id)
        {
            PhotoDTO photoDto;
            try
            {
                photoDto = photoService.GetById(id);
            }
            catch (ValidationException ex)
            {
                return Content(ex.Message);
            }
            if (User.Identity.GetUserName() == photoDto.AuthorUsername || User.IsInRole("moderator"))
            {
                ViewBag.Id = id;
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<PhotoDTO, DetailPhotoViewModel>()).CreateMapper();
                DetailPhotoViewModel photoView = mapper.Map<PhotoDTO, DetailPhotoViewModel>(photoDto);
                return View(photoView);
            }
            else return Content("You don`t have anought permissions!");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        { 
            try
            {
                photoService.Delete(id);
            }
            catch (ValidationException ex)
            {
                return Content(ex.Message);
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(int? id)
        {

            PhotoDTO photoDto;
            try
            {
                photoDto = photoService.GetById(id);
            }
            catch (ValidationException ex)
            {
                return Content(ex.Message);
            }
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<PhotoDTO, EditPhotoViewModel>()).CreateMapper();
            EditPhotoViewModel photoView = mapper.Map<PhotoDTO, EditPhotoViewModel>(photoDto);
            return View(photoView);
        }

        [HttpPost]
        public ActionResult Edit(EditPhotoViewModel photoView)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<EditPhotoViewModel, PhotoDTO>()).CreateMapper();
                PhotoDTO photoDto = mapper.Map<EditPhotoViewModel, PhotoDTO>(photoView);
                try
                {
                    photoService.Edit(photoDto);
                }
                catch (ValidationException)
                {
                    return HttpNotFound();
                }

                return RedirectToAction("Index");
            }
            else return View(photoView);
        }


        [HttpPost]
        public ActionResult PhotoSearch(string name)
        {
            ViewBag.Search = name;

            IEnumerable<PhotoDTO> photoDtos;
            try
            {
                photoDtos = photoService.SearchAllByName(name);
            }
            catch (ValidationException ex)
            {
                return Content(ex.Message);
            }

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<PhotoDTO, IndexPhotoViewModel>()).CreateMapper();
            var photoViews = mapper.Map<IEnumerable<PhotoDTO>, List<IndexPhotoViewModel>>(photoDtos);

            return PartialView(photoViews);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddMark(int? score, int? id)
        {
            if (score == null) return HttpNotFound();

            MarkDTO mark = new MarkDTO()
            {
                AuthorId = User.Identity.GetUserId(),
                PictureId = id.Value,
                Score = score.Value,
                Time = DateTime.Now
            };

            try
            {
                photoService.AddMark(mark);
            }
            catch (ValidationException ex)
            {
                return Content(ex.Message);
            }
            int photoId = id.Value;
            return RedirectToAction("Detail", new { id = photoId });
        }

        public ActionResult Detail(int? id)
        {
            PhotoDTO photoDto;
            try
            {
                photoDto = photoService.GetById(id);
            }
            catch (ValidationException ex)
            {
                return Content(ex.Message);
            }

            ViewBag.returnUrl = Request.UrlReferrer;
            ViewBag.HaveMark = photoService.HaveMark(User.Identity.GetUserId(), photoDto.Id);

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<PhotoDTO, DetailPhotoViewModel>()).CreateMapper();
            DetailPhotoViewModel photoView = mapper.Map<PhotoDTO, DetailPhotoViewModel>(photoDto);
            return View(photoView);
        }

        [HttpPost]
        public ActionResult Back(string returnUrl)
        {
            return Redirect(returnUrl);
        }
    }
}