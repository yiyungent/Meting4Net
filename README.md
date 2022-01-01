<p align="center">
<img src="docs/_images/Meting4Net.png" alt="Meting4Net">
</p>
<h1 align="center">Meting4Net</h1>

> :cake: 一个强大的音乐API框架

[![repo size](https://img.shields.io/github/repo-size/yiyungent/Meting4Net.svg?style=flat)]()
[![LICENSE](https://img.shields.io/github/license/yiyungent/Meting4Net.svg?style=flat)](https://github.com/yiyungent/Meting4Net/blob/master/LICENSE)
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Fyiyungent%2FMeting4Net.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2Fyiyungent%2FMeting4Net?ref=badge_shield)
[![nuget](https://img.shields.io/nuget/v/Meting4Net.svg?style=flat)](https://www.nuget.org/packages/Meting4Net/)
[![downloads](https://img.shields.io/nuget/dt/Meting4Net.svg?style=flat)](https://www.nuget.org/packages/Meting4Net/)
[![QQ Group](https://img.shields.io/badge/QQ%20Group-894031109-deepgreen)](https://jq.qq.com/?_wv=1027&k=q5R82fYN)


[English](README_en.md)

## 介绍

Meting4Net: <a href="https://github.com/metowolf/Meting" target="_blank">Meting</a> for .Net, thanks to <a href="https://github.com/metowolf/Meting" target="_blank">Meting</a>.   

一个强大的音乐API框架促进你的开发
 + **优雅** - 简单易用, 对于所有平台均可一个标准格式.
 + **丰富** - 支持多个音乐平台, 包括 腾讯QQ音乐, 网易云音乐, 虾米音乐, 酷狗, 百度等.
 + **免费** - MIT协议 发布
 
## TODO

- [x] 网易云音乐 Meting Open API 移植完成 v0.1.0
- [x] 腾讯QQ音乐 Meting Open API 移植完成 v0.2.0
- [x] 酷狗音乐 Meting Open API 移植完成 v1.0.0
- [x] 虾米, 百度音乐 Meting Open API 移植完成 v1.1.0
- [x] 支持返回强类型实体对象 eg: SearchObj("千里邀月") v1.1.0  

#### 注

- Meting4Net 跟随 <a href="https://github.com/metowolf/Meting" target="_blank">Meting</a> 更新

## 持续集成

| 环境 | 平台 | 状态 |
| :------: | :------: | :------: |
| Ubuntu-16.04 | .net core 2.0.0 | [![Build Status](https://dev.azure.com/Meting4Net/Meting4Net/_apis/build/status/yiyungent.Meting4Net?branchName=master)](https://dev.azure.com/Meting4Net/Meting4Net/_build/latest?definitionId=1&branchName=master) |
| Linux | mono 5.18.0.240 | [![Build Status](https://travis-ci.com/yiyungent/Meting4Net.svg?branch=master)](https://travis-ci.com/yiyungent/Meting4Net) |

## 依赖

只需要满足下方其中一条.

- .NET Framework (>= 4.0) 且 Newtonsoft.Json (>= 4.5.11) 被安装.
- .NET Standard (>= 2.0) 且 Microsoft.CSharp (>= 4.5.0), Newtonsoft.Json (>= 9.0.1) 被安装.

## 安装

推荐使用 [NuGet](https://www.nuget.org/packages/Meting4Net), 在你项目的根目录 执行下方的命令, 如果你使用 Visual Studio, 这时依次点击 **Tools** -> **NuGet Package Manager** -> **Package Manager Console** , 确保 "Default project" 是你想要安装的项目, 输入下方的命令进行安装.

```bash
PM> Install-Package Meting4Net
```

## 快速开始

```csharp
using Meting4Net.Core;
   ...
// 初始化 网易云音乐API
Meting api = new Meting(ServerProvider.Netease);
// 获得 json 数据
string jsonStr = api.FormatMethod(true).Search("Soldier", new Meting4Net.Core.Models.Standard.Options
{
    page = 1,
    limit = 50
});

return Content(jsonStr, "application/json");
//[{"id":"35847388","name":"Hello","artist":["Adele"],"album":"Hello","pic_id":"1407374890649284","url_id":"35847388","lyric_id":"35847388","source":"netease"},{"id":"33211676","name":"Hello","artist":["OMFG"],"album":"Hello",...
```

## 使用

- [详细文档(/docs)](https://yiyungent.github.io/Meting4Net "在线文档")

## 环境

- 运行环境: .NET Framework (>= 4.0) or .NET Standard (>= 2.0)    
- 开发环境: Visual Studio Community 2017

## 相关项目

- [Meting](https://github.com/metowolf/Meting)
 
## 鸣谢

- 本项目由 <a href="https://github.com/metowolf/Meting" target="_blank">Meting</a> 移植而来，感谢原作者 metowolf 的贡献
- 网易云音乐API加密模块参考自 <a href="https://github.com/IllyaTheHath/Music163Api" target="_blank">Music163Api</a>，感谢作者 IllyaTheHath 的贡献

## 开放源代码许可

[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Fyiyungent%2FMeting4Net.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2Fyiyungent%2FMeting4Net?ref=badge_large)

## Contributors

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore -->
<table><tr><td align="center"><a href="https://yiyungent.github.io"><img src="https://avatars1.githubusercontent.com/u/16939388?v=4" width="100px;" alt="yiyun"/><br /><sub><b>yiyun</b></sub></a><br /><a href="https://github.com/yiyungent/Meting4Net/commits?author=yiyungent" title="Code">💻</a> <a href="https://github.com/yiyungent/Meting4Net/commits?author=yiyungent" title="Documentation">📖</a> <a href="#example-yiyungent" title="Examples">💡</a> <a href="#maintenance-yiyungent" title="Maintenance">🚧</a> <a href="https://github.com/yiyungent/Meting4Net/commits?author=yiyungent" title="Tests">⚠️</a></td></tr></table>

<!-- ALL-CONTRIBUTORS-LIST:END -->

This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!

## 赞助者

感谢这些来自爱发电的赞助者：

<!-- AFDIAN-ACTION:START -->

<a href="https://afdian.net/u/6c944aa0a55f11eabd5f52540025c377">
    <img src="https://pic1.afdiancdn.com/user/6c944aa0a55f11eabd5f52540025c377/avatar/e0b9977363fe0b475e0fb6300c43b4be_w480_h480_s13.jpg?imageView2/1/w/120/h/120" width="40" height="40" alt="MonoLogueChi" title="MonoLogueChi"/>
</a>

<details>
  <summary>点我 打开/关闭 赞助者列表</summary>

<a href="https://afdian.net/u/6c944aa0a55f11eabd5f52540025c377">
MonoLogueChi
</a>
<span>( 1 次赞助, 共 ￥28.2 ) 留言: 感谢你的开源项目</span><br>

</details>
<!-- 注意: 尽量将标签前靠,否则经测试可能被 GitHub 解析为代码块 -->
<!-- AFDIAN-ACTION:END -->

本项目使用 [afdian-action](https://github.com/yiyungent/afdian-action) 自动更新赞助者列表

## Donate

Meting4Net is an MIT licensed open source project and completely free to use. However, the amount of effort needed to maintain and develop new features for the project is not sustainable without proper financial backing.

We accept donations through these channels:
- <a href="https://afdian.net/@yiyun" target="_blank">爱发电</a>

## Author

**Meting4Net** © [yiyun](https://github.com/yiyungent), Released under the [MIT](./LICENSE) License.<br>
Authored and maintained by yiyun with help from contributors ([list](https://github.com/yiyungent/Meting4Net/contributors)).

> GitHub [@yiyungent](https://github.com/yiyungent)

