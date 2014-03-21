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
      protected SqlCopVisitor Visitor { get; set; }
      protected List<SqlRuleProblem> Problems {get; set;}

      public SqlRule()
      {
        Problems = new List<SqlRuleProblem>();
      }

      public abstract IList<SqlRuleProblem> Analyze(SqlRuleContext context);

      protected void AddProblem(string problemDescription)
      {
        Problems.Add(new SqlRuleProblem(this, problemDescription, Visitor.SqlFragment));
      }
    }
}
