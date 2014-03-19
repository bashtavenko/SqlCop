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
  public class TopRule : SqlRule
  {
    public override IList<SqlRuleProblem> Analyze(SqlRuleContext context)
    {
      List<SqlRuleProblem> problems = new List<SqlRuleProblem>();
      TSqlFragment sqlFragment = context.ScriptFragment as TSqlFragment;      
      Debug.Assert(sqlFragment != null, "TSqlFragment is expected");

      TopRowFilterVisitor visitor = new TopRowFilterVisitor();
      sqlFragment.Accept(visitor);
      if (!visitor.HasParenthesis)
      {              
        problems.Add(new SqlRuleProblem(this, Resources.TOP_parenthesis_rule, visitor.SqlFragment));
      }

      return problems;
    }
  }
}
