using System;
using System.Collections.Generic;

namespace NScan.DependencyPathBasedRules;

public class ReferencedProjectNotFoundInSolutionException(string message, KeyNotFoundException keyNotFoundException)
  : Exception(message, keyNotFoundException);
