using Ninject.Extensions.Factory;
using Ninject.Modules;
using OnlinePhotoAlbum.BLL.Interfaces;
using OnlinePhotoAlbum.BLL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlinePhotoAlbum.Web.Util
{
    public class UserModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IUserService>().To<UserService>();
            Bind<IServiceCreator>().ToFactory();
        }
    }
}