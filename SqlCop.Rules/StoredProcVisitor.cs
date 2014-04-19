using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Data.Schema.ScriptDom.Sql;
using SqlCop.Common;


namespace SqlCop.Rules
{
  [Rule(Constants.Namespace, Constants.TopRuleId, Constants.TopRuleName, Constants.BaseRulesCategory)]
  public class StoredProcVisitor : Visitor
  {   
    public bool HasTransactionIsolationLevel { get; private set; }
    public bool HasNocountOn { get; private set; }
    public bool HasComments { get; private set; }    
    private int _approximateBodyStartTokenIndex;

    public override void ExplicitVisit(CreateProcedureStatement node)
    {
      VisitHelper(node);
    }

    public override void ExplicitVisit(AlterProcedureStatement node)
    {
      VisitHelper(node);
    }

    private void VisitHelper(ProcedureStatementBody node)
    {
      WasVisited = true;
      SqlFragment = node;
      TSqlStatement statement;
      if (node.StatementList.Statements.Any())
      {
        var predicate = node.StatementList.Statements[0] as PredicateSetStatement;
        if (predicate != null)
        {
          HasNocountOn = predicate.Options == SetOptions.NoCount && predicate.IsOn;
          _approximateBodyStartTokenIndex = predicate.LastTokenIndex;
        }
      }

      statement = node.StatementList.Statements.FirstOrDefault(s => (s as SetTransactionIsolationLevelStatement) != null);
      if (statement != null)
      {
        var tran = statement as SetTransactionIsolationLevelStatement;
        HasTransactionIsolationLevel = tran.Level == IsolationLevel.ReadUncommitted;
      }

      for (int i = _approximateBodyStartTokenIndex; i < node.LastTokenIndex; i++)
      {
        TSqlParserToken token = node.ScriptTokenStream[i];
        if (token.TokenType == TSqlTokenType.SingleLineComment)
        {
          HasComments = true;
          break;
        }
      }      
    }
  }
}
