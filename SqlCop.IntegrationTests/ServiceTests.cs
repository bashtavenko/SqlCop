using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceStack;
using SqlCop.ServiceModel;
using System.Collections.Generic;
using SqlCop.Common;


namespace SqlCop.IntegrationTests
{
  [TestClass]
  public class ServiceTests
  {
    [TestMethod]
    public void CallServicePost()
    {
      var client = new JsonServiceClient("http://localhost:42020/check");
      var response = client.Post<List<SqlCop.ServiceModel.RuleProblem>>(new CheckRules { Sql = "SELECT TOP(1) FROM pubs;" });
      Assert.AreEqual(0, response.Count);
    }

    [TestMethod]
    public void CallServiceGet()
    {
      var client = new JsonServiceClient("http://localhost:42020/rules");
      var response = client.Get<List<RuleModel>>(new GetRules { });
      Assert.IsTrue(response.Any());
    }
  }
}
