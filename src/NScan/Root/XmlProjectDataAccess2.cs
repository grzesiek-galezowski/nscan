using System;
using System.Collections.Generic;
using System.Linq;
using Functional.Maybe;
using NScan.Domain.ProjectScopedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.ReadingCSharpSourceCode;
using NScan.SharedKernel.ReadingSolution.Lib;
using NScan.SharedKernel.ReadingSolution.Ports;

namespace NScan.Domain.Root
{
  public class XmlProjectDataAccess2 : IXmlProjectDataAccess2
  {
    public XmlProjectDataAccess2(XmlProject xmlProject)
    {
      var xmlProjectDataAccess = new XmlProjectDataAccess(xmlProject);
      AssemblyName = xmlProjectDataAccess.DetermineAssemblyName();
      SourceCodeFiles = xmlProjectDataAccess.SourceCodeFiles();
      TargetFramework = xmlProject.PropertyGroups.First(pg => pg.TargetFramework != null).TargetFramework;
      Id = xmlProjectDataAccess.Id();
      PackageReferences = xmlProjectDataAccess.XmlPackageReferences()
        .Select(r => new PackageReference(r.Include, r.Version)).ToList();
      AssemblyReferences = xmlProjectDataAccess.XmlAssemblyReferences()
        .Select(r => new AssemblyReference(r.Include, r.HintPath)).ToList();
      ProjectIds = xmlProjectDataAccess.ProjectReferences()
        .Select(dto => new ProjectId(dto.FullIncludePath.ToString())).ToArray();
    }

    public string AssemblyName { get; }

    public ProjectId Id { get; }

    public IEnumerable<SourceCodeFileDto> SourceCodeFiles { get; }

    public string TargetFramework { get; }

    public List<PackageReference> PackageReferences { get; }

    public List<AssemblyReference> AssemblyReferences { get; }

    public ProjectId[] ProjectIds { get; }
  }
}