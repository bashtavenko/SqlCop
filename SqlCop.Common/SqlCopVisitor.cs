using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Schema.ScriptDom.Sql;

namespace SqlCop.Common
{
  public class SqlCopVisitor : TSqlConcreteFragmentVisitor
  {
    public TSqlFragment SqlFragment { get; protected set; }
    public bool WasVisited { get; protected set; }
  }
}
