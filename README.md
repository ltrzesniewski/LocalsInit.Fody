# LocalsInit.Fody

[![Build](https://github.com/ltrzesniewski/LocalsInit.Fody/workflows/Build/badge.svg)](https://github.com/ltrzesniewski/LocalsInit.Fody/actions?query=workflow%3ABuild)
[![NuGet package](https://img.shields.io/nuget/v/LocalsInit.Fody.svg?logo=NuGet)](https://www.nuget.org/packages/LocalsInit.Fody)
[![GitHub release](https://img.shields.io/github/release/ltrzesniewski/LocalsInit.Fody.svg?logo=GitHub)](https://github.com/ltrzesniewski/LocalsInit.Fody/releases)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/ltrzesniewski/LocalsInit.Fody/blob/master/LICENSE)

This is an add-in for [Fody](https://github.com/Fody/Fody) which lets you control the value of the `localsinit` flag on methods.

This is most useful to eliminate the zero-initialization of memory returned by `stackalloc`.

## This is now a compiler feature

Roslyn now lets you control this with [`SkipLocalsInitAttribute`](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.skiplocalsinitattribute).

Prefer the compiler feature to this weaver whenever possible. See also [the spec](https://github.com/dotnet/csharplang/blob/master/proposals/csharp-9.0/skip-localsinit.md).

If you want to target frameworks prior to .NET 5, you can still use the compiler feature by defining the attribute in your own assembly:

```C#
namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Module
                    | AttributeTargets.Class
                    | AttributeTargets.Struct
                    | AttributeTargets.Interface
                    | AttributeTargets.Constructor
                    | AttributeTargets.Method
                    | AttributeTargets.Property
                    | AttributeTargets.Event, Inherited = false)]
    internal sealed class SkipLocalsInitAttribute : Attribute
    {
    }
}
```

## Installation

- Install the NuGet packages [`Fody`](https://www.nuget.org/packages/Fody) and [`LocalsInit.Fody`](https://www.nuget.org/packages/LocalsInit.Fody). Installing `Fody` explicitly ensures the latest version is used:

  ```
  PM> Install-Package Fody
  PM> Install-Package LocalsInit.Fody
  ```

- Add the `PrivateAssets="all"` metadata attribute to the `<PackageReference />` items of `Fody` and `LocalsInit.Fody` in your project file, so they won't be listed as dependencies.

- If you already have a `FodyWeavers.xml` file in the root directory of your project, add the `<LocalsInit />` tag there. This file will be created on the first build if it doesn't exist:

  ```XML
  <?xml version="1.0" encoding="utf-8" ?>
  <Weavers>
    <LocalsInit />
  </Weavers>
  ```

See [Fody usage](https://github.com/Fody/Home/blob/master/pages/usage.md) for general guidelines, and [Fody Configuration](https://github.com/Fody/Home/blob/master/pages/configuration.md) for additional options.

## Usage

Add a `[LocalsInit(false)]` attribute to disable the `localsinit` flag at the desired level.

You can also add `[LocalsInit(true)]` attributes at lower levels to override the value set at a higher level.

In practice, you'll probably want to set the default to `false` at the assembly level. You can do so either as an assembly-level attribute:

```C#
[assembly: LocalsInit(false)]
```

Or you can set the default value in `FodyWeavers.xml`:

```XML
<LocalsInit Default="false" />
```

Then, look in your code for usages of the `stackalloc` keyword. In each case, if the code assumes the memory to be zero-initialized, add the `[LocalsInit(true)]` attribute to the method, or change the code to remove that assumption.

## Configuration

The `LocalsInit` element in `FodyWeavers.xml` accepts the following attribute:

 - `Default="true|false"` to set the default value of the `localsinit` flag for the assembly. This is equivalent to setting an assembly-level attribute.
