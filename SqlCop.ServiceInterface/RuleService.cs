using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;
using SqlCop.ServiceModel;
using SqlCop.Common;

namespace SqlCop.ServiceInterface
{
  public class RuleService : IService
  {
    public List<RuleModel> Get(GetRules request)
    {
      var engine = new Engine();      
      var list = engine.GetRules();
      return list.ToList();      
    }
  }
}