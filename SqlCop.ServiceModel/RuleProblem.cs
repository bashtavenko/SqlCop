using SqlCop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlCop.ServiceModel
{
  public class RuleProblem
  {
    public string Description { get; set;}
    public string ErrorMessageString { get; set; }    
    public RuleProblemSeverity Severity { get; set; }
    public int StartColumn { get; set; }
    public int StartLine { get; set;}  
  }
}
