using System;
using System.Collections.Generic;

namespace MyTool.App
{
  public class ReferencedProjectNotFoundInSolutionException : Exception
  {
    public ReferencedProjectNotFoundInSolutionException(string s, KeyNotFoundException keyNotFoundException)
      : base(s, keyNotFoundException)
    {
    }
  }
}