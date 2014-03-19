using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Schema.StaticCodeAnalysis;

namespace SqlCop.Common
{
    public abstract class SqlRule
    {      
      public abstract IList<SqlRuleProblem> Analyze(SqlRuleContext context);
    }
}
