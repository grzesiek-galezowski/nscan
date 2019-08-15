using System.Collections.Generic;

namespace NScan.Adapter.ReadingCSharpSolution.ReadingCSharpSourceCode
{
  public class MethodDeclarationInfo
  {
    public string Name { get; }
    public IReadOnlyList<string> Attributes { get; }

    public MethodDeclarationInfo(string name, IReadOnlyList<string> attributes)
    {
      Name = name;
      Attributes = attributes;
    }

  }
}