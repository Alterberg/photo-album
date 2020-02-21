using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using OnlinePhotoAlbum.BLL.Services;
using Microsoft.AspNet.Identity;
using OnlinePhotoAlbum.BLL.Interfaces;
using Ninject;
using Ninject.Extensions.Factory;

[assembly: OwinStartup(typeof(UserStore.App_Start.Startup))]

namespace UserStore.App_Start
{
    public class Startup
    {
        private readonly IServiceCreator serviceCreator; 

        public Startup()
        {
            IKernel ninjectKernel = new StandardKernel();
            ninjectKernel.Bind<IUserService>().To<UserService>();
            ninjectKernel.Bind<IServiceCreator>().ToFactory();
            serviceCreator = ninjectKernel.Get<IServiceCreator>();
        }
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => serviceCreator.CreateUserService("AlbumConnection"));
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
        }
    }
}