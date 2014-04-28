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
  public class ServiceTests : IDisposable
  {    
    private JsonServiceClient _client;
        
    [TestInitialize]
    public void Init()
    {
      _client = new JsonServiceClient("http://localhost:42020");
    }

    [TestMethod]
    public void CallServicePost()
    {      
      List<SqlCop.ServiceModel.RuleProblem> response = _client.Post(new CheckRules { Sql = "SELECT TOP 1 * FROM pubs;"});      
      Assert.AreEqual(1, response.Count);
    }

    [TestMethod]
    public void CallServiceGet()
    {      
      List<RuleModel> response = _client.Get(new GetRules { });
      Assert.IsTrue(response.Any());
    }

    public void Dispose()
    {
      if (_client != null)
      {
        _client.Dispose();
        _client = null;
      }
    }
  }
}
