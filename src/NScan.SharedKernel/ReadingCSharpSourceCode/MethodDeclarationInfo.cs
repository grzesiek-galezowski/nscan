using System.Collections.Generic;

namespace NScan.SharedKernel.ReadingCSharpSourceCode;

public class MethodDeclarationInfo(string name, IReadOnlyList<string> attributes)
{
  public string Name { get; } = name;
  public IReadOnlyList<string> Attributes { get; } = attributes;
}
