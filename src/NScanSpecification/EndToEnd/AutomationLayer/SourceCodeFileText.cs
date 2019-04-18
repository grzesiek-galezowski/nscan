using System;
using System.Linq;
using TddXt.NScan.ReadingSolution.Ports;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  public static class SourceCodeFileText
  {
    public static string GenerateFrom(XmlSourceCodeFile sourceCodeFile)
    {
      return Usings(sourceCodeFile) + $"namespace {sourceCodeFile.DeclaredNamespaces.Single()}" + " {}";

    }

    private static string Usings(XmlSourceCodeFile sourceCodeFile)
    {
      return string.Join(
        Environment.NewLine, 
        sourceCodeFile.Usings.Select(n => $"using {n};"));
    }

  }

}