using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Schema.StaticCodeAnalysis;

namespace SqlCop.Common
{
    public abstract class Rule
    {
      protected Visitor Visitor { get; set; }
      protected List<RuleProblem> Problems {get; set;}

      public Rule()
      {
        Problems = new List<RuleProblem>();
      }

      public abstract IList<RuleProblem> Analyze(RuleContext context);

      protected void AddProblem(string problemDescription)
      {
        Problems.Add(new RuleProblem(this, problemDescription, Visitor.SqlFragment));
      }
    }
}
