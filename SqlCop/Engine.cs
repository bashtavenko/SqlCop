using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

    public IList<SqlRuleProblem> Run(string sqlFragment)
    {
      using (TextReader textReader = new StringReader(sqlFragment))
      {
        return Run(textReader);
      }
    }

    public IList<SqlRuleProblem> Run(TextReader input)
    {
      List<SqlRuleProblem> problems = new List<SqlRuleProblem>();

      var ruleList = GetRules();
      foreach (var ruleModel in ruleList)
      {
        SqlRule rule = Activator.CreateInstance(ruleModel.RuleType) as SqlRule;
        if (rule != null)
        {
          var ruleProblems = Run(input, rule);
          problems.AddRange(ruleProblems);
        }
      }
      return problems;
    }

    public IList<SqlRuleModel> GetRules()
    {
      var rules = new List<SqlRuleModel>();

      Assembly asm = Assembly.Load("SqlCop.Rules");
      Type[] types = asm.GetTypes();

      foreach (Type type in types)
      {
        if (type.BaseType == typeof(SqlRule))
        {           
          foreach (var ruleAttribute in type.CustomAttributes.Where(a => a.AttributeType == typeof(SqlRuleAttribute)))
          {
            var args = ruleAttribute.ConstructorArguments;
            if (args.Count == 4)
            {
              var rule = new SqlRuleModel()
              { 
                Namespace = args[0].Value as string,
                Id = args[1].Value as string,
                Name = args[2].Value as string,
                Category = args[3].Value as string,
                RuleType = type
              };
              rules.Add(rule);
            }
          }
        }
      }

      return rules;
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
        var ex = new ArgumentException(error.Message);
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
  }
}
