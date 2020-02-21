using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using OnlinePhotoAlbum.BLL.DTO;
using OnlinePhotoAlbum.BLL.Interfaces;
using OnlinePhotoAlbum.BLL.Services;
using OnlinePhotoAlbum.Web.Models;


namespace OnlinePhotoAlbum.Web.Controllers
{
    public class HomeController : Controller
    {
        IPhotoService photoService;

        public HomeController(IPhotoService photoService)
        {
            this.photoService = photoService;
        }

        public ActionResult Index(int page = 1, string selPageSize = "3")
        {
            ViewBag.PageSize = selPageSize;

            int pageSize = int.Parse(selPageSize);
            IEnumerable<PhotoDTO> photoDtos = photoService.GetAll().OrderByDescending(p => p.Mark);

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<PhotoDTO, IndexPhotoViewModel>()).CreateMapper();
            var photos = mapper.Map<IEnumerable<PhotoDTO>, List<IndexPhotoViewModel>>(photoDtos);

            IEnumerable<IndexPhotoViewModel> phonesPerPages = photos.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = photos.Count };
            PhotoPageViewModel ppvm = new PhotoPageViewModel { PageInfo = pageInfo, Photos = phonesPerPages };

            ViewBag.SelPageSize = CreatePageSizeSelectList();
            return View(ppvm);
        }

        public PartialViewResult _PhotoPartialView(int page = 1, string selPageSize = "3")
        {
            ViewBag.PageSize = selPageSize;

            int pageSize = int.Parse(selPageSize);
            IEnumerable<PhotoDTO> photoDtos = photoService.GetAll().OrderByDescending(p => p.Mark);

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<PhotoDTO, IndexPhotoViewModel>()).CreateMapper();
            var photos = mapper.Map<IEnumerable<PhotoDTO>, List<IndexPhotoViewModel>>(photoDtos);

            IEnumerable<IndexPhotoViewModel> phonesPerPages = photos.Skip((page - 1) * pageSize).Take(pageSize);
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = photos.Count };
            PhotoPageViewModel ppvm = new PhotoPageViewModel { PageInfo = pageInfo, Photos = phonesPerPages };

            ViewBag.SelPageSize = CreatePageSizeSelectList();
            return PartialView("_PhotoPartialView",ppvm);
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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Photos()
        {
            return View();
        }
    }
}
