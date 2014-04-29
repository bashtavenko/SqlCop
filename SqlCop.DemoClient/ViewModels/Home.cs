using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SqlCop.DemoClient.ViewModels
{
  public class Home
  {
    public IList<Rule> Rules { get; set; }
    public string Sql { get; set; }
    public IList<SqlCop.Common.RuleProblem> Problems { get; set; }

    public Home()
    {
      Rules = new List<Rule>();
      Problems = new List<SqlCop.Common.RuleProblem>();
    }
  }
}