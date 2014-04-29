using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using ServiceStack;
using SqlCop.Common;
using SqlCop.DemoClient.ViewModels;
using SqlCop.ServiceModel;

namespace SqlCop.DemoClient.Controllers
{
    public class HomeController : Controller
    {
      private readonly JsonServiceClient _client;
      public HomeController()
      {
        _client = new JsonServiceClient("http://localhost:5000/api");
      }    

      public ActionResult Index()
      {        
        IList<RuleModel> rules = _client.Get(new GetRules { });
        var modelRules = Mapper.Map<IList<RuleModel>, IList<SqlCop.DemoClient.ViewModels.Rule>>(rules);
       
        var model = new Home { AllRules = modelRules };
        return View(model);
      }

      [HttpPost]
      public ActionResult Index(Home model)
      {
        var dto = new CheckRules { Sql = model.Sql };
        List<SqlCop.ServiceModel.RuleProblem> response = _client.Post(dto);

        var homeModel = new Home { Sql = model.Sql };
        if (response.Any())
        {
          homeModel.Problems = response;
        }

        return View(homeModel);
      }
    }
}
