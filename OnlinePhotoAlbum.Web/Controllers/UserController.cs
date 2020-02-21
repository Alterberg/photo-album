using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using OnlinePhotoAlbum.BLL.DTO;
using OnlinePhotoAlbum.BLL.Interfaces;
using OnlinePhotoAlbum.BLL.Infrastructure;
using OnlinePhotoAlbum.Web.Models;
using Microsoft.AspNet.Identity;

namespace OnlinePhotoAlbum.Web.Controllers
{
    public class UserController : Controller
    {
        IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        public ActionResult Index()
        {
            IEnumerable<UserDTO> userDtos = userService.GetAll();
            if (userDtos.Count() <= 0)
            {
                return HttpNotFound();
            }
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserViewModel>()).CreateMapper();
            var users = mapper.Map<IEnumerable<UserDTO>, List<UserViewModel>>(userDtos);

            ViewBag.SelRoleName = CreateRoleNamesSelectList();

            return View(users);
        }

        public PartialViewResult SelectData(string selUsername, string selRoleName)
        {
            var userDtos = userService.GetAll();
            if (!string.IsNullOrWhiteSpace(selUsername))
                userDtos = userDtos.Where(u => u.UserName.Contains(selUsername));
            if (selRoleName != null && selRoleName != "All")
                userDtos = userDtos.Where(u => u.Role == selRoleName);

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserViewModel>()).CreateMapper();
            var users = mapper.Map<IEnumerable<UserDTO>, List<UserViewModel>>(userDtos);

            return PartialView("_UserTable", users);
        }

        List<SelectListItem> CreateRoleNamesSelectList()
        {
            List<string> roles = new List<string>();
            roles.Add("All");
            roles.AddRange(userService.GetRoles());
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (string e in roles)
            {
                list.Add(new SelectListItem
                {
                    Text = e,
                    Value = e
                });
            }
            return list;
        }

        [HttpPost]
        public ActionResult UserSearch(string username)
        {
            ViewBag.Query = username;

            IEnumerable<UserDTO> userDtos;
            try
            {
                userDtos = userService.SearchAllByUsername(username);
            }
            catch (ValidationException ex)
            {
                return Content(ex.Message);
            }

            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserViewModel>()).CreateMapper();
            var userViews = mapper.Map<IEnumerable<UserDTO>, List<UserViewModel>>(userDtos);
            
            return PartialView(userViews);
        }

        [Authorize]
        [HttpGet]
        public ActionResult Edit(string id)
        {
            UserDTO userDto;
            try
            {
                userDto = userService.GetById(id);
            }
            catch (ValidationException ex)
            {
                return Content(ex.Message);
            }
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserViewModel>()).CreateMapper();
            UserViewModel userView = mapper.Map<UserDTO, UserViewModel>(userDto);

            ViewBag.SelRoleName = new SelectList(userService.GetRoles(), userDto.Role.ToString());

            return View(userView);
        }

        [HttpPost]
        public ActionResult Edit(UserViewModel userView, string selRoleName)
        {
            if (ModelState.IsValid)
            {
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserViewModel, UserDTO>()).CreateMapper();
                UserDTO userDto = mapper.Map<UserViewModel, UserDTO>(userView);
                userDto.Role = selRoleName;
                try
                {
                    userService.Edit(userDto);
                }
                catch (ValidationException)
                {
                    return HttpNotFound();
                }

                return RedirectToAction("Index");
            }
            else return Content("The form was filled out with errors");
        }

        [Authorize]
        public ActionResult Delete(string id)
        {
            UserDTO userDto;
            try
            {
                userDto = userService.GetById(id);
            }
            catch (ValidationException ex)
            {
                return Content(ex.Message);
            }
            if (User.Identity.GetUserId() == userDto.Id || User.IsInRole("admin"))
            {
                ViewBag.Id = id;
                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserViewModel>()).CreateMapper();
                UserViewModel userView = mapper.Map<UserDTO, UserViewModel>(userDto);
                return View(userView);
            }
            else return Content("You don`t have anought permissions!");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            try
            {
                userService.Delete(id);
            }
            catch (ValidationException ex)
            {
                return Content(ex.Message);
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            userService.Dispose();
            base.Dispose(disposing);
        }

        [HttpGet]
        public JsonResult CheckUsername(string username)
        {
            var result = !userService.SearchAllByUsername(username).Any();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}