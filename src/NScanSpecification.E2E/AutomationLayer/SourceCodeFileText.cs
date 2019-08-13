using System;
using System.Linq;
using FluentAssertions;
using TddXt.NScan.ReadingCSharpSourceCode;
using TddXt.NScan.ReadingSolution.Ports;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  public static class SourceCodeFileText
  {
    public static string GenerateFrom(XmlSourceCodeFile sourceCodeFile)
    {
      return Usings(sourceCodeFile) + $"namespace {sourceCodeFile.DeclaredNamespaces.Single()}" + " {" +
             Classes(sourceCodeFile) + "}";
    }

    private static string Classes(XmlSourceCodeFile sourceCodeFile)
    {
      return string.Join(
        Environment.NewLine, 
        sourceCodeFile.Classes.Select(
          classDeclarationInfo => GenerateClass(classDeclarationInfo, 
            methodDeclarationInfo => GenerateMethod(methodDeclarationInfo,
              s => GenerateAttribute(s)))));
    }

    private static string GenerateClass(ClassDeclarationInfo c, Func<MethodDeclarationInfo, string> generateMethod)
    {
      return "public class " + c.Name + "{"+ Interior(c, generateMethod) +"}";
    }

    private static string Interior(ClassDeclarationInfo classDeclaration, Func<MethodDeclarationInfo, string> generateMethod)
    {
      return string.Join(Environment.NewLine, classDeclaration.Methods.Select(generateMethod));
    }

    private static string GenerateMethod(MethodDeclarationInfo d, Func<string, string> generateAttribute)
    {
      return string.Join("", d.Attributes.Select(generateAttribute)) + "public void " + d.Name + "() {}";
    }

    private static string GenerateAttribute(string a)
    {
      return "[" + a + "]";
    }

    private static string Usings(XmlSourceCodeFile sourceCodeFile)
    {
      return string.Join(
        Environment.NewLine,
        sourceCodeFile.Usings.Select(n => $"using {n};"));
    }

  }

}