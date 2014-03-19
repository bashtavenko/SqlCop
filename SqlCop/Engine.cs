using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Schema.SchemaModel;
using Microsoft.Data.Schema.ScriptDom;
using Microsoft.Data.Schema.ScriptDom.Sql;
using Microsoft.Data.Schema.StaticCodeAnalysis;
using SqlCop.Common;
using SqlCop.Rules;

namespace SqlCop
{
  public class Engine
  {
    public IList<SqlRuleProblem> Run (TextReader input)
    {
      return Run(input, new TopRule());
    }

    public IList<SqlRuleProblem> Run(TextReader input, SqlRule rule)
    {
      IList<SqlRuleProblem> problems;

      var parser = new TSql100Parser(true);
      var parseErrors = new List<ParseError>() as IList<ParseError>;
      IScriptFragment script = parser.Parse(input, out parseErrors);

      var context = new SqlRuleContext
      {
        ScriptFragment = script as TSqlFragment,
      };
      
      problems = rule.Analyze(context);
      return problems;
    }
  }
}
