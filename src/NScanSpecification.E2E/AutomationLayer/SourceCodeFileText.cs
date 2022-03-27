using System;
using System.Linq;
using NScan.SharedKernel.ReadingCSharpSourceCode;
using NScan.SharedKernel.ReadingSolution.Ports;

namespace NScanSpecification.E2E.AutomationLayer;

public static class SourceCodeFileText
{
  public static string GenerateFrom(SourceCodeFileDto sourceCodeFile)
  {
    return Usings(sourceCodeFile) + $"namespace {sourceCodeFile.DeclaredNamespaces.Single()}" + " {" +
           Classes(sourceCodeFile) + "}";
  }

  private static string Classes(SourceCodeFileDto sourceCodeFile)
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

  private static string Usings(SourceCodeFileDto sourceCodeFile)
  {
    return string.Join(
      Environment.NewLine,
      sourceCodeFile.Usings.Select(n => $"using {n};"));
  }

}