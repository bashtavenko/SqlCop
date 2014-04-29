using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SqlCop.DemoClient.ViewModels
{
  public class Rule
  {
    public bool Selected { get; set; }
    public string Namespace { get; set; }
    public string Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
  }
}