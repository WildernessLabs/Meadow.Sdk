# Meadow_SDK Repo

This repo contains the DotNet SDK for Meadow. This SDK defines the MSBuild project properties and settings for Meadow projects and also includes the Meadow app templates for use in Visual Studio, Visual Studio for Mac, VS Code, and DotNet CLI.

This SDK was created with the help of Mikayla Hutchinson at Microsoft and uses the latest techniques to define the appropriate items to integrate with the DotNet Project system, and is a work in progress.

A DotNet SDK is essentially a set of default `property` and `target` settings that are applied to, and describe, a type of project within the DotNet family of build tools such as the DotNet CLI, Visual Studio, Visual Studio for Mac, and VS Code.

SDKs allow you to specify default required nuget packages (in this case, the Meadow core nuget), as well as settings that are specific to your custom project type.

SDKs are essentially empty .NET projects that are published as NuGet packages.

## Structure

The solution herein contains three main projects:

* *BasicMeadowApp_Sample* - This is the source for the `MeadowApp` template.
* *Meadow.Sdk* - This is the main SDK definition project. This is built into a .Nupkg
* *Project_Templates* - This project contains the project templates for Meadow and are uploaded to NuGet.

Additionally, at the top of the solution is a `Directory.Build.props` file which specifies properties that are globally scoped and will get applied to _all_ `.prop` files in the solution:

```xml
<Project>
  <!-- don't put any project with 'Sample' in its name in the output -->
  <PropertyGroup Condition="!$(MSBuildProjectName.Contains('Sample'))">
    <Version>0.1</Version><!-- TODO: figure out how to get this from the property settings $Version or whatever -->
    <GeneratePackageOnBuild>True</GeneratePackageOnBuild>
    <TargetFramework>net472</TargetFramework>
    <Author>Wilderness Labs</Author>
    <Copyright>2019</Copyright>
  </PropertyGroup>
</Project>
```

One important thing to note, there is a bug in Visual Studio (Windows) that requires the `TargetFramework` to be set on all `.props` files, even though it's set here.  

### BasicMeadowApp_Sample

This is a simple Meadow app that is used to test our SDK build. What's interesting here is `NuGet.config` file:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <packageSources>
        <add key="local" value="../Meadow.Sdk/bin/Debug/" />
    </packageSources>
</configuration>
```

Note that the SDK that this project uses, `Meadow.Sdk`, is specified as a nuget package and loaded locally from the **Meadow.Sdk** output, where a `Meadow.Sdk.nupkg` is created.

By specifying this, the Meadow core Nuget package is automatically brought in as a dependency.

### Meadow.Sdk

The `Meadow.Sdk` project is very simple and has two files in it, in an `sdk` folder:

* **Sdk.props**
* **Sdk.targets**

#### Sdk.props

Anything in this file will get inserted into the **top** of the project file for a new Meadow application, so any custom project properties go here. Currently, it only has one entry:

```xml
<Import Sdk="Microsoft.NET.Sdk" Project="Sdk.props" />
```

This tells the DotNet build system to import any properties from the base [`Microsoft.NET.Sdk`](https://www.nuget.org/packages/Microsoft.NET.Sdk) SDK. 

#### Sdk.targets

Anything in this file will get inserted into the **bottom** of the project file for a new Meadow application. Currently, we use it to specify that the `Meadow` NuGet package should be included as a dependency. 

We also specify a custom [`ProjectCapability`](https://github.com/microsoft/VSProjectSystem/blob/master/doc/overview/dynamicCapabilities.md) called `Meadow`. Project Capabilities are magic strings that replace the old project type GUIDs that tell the DotNet build system how to build/what features a particular project can have.

```xml
<Project>
  <PropertyGroup>
    <MeadowCoreVersion Condition="'$(MeadowCoreVersion)'==''">0.*</MeadowCoreVersion>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Meadow" Version="$(MeadowCoreVersion)"/>
    <ProjectCapability Include="Meadow"/>
  </ItemGroup>
  <Import Sdk="Microsoft.NET.Sdk" Project="Sdk.targets" />
</Project>
```

Project Capabilities can be queried by the build system or the IDE at runtime via an `AppliesTo` attribute to turn features on/off. 

[here](https://github.com/dotnet/project-system/search?q=ProjectCapability&unscoped_q=ProjectCapability) is an example of its usage within the DotNet build system, and [here](https://github.com/mhutch/MonoDevelop.AddinMaker/blob/eff386bfcce05918dbcfe190e9c2ed8513fe92ff/MonoDevelop.AddinMaker/AddinProjectFlavor.cs#L16) is an example of its usage in a custom Visual Studio for Mac extension.


For our use case, when a `Meadow` capability is encountered (`AppliesTo[ProjectCapability.Meadow]`), we should display a drop down in the IDE (via our custom extensions) that enumerate attached Meadow boards and allow a user to select which to deploy to.

In the case above, we also import the `Microsoft.NET.Sdk` targets, which tell the build system that the project can be built using regular the regular DotNet build chain.

### Project_Templates

Currently, there is only one template in here. The code and csproj file comes directly from the `BasicMeadowApp_Sample`. The interesting stuff in here is the `.template.config\template.json` file:

```json
{
    "$schema": "http://json.schemastore.org/template",
    "author": "Wilderness Labs",
    "classifications": [ "Stuff", "More Stuff" ],
    "name": "Basic Meadow App",
    "identity": "WildernessLabs.Meadow.Templates.BasicApp",
    "groupIdentity": "WildernessLabs.Meadow.Templates",
    "shortName": "Meadow",
    "tags": {
        "language": "C#",
        "type": "project"
    },
    "sourceName": "MeadowApp",
    "preferNameDirectory": true,
    "primaryOutputs": [
        { "path": "MeadowApp.csproj" }
    ]
}
```

The template config reference docs can be found [here](https://github.com/dotnet/templating/wiki/Reference-for-template.json).

TODO: note sure what `classifications`, if any, should be set. Same for `groupIdentity`.
