using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlCop.Common;
using SqlCop.Rules;

namespace SqlCop.UnitTests
{
  [TestClass]
  public class EngineTest : FileBasedTest
  {
    private Engine _engine;
    [TestInitialize]
    public void Init()
    {
      _engine = new Engine();
    }

    [TestMethod]
    public void TestRun()
    {      
      IList<SqlRuleProblem> problems;

      using (var sr = new StreamReader(GetFilePath("TsqlSample1.sql")))
      {
        problems = _engine.Run(sr);
        Assert.AreEqual(0, problems.Count);
      }

      using (var sr = new StreamReader(GetFilePath("TsqlSample2.sql")))
      {
        problems = _engine.Run(sr);
        Assert.AreEqual(1, problems.Count);
      }
    }

    [TestMethod]
    public void TopRule()
    {
      var rule = new TopRule();
      IList<SqlRuleProblem> problems;
      string sql;

      sql = "SELECT TOP(10) * FROM abc";
      problems = _engine.Run(sql, rule);
      Assert.AreEqual(0, problems.Count);

      sql = "SELECT TOP 10 * FROM abc";
      problems = _engine.Run(sql, rule);
      Assert.AreEqual(1, problems.Count);
    }

    [TestMethod]
    public void SetNoCountOnRule()
    {
      var rule = new SetNoCountOnRule();
      IList<SqlRuleProblem> problems;
      using (var sr = new StreamReader(GetFilePath("StoredProc.sql")))
      {
        problems = _engine.Run(sr, rule);
        Assert.AreEqual(0, problems.Count);
      }
    }

    [TestMethod]
    public void SetNoCountOnRule_With_Not_ApplicableCode()
    {
      var rule = new SetNoCountOnRule();
      IList<SqlRuleProblem> problems;
      using (var sr = new StreamReader(GetFilePath("TsqlSample1.sql")))
      {
        problems = _engine.Run(sr, rule);
        Assert.AreEqual(0, problems.Count);
      }
    }

    [TestMethod]
    public void SetNoCountOnRule2()
    {
      var rule = new SetNoCountOnRule();
      IList<SqlRuleProblem> problems;
      string sql;
      sql = @"CREATE PROCEDURE dbo._Stored_Procedure_Template ( @Parameter_1 INT ) AS RETURN;";
      problems = _engine.Run(sql, rule);
      Assert.AreEqual(1, problems.Count);      
    }
  }
}
