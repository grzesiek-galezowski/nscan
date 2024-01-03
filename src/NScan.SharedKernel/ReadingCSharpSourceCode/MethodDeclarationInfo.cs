using System.Collections.Generic;
using LanguageExt;

namespace NScan.SharedKernel.ReadingCSharpSourceCode;

public class MethodDeclarationInfo(string name, Seq<string> attributes)
{
  public string Name { get; } = name;
  public Seq<string> Attributes { get; } = attributes;
}
