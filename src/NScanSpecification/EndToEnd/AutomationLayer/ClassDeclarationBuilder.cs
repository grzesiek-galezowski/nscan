using System.Collections.Generic;
using System.Linq;
using TddXt.NScan.ReadingCSharpSourceCode;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  public class ClassDeclarationBuilder
  {
    private readonly string _name;
    private readonly List<MethodDeclarationBuilder> _methods = new List<MethodDeclarationBuilder>();

    private ClassDeclarationBuilder(string name)
    {
      _name = name;
    }

    public static ClassDeclarationBuilder Class(string name)
    {
      return new ClassDeclarationBuilder(name);
    }

    public ClassDeclarationBuilder With(params MethodDeclarationBuilder[] methodDeclarationBuilders)
    {
      _methods.AddRange(methodDeclarationBuilders);
      return this;
    }

    public ClassDeclarationInfo Build()
    {
      var classDeclarationInfo = new ClassDeclarationInfo(_name, "" /* bug */);
      classDeclarationInfo.Methods.AddRange(_methods.Select(m => m.Build()));
      return classDeclarationInfo;
    }
  }
}