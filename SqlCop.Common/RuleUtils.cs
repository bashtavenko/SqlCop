using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Data.Schema.SchemaModel;
using Microsoft.Data.Schema.Sql.SchemaModel;
using Microsoft.Data.Schema.StaticCodeAnalysis;
using Microsoft.Data.Schema;


namespace SqlCop.Common
{
  /// <summary>
  ///  TBD
  /// </summary>
  public static class RuleUtils
  {
    /// <summary>
    /// Get escaped fully qualified name of a model element 
    /// </summary>
    /// <param name="sm">schema model</param>
    /// <param name="element">model element</param>
    /// <returns>name of the element</returns>
    public static string GetElementName(SqlSchemaModel sm, ISqlModelElement element)
    {
      return sm.DatabaseSchemaProvider.UserInteractionServices.GetElementName(element, ElementNameStyle.EscapedFullyQualifiedName);
    }

    /// <summary>
    /// Read file content from a file.
    /// </summary>
    /// <param name="filePath"> file path </param>
    /// <returns> file content in a string </returns>
    public static string ReadFileContent(string filePath)
    {
      //  Verify that the file exists first.
      if (!File.Exists(filePath))
      {
        Debug.WriteLine(string.Format("Cannot find the file: '{0}'", filePath));
        return string.Empty;
      }

      string content;
      using (StreamReader reader = new StreamReader(filePath))
      {
        content = reader.ReadToEnd();
        reader.Close();
      }
      return content;
    }

    /// <summary>
    /// Get the corresponding script file path from a model element.
    /// </summary>
    /// <param name="element">model element</param>
    /// <param name="fileName">file path of the scripts corresponding to the model element</param>
    /// <returns></returns>
    private static Boolean GetElementSourceFile(IModelElement element, out String fileName)
    {
      fileName = null;

      IScriptSourcedModelElement scriptSourcedElement = element as IScriptSourcedModelElement;
      if (scriptSourcedElement != null)
      {
        ISourceInformation elementSource = scriptSourcedElement.PrimarySource;
        if (elementSource != null)
        {
          fileName = elementSource.SourceName;
        }
      }

      return String.IsNullOrEmpty(fileName) == false;
    }

    /// This method converts offset from ScriptDom to line\column in script files.
    /// A line is defined as a sequence of characters followed by a carriage return ("\r"), 
    /// a line feed ("\n"), or a carriage return immediately followed by a line feed. 
    public static bool ComputeLineColumn(string text, Int32 offset, Int32 length,
                                        out Int32 startLine, out Int32 startColumn, out Int32 endLine, out Int32 endColumn)
    {
      const char LF = '\n';
      const char CR = '\r';

      // Setting the initial value of line and column to 0 since VS auto-increments by 1.
      startLine = 0;
      startColumn = 0;
      endLine = 0;
      endColumn = 0;

      int textLength = text.Length;

      if (offset < 0 || length < 0 || offset + length > textLength)
      {
        return false;
      }

      for (int charIndex = 0; charIndex < length + offset; ++charIndex)
      {
        char currentChar = text[charIndex];
        Boolean afterOffset = charIndex >= offset;
        if (currentChar == LF)
        {
          ++endLine;
          endColumn = 0;
          if (afterOffset == false)
          {
            ++startLine;
            startColumn = 0;
          }
        }
        else if (currentChar == CR)
        {
          // CR/LF combination, consuming LF.
          if ((charIndex + 1 < textLength) && (text[charIndex + 1] == LF))
          {
            ++charIndex;
          }

          ++endLine;
          endColumn = 0;
          if (afterOffset == false)
          {
            ++startLine;
            startColumn = 0;
          }
        }
        else
        {
          ++endColumn;
          if (afterOffset == false)
          {
            ++startColumn;
          }
        }
      }

      return true;
    }

    /// <summary>
    /// Compute the start Line/Col and the end Line/Col to update problem info
    /// </summary>
    /// <param name="problem">problem found</param>
    /// <param name="offset">offset of the fragment having problem</param>
    /// <param name="length">length of the fragment having problem</param>
    public static void UpdateProblemPosition(DataRuleProblem problem, int offset, int length)
    {
      if (problem.ModelElement != null)
      {
        String fileName = null;
        int startLine = 0;
        int startColumn = 0;
        int endLine = 0;
        int endColumn = 0;

        bool ret = GetElementSourceFile(problem.ModelElement, out fileName);
        if (ret)
        {
          string fullScript = ReadFileContent(fileName);

          if (fullScript != null)
          {
            if (ComputeLineColumn(fullScript, offset, length, out startLine, out startColumn, out endLine, out endColumn))
            {
              problem.FileName = fileName;
              problem.StartLine = startLine + 1;
              problem.StartColumn = startColumn + 1;
              problem.EndLine = endLine + 1;
              problem.EndColumn = endColumn + 1;
            }
            else
            {
              Debug.WriteLine("Could not compute line and column");
            }
          }
        }
      }
    }
  }
}
