using System.Collections.Generic;
using System.Linq;

namespace TddXt.NScan.ReadingSolution.Ports
{
  internal static class CsharpProjectReferencesExtraction
  {
    public static IEnumerable<XmlProjectReference> ProjectReferences(XmlProject xmlProject)
    {
      var xmlItemGroups = xmlProject.ItemGroups
        .Where(ig => ig.ProjectReferences != null && ig.ProjectReferences.Any()).ToList();
      if (xmlItemGroups.Any())
      {
        return xmlItemGroups.First().ProjectReferences;
      }

      return new List<XmlProjectReference>();
    }
  }
}