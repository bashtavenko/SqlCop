using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;
using SqlCop.ServiceModel;
using SqlCop.Common;

namespace SqlCop.ServiceInterface
{
  public class RuleProblemService : IService
  {
    public List<SqlCop.ServiceModel.RuleProblem> Post(CheckRules request)
    {
      var engine = new Engine();
      var list = engine.RunRules(request.Sql, null);
      return list.Select(s => 
        new SqlCop.ServiceModel.RuleProblem
        { 
          Description = s.Description,
          ErrorMessageString = s.ErrorMessageString,
          Severity = s.Severity,
          StartColumn = s.StartColumn,
          StartLine = s.StartLine
        }).ToList();          
    }
  }
}