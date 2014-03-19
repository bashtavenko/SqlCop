using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Data.Schema.ScriptDom;
using Microsoft.Data.Schema.ScriptDom.Sql;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlCop.Rules;

namespace SqlCop.UnitTests
{
  [TestClass]
  public class TsqlParserTest : FileBasedTest
  {
    [TestMethod]
    public void SmokeTest()
    {
      TSqlScript script = Parse("TsqlSample1.sql");
           

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
    
    [TestMethod]
    public void SmokeTestWithVisitor()
    {
      var visitor = new TopRowFilterVisitor();
      TSqlScript script = Parse("TsqlSample1.sql");
      script.Accept(visitor);
      Assert.IsTrue(visitor.HasParenthesis);

      visitor = new TopRowFilterVisitor();
      script = Parse("TsqlSample2.sql");
      script.Accept(visitor);
      Assert.IsFalse(visitor.HasParenthesis);
    }

    [TestMethod]
    public void SprocVisitor()
    {
      var visitor = new StoredProcVisitor();
      TSqlScript script = Parse("StoredProc.sql");
      script.Accept(visitor);
      Assert.IsTrue(visitor.HasNocountOn);
      Assert.IsTrue(visitor.HasTransactionIsolationLevel);
      Assert.IsTrue(visitor.HasComments);
    }

    private TSqlScript Parse(string fileName)
    {
      var parser = new TSql100Parser(true);
      var parseErrors = new List<ParseError>() as IList<ParseError>;
      TSqlScript script;
      using (var sr = new StreamReader(GetFilePath(fileName)))
      {
        script = parser.Parse(sr, out parseErrors) as TSqlScript;
      }
      return script;
    }
  }
}
