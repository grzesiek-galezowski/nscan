# AGENTS.md

## Repository overview

NScan is a .NET structural constraints analyzer. It is delivered as the `NScan` .NET Standard 2.1 library, the `NScan.Console` command-line tool, and the `Cake.NScan` build add-in.

## Build and test

Use the Bullseye build project for repository-wide operations:

```bash
dotnet run --project src/Build/Build.csproj -c Release -- default
```

Useful targets include `RunNScanUnitTests`, `RunE2ETests`, `Clean`, and `Pack`. Individual test projects are named `*Specification.csproj`; run them with `dotnet test` and the repository version passed as `-p:VersionPrefix` when needed.

The build runs NScan against the repository's own `nscan.config`, so changes to project structure, dependencies, namespaces, or rules may affect the build.

## Architecture

The core analysis uses a ports-and-adapters design. `NScan.Main` coordinates rule engines for dependency paths, project-scoped properties/namespaces, and namespace-level using dependencies. Keep I/O behind the `NScan.SharedKernel` output and support ports. Rule variants use the union/visitor types in `NScan.Lib`.

Rules are parsed with Sprache in `NScan.Adapters.Secondary`. Preserve the existing functional style based on LanguageExt immutable collections and options.

## Code and test conventions

- Follow the shared settings in `src/Directory.Build.props`; warnings are treated as errors and nullable reference types are enabled.
- Use xUnit, NSubstitute, AwesomeAssertions, and AnyRoot as established by the existing tests.
- Prefer the existing BDD test style (`//GIVEN`, `//WHEN`, `//THEN`) and builder classes for complex subjects.
- Keep package versions centralized in `src/Directory.Packages.props`.

## Change discipline

Inspect nearby code and existing tests before introducing new patterns. Add or update tests with behavior changes, and run the narrowest relevant test target followed by the full build when practical. Do not overwrite unrelated working-tree changes.
