・[English](README.en.md) ・[中文](README.md) ・[日本語](README.jp.md)

# <img src="src/GenshinModelViewer/Resources/YunjinSideFace.png" width = "56" height = "56" alt="" align="left" /> 原神模型预览器

同时亦支持其他PMX格式DMM模型的预览。

> 特性：支持任意旋转平移！支持穿模！

## 模型格式

> 支持模型文件格式： `*.pmx` `*.7z` `*.zip` 

※归档类型将自动非解压方式导入首个`*.pmx`文件

> 支持贴图格式：`*.png` `*.bmp` `*.dds` `*.tga` 等

原神官方模型地址：https://www.aplaybox.com/u/680828836

## 截图预览

![demo2](screen-shot/demo-01.png)

![demo1](screen-shot/demo-02.png)

## 运行环境

-  Windows7及以上
-  .NET Framework 4.8

## 使用方法

点击选择模型文件或将文件拖拽到窗口里。

## 程序下载
[![GitHub downloads](https://img.shields.io/github/downloads/emako/genshin-model-viewer/total)](https://github.com/emako/genshin-model-viewer/releases)
[![GitHub downloads](https://img.shields.io/github/downloads/emako/genshin-model-viewer/latest/total)](https://github.com/emako/genshin-model-viewer/releases)

> 下载页：https://github.com/emako/genshin-model-viewer/releases
>
> 度盘：https://pan.baidu.com/s/1zyV4h6zHmqeWdlbeYyY0-A `7234`

## 依赖支持

- QuickLook.Plugin.HelixViewer & HelixToolkit

> https://github.com/ShiinaManatsu/QuickLook.Plugin.HelixViewer
>
> https://github.com/helix-toolkit/helix-toolkit

- Pfim

> https://github.com/nickbabcock/Pfim

- SevenZipSharp & 7-Zip

>https://github.com/tomap/SevenZipSharp
>
>https://www.7-zip.org/

- SharpVectors

> https://github.com/ElinamLLC/SharpVectors

## 编译生成

基于WPF开发。

所需工具：`VS2022` `7z` `dotNET_Reactor`

其中dotNET_Reactor用于编译合成为单个exe的版本。

> 参考：[app_build.bat](app_build.bat)

## 开发任务

- [ ] 支持导入文件夹
- [ ] 支持在导入文件夹或归档文件时多个pmx文件则提供选项
- [ ] 支持全屏模式以及重写窗体标题栏
- [ ] 支持交互的按钮

## 已知问题

1. 以归档文件形式导入`*.dss`的DirectX贴图时可能会导入失败

> 欢迎提出[Issues](https://github.com/emako/genshin-model-viewer/issues)

## 更新日志

**v1.0 **@2022-01-20

> 1. 初版

