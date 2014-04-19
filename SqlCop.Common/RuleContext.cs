using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Schema.SchemaModel;
using Microsoft.Data.Schema.ScriptDom;
using Microsoft.Data.Schema.ScriptDom.Sql;

namespace SqlCop.Common
{
  public class RuleContext
  {    
    public IScriptFragment ScriptFragment { get; set; }    
  }
}
