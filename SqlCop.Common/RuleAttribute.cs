using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlCop.Common
{
  [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
  public class RuleAttribute : Attribute
  {
    private readonly string _namespace;
    private readonly string _id;
    private readonly string _name;
    private readonly string _category;    

    public RuleAttribute (string @namespace, string id, string name, string category)
    {
      _namespace = @namespace;
      _id = id;
      _name = name;
      _category = category;      
    }

    public string DescriptionResourceId { get; set; }
  }
}
