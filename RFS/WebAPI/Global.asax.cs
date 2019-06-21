using Ninject;
using Ninject.Web.Common.WebHost;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using RoomManagement;
using RoomManagement.Entities;
using WebAPI.Models.Entities;

namespace WebAPI
{
    public class WebApiApplication : NinjectHttpApplication
    {
        protected override void OnApplicationStarted()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
          
            base.OnApplicationStarted();
        }
        protected void Application_Start()
        {

        }

        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            RegisterServices(kernel);
            return kernel;
        }
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IUserManager>().To<UserManager>();
            kernel.Bind<IRoomManager>().To<RoomManager>();
            kernel.Bind<ILocationManager>().To<LocationManager>();
            kernel.Bind<IBookingManager>().To<BookingManager>();
            kernel.Bind<IRoomManagementDatabasseEntities>().To<rfsEntities>();
        }
    }
    
}
