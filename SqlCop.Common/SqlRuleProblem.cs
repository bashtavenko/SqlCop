using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Schema.SchemaModel;
using Microsoft.Data.Schema.ScriptDom.Sql;

namespace SqlCop.Common
{
  public class SqlRuleProblem
  {
    private readonly SqlRule _rule;
    private readonly string _description;
    private readonly TSqlFragment _sqlFragment;


    public SqlRuleProblem(SqlRule rule, string description)
      : this(rule, description, null)
    {
    }
    
    public SqlRuleProblem(SqlRule rule, string description, TSqlFragment sqlFragment)
    {
      _rule = rule;
      _description = description;
      _sqlFragment = sqlFragment;      
    }       
        
    public string Description { get { return _description;} }        
    public string ErrorMessageString { get; set; }    
    public string FileName { get; set; }
    public TSqlFragment SqlFragment { get { return _sqlFragment; } }
    public SqlRule Rule { get { return _rule; } }    
    public SqlRuleProblemSeverity Severity { get; set; }
    public int StartColumn { get { return _sqlFragment.StartColumn; } }
    public int StartLine { get { return _sqlFragment.StartLine; } }    
  }
}
