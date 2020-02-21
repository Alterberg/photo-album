using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using OnlinePhotoAlbum.DAL.Interfaces;
using OnlinePhotoAlbum.DAL.Repositories;

namespace OnlinePhotoAlbum.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private string connectionString;
        public ServiceModule(string connection)
        {
            connectionString = connection;
        }
        public override void Load()
        {
            Bind<IUnitOfWork>().To<EFUnitOfWork>().WithConstructorArgument(connectionString);
            Bind<IClientManager>().To<ClientManager>().WithConstructorArgument(connectionString);
        }
    }
}
