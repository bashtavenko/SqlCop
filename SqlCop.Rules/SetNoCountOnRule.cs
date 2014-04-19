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
  [Rule(Constants.Namespace, Constants.SetNoCountOnRuleId, Constants.SetNoCountOnRuleName, Constants.BaseRulesCategory)]
  public class SetNoCountOnRule : Rule
  {    
    public override IList<RuleProblem> Analyze(RuleContext context)
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
