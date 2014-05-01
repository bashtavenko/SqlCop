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
      private string ApiPath { get { return Request.Url.OriginalString + "/api"; } }

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
            if (item != null) item.Selected = true;
          }
        }

        JsonServiceClient client = null;
        List<SqlCop.ServiceModel.RuleProblem> response = null;
        try
        {
          client = new JsonServiceClient(ApiPath);
          response = client.Post(dto);
        }
        catch (WebServiceException ex)
        {
          homeModel.ErrorDescription = ex.ErrorMessage;
        }
        finally
        {
          if (client !=null) client.Dispose();
        }

        if (response != null && response.Any())
        {
          homeModel.Problems = response;
        }
        else
        {
          homeModel.SuccessMessage = "No rule violations found";
        }
        return View(homeModel);
      }

      private Home BuildModel()
      {
        IList<RuleModel> rules;
        using (JsonServiceClient client = new JsonServiceClient(ApiPath))
        {
          rules = client.Get(new GetRules { });
        }
        var modelRules = Mapper.Map<IList<RuleModel>, IList<SqlCop.DemoClient.ViewModels.Rule>>(rules);
       
        var model = new Home { AllRules = modelRules };
        return model;      
      }
    }
}
