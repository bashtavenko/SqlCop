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

namespace SqlCop.Rules
{
  [DataRuleAttribute(
    Constants.NameSpace,
    Constants.TopRuleId,
    Constants.ResourceBaseName,
    Constants.TopRule_RuleName,
    Constants.CategorySamples,
    DescriptionResourceId = Constants.TopRule_ProblemDescription)]
  public class TopRule : StaticCodeAnalysisRule
  {
    public override IList<DataRuleProblem> Analyze(DataRuleSetting ruleSetting, DataRuleExecutionContext context)
    {
      List<DataRuleProblem> problems = new List<DataRuleProblem>();

      IModelElement modelElement = context.ModelElement;

      // casting to SQL specific 
      SqlSchemaModel sqlSchemaModel = modelElement.Model as SqlSchemaModel;
      Debug.Assert(sqlSchemaModel != null, "SqlSchemaModel is expected");

      ISqlModelElement sqlElement = modelElement as ISqlModelElement;
      Debug.Assert(sqlElement != null, "ISqlModelElement is expected");

      // Get ScriptDom for this model element
      TSqlFragment sqlFragment = context.ScriptFragment as TSqlFragment;
      Debug.Assert(sqlFragment != null, "TSqlFragment is expected");

      TopRowFilterVisitor visitor = new TopRowFilterVisitor();
      sqlFragment.Accept(visitor);
      if (!visitor.HasParenthesis)
      {
        problems.Add(new DataRuleProblem(this, string.Format(this.RuleProperties.Description, SqlRuleUtils.GetElementName(sqlSchemaModel, sqlElement)), sqlElement));
      }

      return problems;
    }
  }
}
