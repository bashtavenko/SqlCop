using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlCop.UnitTests
{
  public class FileBasedTest
  {
    protected string GetFilePath(string fileName)
    {
      return Path.Combine("..\\..\\TestFiles", fileName).ToString();
    }
  }
}
