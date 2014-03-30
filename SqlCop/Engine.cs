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
      List<SqlRule> rules = new List<SqlRule>();

      IList<SqlRuleModel> ruleList = GetRules();
      foreach (var ruleModel in ruleList)
      {
        SqlRule rule = Activator.CreateInstance(ruleModel.RuleType) as SqlRule;
        if (rule != null)
        {
          rules.Add(rule);          
        }
      }
      IList<SqlRuleProblem> problems = Run(input, rules);
      return problems;
    }

    public IList<SqlRuleProblem> Run(TextReader input, IList<SqlRule> rules)
    {
        List<SqlRuleProblem> problems = new List<SqlRuleProblem>();

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
        };

        foreach (var rule in rules)
        {
          var rulePoblems = rule.Analyze(context);
          problems.AddRange(rulePoblems);
        }
        return problems;
    }

    public IList<SqlRuleProblem> Run(TextReader input, SqlRule rule)
    {
      return Run(input, new List<SqlRule> { rule });
    }

    public IList<SqlRuleModel> GetRules()
    {
      var rules = new List<SqlRuleModel>();

      Assembly asm = Assembly.Load("SqlCop.Rules");
      Type[] types = asm.GetTypes();

      IEnumerable<Type> sqlRuleTypes = types.Where(s => s.BaseType == typeof(SqlRule));
      foreach(Type type in sqlRuleTypes)
      {
        IEnumerable<CustomAttributeData> attributes = type.CustomAttributes.Where(a => a.AttributeType == typeof(SqlRuleAttribute));
        foreach (var ruleAttribute in attributes)
        {
          IList<CustomAttributeTypedArgument> args = ruleAttribute.ConstructorArguments;
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
      return rules;
    }
  }
}
