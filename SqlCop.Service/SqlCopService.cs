using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;

namespace SqlCop.Service
{
  public class SqlCopService : ServiceStack.Service
  {
    public object Any(SqlRuleRequest request)
    {
      return new SqlRuleProblemResponse();
    }
  }
}