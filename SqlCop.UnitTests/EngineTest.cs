using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlCop.Common;

namespace SqlCop.UnitTests
{
  [TestClass]
  public class EngineTest : FileBasedTest
  {
    [TestMethod]
    public void TestRun()
    {
      var engine = new Engine();
      IList<SqlRuleProblem> problems;

      using (var sr = new StreamReader(GetFilePath("TsqlSample1.sql")))
      {
        problems = engine.Run(sr);
      }

      using (var sr = new StreamReader(GetFilePath("TsqlSample2.sql")))
      {
        problems = engine.Run(sr);
      }
    }
  }
}
