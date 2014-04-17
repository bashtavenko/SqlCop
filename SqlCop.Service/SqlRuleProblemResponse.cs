using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SqlCop.Service
{
  public class SqlRuleProblemResponse
  {
    public string Description { get; set; }
    public string ErrorMessageString { get; set; }
    public int StartColumn { get; set; }
    public int StartLine { get; set; }    
  }
}