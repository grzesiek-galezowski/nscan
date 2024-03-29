﻿using System;

namespace NScan.DependencyPathBasedRules;

public class ProjectShadowingException(
  IDependencyPathBasedRuleTarget previousProject,
  IDependencyPathBasedRuleTarget newProject)
  : Exception("Two distinct projects are being added with the same path. " +
              $"{previousProject} would be shadowed by {newProject}. " +
              "This typically indicates a programmer error.");
