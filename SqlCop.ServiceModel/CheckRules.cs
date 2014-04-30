using ServiceStack;
using SqlCop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlCop.ServiceModel
{
  [Route("/check", "POST")]
  public class CheckRules : IReturn<List<RuleProblem>>
  {
    public string Sql { get; set; }
    public List<RuleModel> Rules { get; set; }
  }
}
