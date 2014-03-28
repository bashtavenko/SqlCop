using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlCop.Common
{
  public class SqlRuleModel
  {
    public string Namespace { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public Type RuleType { get; set; }
  }
}
