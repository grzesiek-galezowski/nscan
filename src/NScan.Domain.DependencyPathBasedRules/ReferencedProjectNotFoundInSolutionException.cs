using System;
using System.Collections.Generic;

namespace NScan.DependencyPathBasedRules;

public class ReferencedProjectNotFoundInSolutionException : Exception
{
  public ReferencedProjectNotFoundInSolutionException(string message, KeyNotFoundException keyNotFoundException) : base(message, keyNotFoundException)
  {
  }

  public ReferencedProjectNotFoundInSolutionException(string message) : base(message)
  {
  }
}
