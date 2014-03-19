using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Schema.ScriptDom.Sql;
using SqlCop.Common;

namespace SqlCop.Rules
{
  public class SetNoCountOnRule : SqlRule
  {
    public override IList<SqlRuleProblem> Analyze(SqlRuleContext context)
    {
      List<SqlRuleProblem> problems = new List<SqlRuleProblem>();

      TSqlScript script = context.ScriptFragment as TSqlScript;
      Debug.Assert(script != null, "TSqlScript is expected");

      var visitor = new StoredProcVisitor();
      script.Accept(visitor);
      if (visitor.WasVisited && !visitor.HasNocountOn)
      {
        problems.Add(new SqlRuleProblem(this, Resources.SetNoCountOnRule, visitor.SqlFragment));
      }

      return problems;
    }
  }
}
