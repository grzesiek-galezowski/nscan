using System;
using System.Linq;
using TddXt.NScan.ReadingCSharpSourceCode;
using TddXt.NScan.ReadingSolution.Ports;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  public static class SourceCodeFileText
  {
    public static string GenerateFrom(XmlSourceCodeFile sourceCodeFile)
    {
      return Usings(sourceCodeFile) + $"namespace {sourceCodeFile.DeclaredNamespaces.Single()}" + " {" +
             Interior(sourceCodeFile) + "}";

    }

    private static string Interior(XmlSourceCodeFile sourceCodeFile)
    {
      return string.Join(
        Environment.NewLine, 
        sourceCodeFile.Classes.Select(c => "public class " + c.Name + "{"+ Interior(c) +"}"));
    }

    private static string Interior(ClassDeclarationInfo classDeclaration)
    {
      return string.Join(Environment.NewLine, classDeclaration.Methods.Select(d => "public void " + d.Name + "() {}"));
    }

    private static string Usings(XmlSourceCodeFile sourceCodeFile)
    {
      return string.Join(
        Environment.NewLine,
        sourceCodeFile.Usings.Select(n => $"using {n};"));
    }

  }

}