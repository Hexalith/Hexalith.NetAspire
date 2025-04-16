# Hexalith.NetAspire

## Build Status

[![License: MIT](https://img.shields.io/github/license/hexalith/hexalith.NetAspire)](https://github.com/hexalith/hexalith/blob/main/LICENSE)
[![Discord](https://img.shields.io/discord/1063152441819942922?label=Discord&logo=discord&logoColor=white&color=d82679)](https://discordapp.com/channels/1102166958918610994/1102166958918610997)

[![Coverity Scan Build Status](https://scan.coverity.com/projects/27051/badge.svg)](https://scan.coverity.com/projects/hexalith-NetAspire)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/d48f6d9ab9fb4776b6b4711fc556d1c4)](https://app.codacy.com/gh/Hexalith/Hexalith.NetAspire/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.NetAspire&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.NetAspire)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.NetAspire&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.NetAspire)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.NetAspire&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.NetAspire)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.NetAspire&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.NetAspire)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.NetAspire&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.NetAspire)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.NetAspire&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.NetAspire)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.NetAspire&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.NetAspire)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.NetAspire&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.NetAspire)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.NetAspire&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.NetAspire)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.NetAspire&metric=bugs)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.NetAspire)

[![Build status](https://github.com/Hexalith/Hexalith.NetAspire/actions/workflows/build-release.yml/badge.svg)](https://github.com/Hexalith/Hexalith.NetAspire/actions)
[![NuGet](https://img.shields.io/nuget/v/Hexalith.NetAspire.svg)](https://www.nuget.org/packages/Hexalith.NetAspire)
[![Latest](https://img.shields.io/github/v/release/Hexalith/Hexalith.NetAspire?include_prereleases&label=preview)](https://github.com/Hexalith/Hexalith.NetAspire/pkgs/nuget/Hexalith.NetAspire)

## Overview

This repository provides libraries and a host project for integrating Hexalith applications with .NET Aspire. It facilitates the orchestration of various Hexalith services and components within the Aspire ecosystem.

Key components include:
- **Hexalith.NetAspire Libraries**: Core libraries (`Hexalith.NetAspire`, `Hexalith.NetAspire.Abstractions`, `Hexalith.NetAspire.Defaults`) for integrating Hexalith services with Aspire.
- **AspireHost**: The .NET Aspire application host project (`AspireHost/AspireHost.csproj`) responsible for orchestrating the application's services.
- **HexalithApp Submodule**: Contains the core Hexalith application modules.
- **Hexalith.Builds Submodule**: Provides shared build configurations and tooling.


## Repository Structure

The repository is organized as follows:

- [src](./src/README.md) Is the source code directory of your project.
- [src/libraries](./src/libraries/README.md) Is the source code directory where you will add your Nuget package projects.
- [src/examples](./src/examples/README.md) Contains example implementations of your projects.
- [src/servers](./src/servers/README.md) Is the source code directory where you will add your Docker container projects.
- [AspireHost](./AspireHost/README.md) Contains the .NET Aspire host project.
- [test](./test/README.md) Contains test projects for your packages.
- [Hexalith.Builds](./Hexalith.Builds/README.md) (Submodule) Contains shared build configurations and tools.
- [HexalithApp](./HexalithApp/README.md) (Submodule) Contains the core Hexalith application modules.

## Getting Started

### Prerequisites

- [Hexalith.Builds](https://github.com/Hexalith/Hexalith.Builds)
- [.NET 8 SDK](https://dotnet.microsoft.com/download) or later
- [PowerShell 7](https://github.com/PowerShell/PowerShell) or later
- [Git](https://git-scm.com/)

### Initializing the Package

To initialize the repository environment, including setting up necessary configurations or dependencies, run the initialization script:

```powershell
./initialize.ps1
```


### Git Submodules

This repository utilizes Git submodules to include external dependencies:

- **Hexalith.Builds**: Contains common build scripts, configurations, and tools used across Hexalith projects.
- **HexalithApp**: Includes the core application logic and modules for the Hexalith platform.

To initialize and fetch the content of these submodules, run the following Git command after cloning the repository:

```bash
git submodule update --init --recursive
```

The `initialize.ps1` script might also handle submodule initialization.


## Development

To build the entire solution, navigate to the root directory and run:

```bash
dotnet build Hexalith.NetAspire.sln
```

To run the application using the .NET Aspire host:

```bash
dotnet run --project AspireHost/AspireHost.csproj
```

This will start the Aspire dashboard and orchestrate the services defined in the host project.


## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
