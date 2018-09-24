using System.Collections.Generic;
using System.Linq;
using NScanRoot.App;

namespace NScanRoot.CompositionRoot
{
  public class PlainProjectPathFormat : IProjectPathFormat
  {
    public string ApplyTo(List<IReferencedProject> violationPath)
    {
      return violationPath.Skip(1).Aggregate(
        "[" + violationPath.First().ToString() + "]", 
        (total, current) => total + "->" + "[" + current.ToString() + "]");
    }
  }
}