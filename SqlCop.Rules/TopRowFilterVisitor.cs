using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Schema.ScriptDom.Sql;
using SqlCop.Common;

namespace SqlCop.Rules
{
    public class TopRowFilterVisitor : SqlCopVisitor
    {
      private int _leftParethesisCount;
      private int _rightParethesisCount;      

      public bool HasParenthesis { get { return _leftParethesisCount == 1 && _rightParethesisCount == 1; } }

      public override void ExplicitVisit(TopRowFilter node)
      {
        WasVisited = true;
        SqlFragment = node;
        for (int i = node.FirstTokenIndex; i <= node.LastTokenIndex; i++)
        {
          TSqlParserToken token = node.ScriptTokenStream[i];
          if (token.TokenType == TSqlTokenType.LeftParenthesis) _leftParethesisCount++;
          if (token.TokenType == TSqlTokenType.RightParenthesis) _rightParethesisCount++;
        }        
      }
    }
}
