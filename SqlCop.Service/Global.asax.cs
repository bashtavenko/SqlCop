using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using ServiceStack;
using SqlCop.ServiceInterface;
using Funq;

namespace SqlCop.Service
{
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

  public class Global : System.Web.HttpApplication
  {
    protected void Application_Start(object sender, EventArgs e)
    {
      new AppHost().Init();
    }
  }
}