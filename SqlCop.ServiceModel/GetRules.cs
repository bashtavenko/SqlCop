using ServiceStack;
using SqlCop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlCop.ServiceModel
{
  [Route("/rules", "GET")]
  [Route("/rules/{Id}", "GET")]
  public class GetRules : IReturn<List<RuleModel>>
  {
    public string Id { get; set; }
  }
}
