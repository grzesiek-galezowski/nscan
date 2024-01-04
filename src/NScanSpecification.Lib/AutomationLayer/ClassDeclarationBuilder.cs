using LanguageExt;
using NScan.SharedKernel.ReadingCSharpSourceCode;

namespace NScanSpecification.Lib.AutomationLayer;

public class ClassDeclarationBuilder
{
  private readonly string _name;
  private readonly string _namespaceName;
  private readonly Seq<MethodDeclarationBuilder> _methods;

  private ClassDeclarationBuilder(string name, Seq<MethodDeclarationBuilder> methods, string namespaceName)
  {
    _name = name;
    _namespaceName = namespaceName;
    _methods = methods;
  }

  public static ClassDeclarationBuilder Class(string name)
  {
    return new ClassDeclarationBuilder(name, Seq<MethodDeclarationBuilder>.Empty, string.Empty);
  }

  public ClassDeclarationBuilder With(params MethodDeclarationBuilder[] methodDeclarationBuilders)
  {
    var methods = _methods.Concat(methodDeclarationBuilders);
    return new ClassDeclarationBuilder(_name, methods, _namespaceName);
  }
    
  public ClassDeclarationBuilder WithNamespace(string namespaceName)
  {
    return new ClassDeclarationBuilder(_name, _methods, namespaceName);
  }

  public ClassDeclarationInfo Build()
  {
    var classDeclarationInfo = new ClassDeclarationInfo(_name, _namespaceName);
    classDeclarationInfo.AddMethodDeclarations(_methods.Select(m => m.Build()));
    return classDeclarationInfo;
  }
}
