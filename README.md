# LocalsInit.Fody

[![Build Status](https://dev.azure.com/Lucas-Trzesniewski/LocalsInit/_apis/build/status/LocalsInit.Fody?branchName=master)](https://dev.azure.com/Lucas-Trzesniewski/LocalsInit/_build/latest?definitionId=3&branchName=master)
[![NuGet package](https://img.shields.io/nuget/v/LocalsInit.Fody.svg?logo=NuGet)](https://www.nuget.org/packages/LocalsInit.Fody)
[![GitHub release](https://img.shields.io/github/release/ltrzesniewski/LocalsInit.Fody.svg?logo=GitHub)](https://github.com/ltrzesniewski/LocalsInit.Fody/releases)
[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/ltrzesniewski/LocalsInit.Fody/blob/master/LICENSE)

This is an add-in for [Fody](https://github.com/Fody/Fody) which lets you control the value of the `localsinit` flag on methods.

This is most useful to eliminate the zero-initialization of memory returned by `stackalloc`.

There is a [compiler feature proposal](https://github.com/dotnet/csharplang/blob/master/proposals/skip-localsinit.md) with the same goal. It will replace this weaver if it ever ships.

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

In practice, you'll probably want to set the default to `false` on the assembly level:

```C#
[assembly: LocalsInit(false)]
```

Then, look in your code for usages of the `stackalloc` keyword. In each case, if the code assumes the memory to be zero-initialized, add the `[LocalsInit(true)]` attribute to the method (or change the code to remove that assumption).
