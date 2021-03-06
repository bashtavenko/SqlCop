﻿using System;
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
      IList<RuleProblem> problems;

      using (var sr = new StreamReader(GetFilePath("TsqlSample1.sql")))
      {
        problems = _engine.RunRules(sr, null);
        Assert.AreEqual(0, problems.Count);
      }

      using (var sr = new StreamReader(GetFilePath("TsqlSample2.sql")))
      {
        problems = _engine.RunRules(sr, null);
        Assert.AreEqual(1, problems.Count);
      }
    }

    [TestMethod]
    public void TopRule()
    {
      var rule = new TopRule();
      IList<RuleProblem> problems;
      string sql;

      sql = "SELECT TOP(10) * FROM abc";
      problems = _engine.RunRules(sql, GetRuleRequest(Constants.Namespace, Constants.TopRuleId));
      Assert.AreEqual(0, problems.Count);

      sql = "SELECT TOP 10 * FROM abc";
      problems = _engine.RunRules(sql, GetRuleRequest(Constants.Namespace, Constants.TopRuleId));
      Assert.AreEqual(1, problems.Count);
    }

    [TestMethod]
    public void SetNoCountOnRule()
    {      
      IList<RuleProblem> problems;
      using (var sr = new StreamReader(GetFilePath("StoredProc.sql")))
      {        
        problems = _engine.RunRules(sr, GetRuleRequest(Constants.Namespace, Constants.SetNoCountOnRuleId));
        Assert.AreEqual(0, problems.Count);
      }
    }

    [TestMethod]
    public void SetNoCountOnRule_With_Not_ApplicableCode()
    {
      var rule = new SetNoCountOnRule();
      IList<RuleProblem> problems;
      using (var sr = new StreamReader(GetFilePath("TsqlSample1.sql")))
      {
        problems = _engine.RunRules(sr, GetRuleRequest(Constants.Namespace, Constants.SetNoCountOnRuleId ));
        Assert.AreEqual(0, problems.Count);
      }
    }

    [TestMethod]
    public void SetNoCountOnRule2()
    { 
      IList<RuleProblem> problems;
      string sql;
      sql = @"CREATE PROCEDURE dbo._Stored_Procedure_Template ( @Parameter_1 INT ) AS RETURN;";
      problems = _engine.RunRules(sql, GetRuleRequest(Constants.Namespace, Constants.SetNoCountOnRuleId)) ;
      Assert.AreEqual(1, problems.Count);
    }

    [TestMethod]
    public void GetRules_Reflection_Test()
    {
      var list = _engine.GetRules();
    }

    [TestMethod]
    public void RunAllRules()
    {
      IList<RuleProblem> problems;
      string sql = "SELECT TOP 10 * FROM abc";
      problems = _engine.RunRules(sql, null);
    }

    private IList<RuleModel> GetRuleRequest(string @namespace, string id)
    {
      return new List<RuleModel> {new RuleModel { Namespace = @namespace, Id = @id }};
    }

  }
}
