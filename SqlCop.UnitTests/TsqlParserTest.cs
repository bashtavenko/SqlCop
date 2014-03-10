using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Data.Schema.ScriptDom;
using Microsoft.Data.Schema.ScriptDom.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlCop.UnitTests
{
  [TestClass]
  public class TsqlParserTest
  {
    [TestMethod]
    public void SmokeTest()
    {
      var parser = new TSql100Parser(true);
      var parseErrors = new List<ParseError>() as IList<ParseError>;
      TSqlScript script;
      using (var sr = new StreamReader(GetFilePath("TsqlSample1.sql")))
      {
        script = parser.Parse(sr, out parseErrors) as TSqlScript; 
      }

      foreach (TSqlBatch batch in script.Batches)
      {
        foreach (TSqlStatement statement in batch.Statements)
        {
          var selectStatement = statement as SelectStatement;
          if (selectStatement != null && selectStatement.QueryExpression != null)
          {
            var querySpecification = selectStatement.QueryExpression as QuerySpecification;
            foreach (TableSource tableSource in querySpecification.FromClauses)
            {              
            }
          }
        }
      }
    }

    private string GetFilePath(string fileName)
    {
      return Path.Combine("..\\..\\TestFiles", fileName).ToString();
    }
  }
}
