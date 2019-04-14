using System;
using System.Collections.Generic;

namespace TddXt.NScan.ReadingCSharpSourceCode
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