<p align="center">
<img src="./docs/images/Meting4Net.png" alt="Meting4Net">
</p>
<h1 align="center">Meting4Net</h1>

> :cake: Wow, such a powerful music API framework for .Net

[![repo size](https://img.shields.io/github/repo-size/yiyungent/Meting4Net.svg?style=flat)]()
[![LICENSE](https://img.shields.io/github/license/yiyungent/Meting4Net.svg?style=flat)](https://mit-license.org/)
[![nuget](https://img.shields.io/nuget/v/Meting4Net.svg?style=flat)](https://www.nuget.org/packages/Meting4Net/)
[![downloads](https://img.shields.io/nuget/dt/Meting4Net.svg?style=flat)](https://www.nuget.org/packages/Meting4Net/)


## Introduction

Meting4Net: <a href="https://github.com/metowolf/Meting" target="_blank">Meting</a> for .Net, Thanks to <a href="https://github.com/metowolf/Meting" target="_blank">Meting</a>.   
A powerful music API framework to accelerate your development
 + **Elegant** - Easy to use, a standardized format for all music platforms.
 + **Powerful** - Support various music platforms, including Tencent, NetEase, Xiami, KuGou, Baidu and more.
 + **Free** - Under MIT license, need I say more?

## Requirement

.NET Framework 4.5+ and Newtonsoft.Json 12.0.1+ installed.

## Installation

Require this package, with [NuGet](https://www.nuget.org/packages/Meting4Net), in the root directory of your project, if you use Visual Studio, then click **Tools** -> **NuGet Package Manager** -> **Package Manager Console** , make sure "Default project" is the project you want to install, enter the command below to install.

```bash
Install-Package Meting4Net -Version 0.1.0
```

## Quick Start

```csharp
using Meting4Net.Core;
   ...
// Initialize to netease API
Meting api = new Meting("netease");
// Get data
string jsonStr = api.FormatMethod(true).Search("Soldier", new Meting4Net.Core.Models.Standard.Options
{
    page = 1,
    limit = 50
});

return Content(jsonStr, "application/json");
//[{"id":35847388,"name":"Hello","artist":["Adele"],"album":"Hello","pic_id":"1407374890649284","url_id":35847388,"lyric_id":35847388,"source":"netease"},{"id":33211676,"name":"Hello","artist":["OMFG"],"album":"Hello",...
```

## Environment

- Operating environment: .NET Framework 4.5+    
- Development environment: Visual Studio Community 2017

## Related Projects

 - [Meting](https://github.com/metowolf/Meting)
 
 