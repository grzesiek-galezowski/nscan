using System;
using System.Collections.Generic;

namespace NScan.Domain.Root
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