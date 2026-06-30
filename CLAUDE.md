# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What NScan Is

NScan is a .NET structural constraints analyzer — it reads a `.sln` file and a rules file, then reports violated dependency conventions. It is distributed as a .NET Standard 2.1 library (`NScan`), a console tool (`NScan.Console`), and a Cake build addin (`Cake.NScan`). The repository eats its own dog food: `nscan.config` at the root runs NScan against itself during every build.

## Build System

The build uses **Bullseye** (a task runner) via a dedicated `src/Build/Build.csproj` console app. All build/test/pack operations go through it:

```bash
# Full default build (Build + Tests + Pack)
dotnet run --project src/Build/Build.csproj -c Release -- default

# Specific targets
dotnet run --project src/Build/Build.csproj -c Release -- RunNScanUnitTests
dotnet run --project src/Build/Build.csproj -c Release -- RunE2ETests
dotnet run --project src/Build/Build.csproj -c Release -- Clean
dotnet run --project src/Build/Build.csproj -c Release -- Pack
dotnet run --project src/Build/Build.csproj -c Release -- Push
```

The `default` target runs: `BuildNScan` → `BuildNScanConsole` → `BuildCakeNScan` → `RunNScanUnitTests` → `Pack`. `BuildNScan` depends on `RunPreviousNScan`, which validates the solution against `nscan.config` using the previously-released NuGet package.

**Version** is hardcoded as `0.400.0` in `src/Build/Program.cs`.

## Running Tests Directly

All test projects are named `*Specification.csproj` and live alongside their production counterparts:

```bash
# Run a specific test project
dotnet test src/NScan.MainSpecification -c Release -p:VersionPrefix=0.400.0

# Run all unit/component tests (what RunNScanUnitTests does)
# Glob: src/**/*Specification.csproj + src/NScanSpecification.Component/

# Run E2E tests only
dotnet test src/NScanSpecification.E2E -c Release -p:VersionPrefix=0.400.0
```

## Architecture

### Hexagonal / Ports & Adapters

The core analysis is decoupled from I/O through two output ports defined in `NScan.SharedKernel`:
- `INScanOutput` — writes the analysis report and version string
- `INScanSupport` — logs errors, skipped projects, and discovered rules

Both `NScan.Console` and `Cake.NScan` implement these ports and call `NScanMain.Run(InputArgumentsDto, INScanOutput, INScanSupport)` directly.

### Three Rule Engines

`Analysis` (in `NScan.Main`) coordinates three independent evaluation engines, each a separate project:

| Project | Rule examples | What it analyzes |
|---|---|---|
| `NScan.Domain.DependencyPathBasedRules` | `independentOf` | Cross-project dependency chains |
| `NScan.Domain.ProjectScopedRules` | `hasCorrectNamespaces`, `hasProperty`, `hasAttributesOn` | Per-project file properties and declared namespaces |
| `NScan.Domain.NamespaceBasedRules` | `hasNoCircularUsings`, `hasNoUsings` | Namespace-level `using` dependencies within projects |

Each engine has its own: rule factory, violation factory, report format adapter, and a union DTO (see below).

### Union / Visitor Pattern for Rule Variants

`NScan.Lib` provides generic `Union<T1, T2, …>` types that model discriminated unions. Rule complement DTOs (e.g., `NamespaceBasedRuleUnionDto`) wrap the concrete rule types and expose `Accept(IUnionVisitor<…>)` / `Accept<TReturn>(IUnionTransformingVisitor<…>)` for type-safe dispatch without casting.

### Rules File Parsing

Rules are parsed from the text rules file (e.g., `nscan.config`) using **Sprache** (a parser-combinator library). The parser lives in `NScan.Adapters.Secondary` and produces the three union DTO collections that are handed to `Analysis`.

### Functional Style

The codebase uses `LanguageExt` (`Seq`, `HashMap`, `Option`/`Maybe`) for immutable collections and optional values throughout domain logic.

## Test Style

Framework: **xUnit** + **NSubstitute** (mocks) + **AwesomeAssertions** (fluent assertions) + **AnyRoot** (`Any.Instance<T>()` for arbitrary test data).

Tests are written BDD-style with `//GIVEN`, `//WHEN`, `//THEN` comments. Complex subjects are assembled via builder classes (e.g., `AnalysisBuilder`). Every test project imports shared `global using` aliases.

## Key Configuration Files

- `src/Directory.Build.props` — shared build properties for all projects (nullable enabled, warnings as errors)
- `src/Directory.Packages.props` — centralized NuGet package versions
- `nscan.config` — NScan rules applied to this solution itself on every build
