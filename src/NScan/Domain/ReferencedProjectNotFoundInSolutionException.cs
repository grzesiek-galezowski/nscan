using System;
using System.Collections.Generic;

namespace TddXt.NScan.Domain
{
  public class ReferencedProjectNotFoundInSolutionException : Exception
  {
    public ReferencedProjectNotFoundInSolutionException(
      string message, KeyNotFoundException keyNotFoundException)
      : base(message, keyNotFoundException)
    {
    }
  }
}