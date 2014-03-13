using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Schema.ScriptDom.Sql;

namespace SqlCop.Rules
{
    public class TopRowFilterVisitor : TSqlConcreteFragmentVisitor
    {
      private int _leftParethesisCount;
      private int _rightParethesisCount;

      public bool HasParenthesis { get { return _leftParethesisCount == 1 && _rightParethesisCount == 1; } }

      public override void ExplicitVisit(TopRowFilter node)
      {
        for (int i = node.FirstTokenIndex; i <= node.LastTokenIndex; i++)
        {
          TSqlParserToken token = node.ScriptTokenStream[i];
          if (token.TokenType == TSqlTokenType.LeftParenthesis) _leftParethesisCount++;
          if (token.TokenType == TSqlTokenType.RightParenthesis) _rightParethesisCount++;
        }        
      }
    }
}
