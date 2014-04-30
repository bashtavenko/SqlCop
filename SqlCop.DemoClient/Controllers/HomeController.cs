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
        var model = BuildModel();
        return View(model);
      }

      [HttpPost]
      public ActionResult Index(Home model)
      {
        var homeModel = BuildModel();
        homeModel.Sql = model.Sql;   
        
        var dto = new CheckRules { Sql = model.Sql, Rules = new List<RuleModel>()};

        if (model.SelectedRules != null)
        {
          foreach (string ruleId in model.SelectedRules)
          {
            dto.Rules.Add(new RuleModel { Id = ruleId, Namespace = "SqlCop.Rules" });
            var item = homeModel.AllRules.SingleOrDefault(s => s.Id == ruleId);
            //if (item != null) item.Selected = true;
          }
        }
                                 
        List<SqlCop.ServiceModel.RuleProblem> response = null;
        try
        {
           response = _client.Post(dto);
        }
        catch (WebServiceException ex)
        {
          homeModel.ErrorDescription = ex.ErrorMessage;
        }
                
        if (response !=null && response.Any())
        {
          homeModel.Problems = response;
        }

        return View(homeModel);
      }

      private Home BuildModel()
      {       
        IList<RuleModel> rules = _client.Get(new GetRules { });
        var modelRules = Mapper.Map<IList<RuleModel>, IList<SqlCop.DemoClient.ViewModels.Rule>>(rules);
       
        var model = new Home { AllRules = modelRules };
        return model;      
      }
    }
}
