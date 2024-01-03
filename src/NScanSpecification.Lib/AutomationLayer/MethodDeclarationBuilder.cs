using System.Collections.Generic;
using LanguageExt;
using NScan.SharedKernel.ReadingCSharpSourceCode;

namespace NScanSpecification.Lib.AutomationLayer;

public class MethodDeclarationBuilder(string name)
{
  private Seq<string> _attributes;

  public static MethodDeclarationBuilder Method(string name)
  {
    return new MethodDeclarationBuilder(name);
  }

  public MethodDeclarationBuilder DecoratedWithAttribute(string attributeName)
  {
    _attributes = _attributes.Add(attributeName);
    return this;
  }

  public MethodDeclarationInfo Build()
  {
    return new MethodDeclarationInfo(name, _attributes);
  }
}
