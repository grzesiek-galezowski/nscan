﻿using System.Collections.Generic;
using NScan.SharedKernel.ReadingCSharpSourceCode;

namespace NScanSpecification.Lib.AutomationLayer
{
  public class MethodDeclarationBuilder
  {
    private readonly string _name;
    private readonly List<string> _attributes = new();

    public MethodDeclarationBuilder(string name)
    {
      _name = name;
    }

    public static MethodDeclarationBuilder Method(string name)
    {
      return new(name);
    }

    public MethodDeclarationBuilder DecoratedWithAttribute(string attributeName)
    {
      _attributes.Add(attributeName);
      return this;
    }

    public MethodDeclarationInfo Build()
    {
      return new(_name, _attributes);
    }
  }

}
