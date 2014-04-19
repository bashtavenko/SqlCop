using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Schema.SchemaModel;
using Microsoft.Data.Schema.ScriptDom.Sql;

namespace SqlCop.Common
{
  public class RuleProblem
  {
    private readonly Rule _rule;
    private readonly string _description;
    private readonly TSqlFragment _sqlFragment;


    public RuleProblem(Rule rule, string description)
      : this(rule, description, null)
    {
    }
    
    public RuleProblem(Rule rule, string description, TSqlFragment sqlFragment)
    {
      _rule = rule;
      _description = description;
      _sqlFragment = sqlFragment;      
    }       
        
    public string Description { get { return _description;} }        
    public string ErrorMessageString { get; set; }    
    public string FileName { get; set; }
    public TSqlFragment SqlFragment { get { return _sqlFragment; } }
    public Rule Rule { get { return _rule; } }    
    public RuleProblemSeverity Severity { get; set; }
    public int StartColumn { get { return _sqlFragment.StartColumn; } }
    public int StartLine { get { return _sqlFragment.StartLine; } }    
  }
}
