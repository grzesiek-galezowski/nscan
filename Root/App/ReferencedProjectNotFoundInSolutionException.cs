using System;
using System.Collections.Generic;

namespace NScanRoot.App
{
  public class ReferencedProjectNotFoundInSolutionException : Exception
  {
    public ReferencedProjectNotFoundInSolutionException(string s, KeyNotFoundException keyNotFoundException)
      : base(s, keyNotFoundException)
    {
    }
  }
}