using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SqlCop.DemoClient.ViewModels
{
  public class Home
  {
    public IList<Rule> AllRules { get; set; }
    public string Sql { get; set; }
    public IList<SqlCop.ServiceModel.RuleProblem> Problems { get; set; }
    public string ErrorDescription { get; set; }
    public List<string> SelectedRules { get; set; }

    public Home()
    {
      AllRules = new List<Rule>();
      Problems = new List<SqlCop.ServiceModel.RuleProblem>();      
    }
  }
}