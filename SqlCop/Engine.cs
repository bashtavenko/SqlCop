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
    
    public IList<SqlRuleProblem> Run(string sqlFragment, SqlRule rule)
    {
      using (TextReader textReader = new StringReader(sqlFragment))
      {
        return Run(textReader, rule);
      }
    }
    
    public IList<SqlRuleProblem> Run(TextReader input, SqlRule rule)
    {
      IList<SqlRuleProblem> problems;

      var parser = new TSql100Parser(true);
      var parseErrors = new List<ParseError>() as IList<ParseError>;
      IScriptFragment scriptFragment = parser.Parse(input, out parseErrors);

      if (parseErrors.Count > 0)
      {
        // TODO: do custom exception
        var error = parseErrors[0];
        var ex = new ArgumentException (error.Message);
        throw ex;
      }

      var context = new SqlRuleContext
      {
        ScriptFragment = scriptFragment,
        //Script = scriptFragment as TSqlScript
      };
      
      problems = rule.Analyze(context);
      return problems;
    }

    public IList<SqlRuleProblem> Run(TextReader input)
    {
      /// todo: reflect over assembly and find all the rules
      return Run(input, new TopRule());
    }
  }
}
