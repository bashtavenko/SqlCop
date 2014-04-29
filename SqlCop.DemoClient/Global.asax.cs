using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Funq;
using ServiceStack;
using SqlCop.ServiceInterface;

namespace SqlCop.DemoClient
{
  // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
  // visit http://go.microsoft.com/?LinkId=9394801


  public class AppHost : AppHostBase
  {
    //Tell Service Stack the name of your application and where to find your web services
    public AppHost() : base("SqlCop Web Services", typeof(RuleProblemService).Assembly) { }

    public override void Configure(Container container)
    {
      //register any dependencies your services use, e.g:
      //container.Register<ICacheClient>(new MemoryCacheClient());
    }
  }


  public class MvcApplication : System.Web.HttpApplication
  {
    protected void Application_Start()
    {
      AreaRegistration.RegisterAllAreas();
            
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);
      AutoMapperConfig.CreateMaps();

      new AppHost().Init();
    }
  }
}