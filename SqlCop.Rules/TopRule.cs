using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Schema.SchemaModel;
using Microsoft.Data.Schema.ScriptDom.Sql;
using Microsoft.Data.Schema.Sql.SchemaModel;
using Microsoft.Data.Schema.StaticCodeAnalysis;
using SqlCop.Common;

namespace SqlCop.Rules
{  
  [Rule(Constants.Namespace, Constants.TopRuleId, Constants.TopRuleName, Constants.BaseRulesCategory)]
  public class TopRule : Rule
  {
    public override IList<RuleProblem> Analyze(RuleContext context)
    {      
      TSqlScript script = context.ScriptFragment as TSqlScript;
      Debug.Assert(script != null, "TSqlScript is expected");

      Visitor = new TopRowFilterVisitor();
      script.Accept(Visitor);
      if (Visitor.WasVisited && !(Visitor as TopRowFilterVisitor).HasParenthesis)
      {              
        AddProblem(Resources.TopRule);
      }

      return Problems;
    }
  }
}
