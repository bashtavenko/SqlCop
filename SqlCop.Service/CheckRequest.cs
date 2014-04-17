using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;

namespace SqlCop.Service
{
  [Route("/check")]
  [Route("/check/{sql}")]
  public class CheckRequest
  {
    public string Sql { get; set; }
    public IList<SqlRuleRequest> Rules { get; set; }
  }
}