using System;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.Lib;

namespace TddXt.NScan.Specification.AutomationLayer
{
  public class HasNoCircularUsingsMessage : GenericReportedMessage<HasNoCircularUsingsMessage>
  {
    public HasNoCircularUsingsMessage(string returnValue) : base(returnValue)
    {
    }

    public HasNoCircularUsingsMessage CycleFound(string projectName, params string[] cyclePath)
    {
      return NewInstance(this + Environment.NewLine +
                         $"Discovered cycle(s) in project {projectName}:{Environment.NewLine}" +
                         Format(cyclePath));
    }

    private static string Format(string[] cyclePath)
    {
      var result = $"Cycle 1:{Environment.NewLine}";
      for (var i = 0; i < cyclePath.Length; ++i)
      {
        result += i.Indentations() + cyclePath[i] + Environment.NewLine;
      }

      return result;
    }

    protected override HasNoCircularUsingsMessage NewInstance(string str)
    {
      return new HasNoCircularUsingsMessage(str);
    }

    public static HasNoCircularUsingsMessage HasNoCircularUsings(string projectGlob)
    {
      return new HasNoCircularUsingsMessage($"{projectGlob} hasNoCircularUsings: ");
    }
  }
}