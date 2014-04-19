using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Data.Schema.ScriptDom;
using Microsoft.Data.Schema.ScriptDom.Sql;
using SqlCop.Common;

namespace SqlCop
{
  public class Engine
  {
    public IList<RuleProblem> RunRules(string sqlFragment, IList<RuleModel> ruleRequestList)
    {
      using (TextReader textReader = new StringReader(sqlFragment))
      {
        return RunRules(textReader, null);
      }
    }

    public IList<RuleProblem> RunRules(TextReader input, IList<RuleModel> ruleRequestList)
    {      
      List<Rule> rules = new List<Rule>();

      IList<RuleModel> ruleList = GetRules();
      if (ruleRequestList != null)
      {
        ruleList = ruleList.Join(ruleRequestList,
          s => new { @Namespace = s.Namespace, Id = s.Id },
          t => new { @Namespace = t.Namespace, Id = t.Id },
          (s, t) => s).ToList();
      }
      foreach (var ruleModel in ruleList)
      {
        Rule rule = Activator.CreateInstance(ruleModel.RuleType) as Rule;
        if (rule != null)
        {
          rules.Add(rule);          
        }
      }
      IList<RuleProblem> problems = Run(input, rules);
      return problems;
    }           
        
    public IList<RuleModel> GetRules()
    {
      var rules = new List<RuleModel>();

      Assembly asm = Assembly.Load("SqlCop.Rules");
      Type[] types = asm.GetTypes();

      IEnumerable<Type> sqlRuleTypes = types.Where(s => s.BaseType == typeof(Rule));
      foreach(Type type in sqlRuleTypes)
      {
        IEnumerable<CustomAttributeData> attributes = type.CustomAttributes.Where(a => a.AttributeType == typeof(RuleAttribute));
        foreach (var ruleAttribute in attributes)
        {
          IList<CustomAttributeTypedArgument> args = ruleAttribute.ConstructorArguments;
          if (args.Count == 4)
          {
            var rule = new RuleModel()
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

    private IList<RuleProblem> RunOne(TextReader input, Rule rule)
    {
      return Run(input, new List<Rule> { rule });
    }

    private IList<RuleProblem> RunOne(string sqlFragment, Rule rule)
    {
      using (TextReader textReader = new StringReader(sqlFragment))
      {
        return RunOne(textReader, rule);
      }
    }

    // Main run method
    private IList<RuleProblem> Run(TextReader input, IList<Rule> rules)
    {
      List<RuleProblem> problems = new List<RuleProblem>();

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
      var context = new RuleContext
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
  }
}
