using System.Collections.Generic;
using NScan.SharedKernel.ReadingCSharpSourceCode;

namespace NScanSpecification.Lib.AutomationLayer;

public class MethodDeclarationBuilder(string name)
{
  private readonly List<string> _attributes = new();

  public static MethodDeclarationBuilder Method(string name)
  {
    return new MethodDeclarationBuilder(name);
  }

  public MethodDeclarationBuilder DecoratedWithAttribute(string attributeName)
  {
    _attributes.Add(attributeName);
    return this;
  }

  public MethodDeclarationInfo Build()
  {
    return new MethodDeclarationInfo(name, _attributes);
  }
}
