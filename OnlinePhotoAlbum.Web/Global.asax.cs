using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;
using OnlinePhotoAlbum.BLL.Infrastructure;
using OnlinePhotoAlbum.Web.App_Start;
using OnlinePhotoAlbum.Web.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OnlinePhotoAlbum.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Dependency injection
            NinjectModule userModule = new UserModule();
            NinjectModule photoModule = new PhotoModule();
            NinjectModule serviceModule = new ServiceModule("AlbumConnection");
            var kernel = new StandardKernel(userModule, photoModule, serviceModule);
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}
