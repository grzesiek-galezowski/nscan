using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NScan.Adapter.ReadingCSharpSolution.ReadingCSharpSourceCode;

namespace NScanSpecification.Lib.AutomationLayer
{
  public class ClassDeclarationBuilder
  {
    private readonly string _name;
    private readonly string _namespaceName;
    private readonly ImmutableList<MethodDeclarationBuilder> _methods;

    private ClassDeclarationBuilder(string name, IEnumerable<MethodDeclarationBuilder> methods, string namespaceName)
    {
      _name = name;
      _namespaceName = namespaceName;
      _methods = ImmutableList.Create(methods.ToArray());
    }

    public static ClassDeclarationBuilder Class(string name)
    {
      return new ClassDeclarationBuilder(name, new List<MethodDeclarationBuilder>(), string.Empty);
    }

    public ClassDeclarationBuilder With(params MethodDeclarationBuilder[] methodDeclarationBuilders)
    {
      return new ClassDeclarationBuilder(_name, _methods.Concat(methodDeclarationBuilders), _namespaceName);
    }
    
    public ClassDeclarationBuilder WithNamespace(string namespaceName)
    {
      return new ClassDeclarationBuilder(_name, _methods, namespaceName);
    }

    public ClassDeclarationInfo Build()
    {
      var classDeclarationInfo = new ClassDeclarationInfo(_name, _namespaceName);
      classDeclarationInfo.Methods.AddRange(_methods.Select(m => m.Build()));
      return classDeclarationInfo;
    }
  }
}