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
  [SqlRule(Constants.Namespace, Constants.TopRuleId, Constants.TopRuleName, Constants.BaseRulesCategory)]
  public class TopRule : SqlRule
  {
    public override IList<SqlRuleProblem> Analyze(SqlRuleContext context)
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
