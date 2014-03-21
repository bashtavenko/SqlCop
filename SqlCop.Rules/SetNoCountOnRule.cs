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
  [SqlRule(Constants.Namespace, Constants.SetNoCountOnRuleId, Constants.SetNoCountOnRuleName, Constants.BaseRulesCategory)]
  public class SetNoCountOnRule : SqlRule
  {    
    public override IList<SqlRuleProblem> Analyze(SqlRuleContext context)
    {
      TSqlScript script = context.ScriptFragment as TSqlScript;
      Debug.Assert(script != null, "TSqlScript is expected");

      Visitor = new StoredProcVisitor();
      script.Accept(Visitor);
      if (Visitor.WasVisited && !(Visitor as StoredProcVisitor).HasNocountOn)
      {
        AddProblem(Resources.SetNoCountOnRule);        
      }

      return Problems;
    }
  }
}
