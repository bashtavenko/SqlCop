using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Data.Schema.ScriptDom.Sql;
using SqlCop.Common;

namespace SqlCop.Rules
{
    public class StatementVisitor : Visitor
    {
      private List<TSqlTokenType> _tokenTypes = new List<TSqlTokenType>
      {
        TSqlTokenType.As,
        TSqlTokenType.Asc,
        TSqlTokenType.Begin,
        TSqlTokenType.Between,
        TSqlTokenType.By,
        TSqlTokenType.Case,
        TSqlTokenType.Close,
        TSqlTokenType.Create,
        TSqlTokenType.Cursor,
        TSqlTokenType.Database,
        TSqlTokenType.Declare,
        TSqlTokenType.Desc,
        TSqlTokenType.Distinct,
        TSqlTokenType.Drop,
        TSqlTokenType.Else,
        TSqlTokenType.End,
        TSqlTokenType.Exec,
        TSqlTokenType.Exists,
        TSqlTokenType.Fetch,
        TSqlTokenType.For,
        TSqlTokenType.From,
        TSqlTokenType.Function,
        TSqlTokenType.Group,
        TSqlTokenType.Select
        // I got tired of it
      };

      public bool IsUpperCase { get; private set; }

      public override void ExplicitVisit(TSqlScript node)
      { 	 
        WasVisited = true;
        SqlFragment = node;

        IsUpperCase = true;
        for (int i = node.FirstTokenIndex; i <= node.LastTokenIndex; i++)
        {
          TSqlParserToken token = node.ScriptTokenStream[i];
          if (_tokenTypes.Contains(token.TokenType))
          {            
            if (!Regex.IsMatch(token.Text, "[A-Z]"))
            {
              IsUpperCase = false;
              break;
            }
          }          
        }        
      }
    }
}
