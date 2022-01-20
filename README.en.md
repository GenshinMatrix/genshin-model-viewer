・[English](README.en.md) ・[中文](README.md) ・[日本語](README.jp.md)

  # <img src="src/GenshinModelViewer/Resources/YunjinSideFace.png" width = "56" height = "56" alt="" align="left" /> Genshin Model Viewer

It also supports the preview of DMM models in other PMX formats.

  > Attributes: Support arbitrary rotation and translation! Support model threading up!

  ## Model format

  > Support model file format: `*.pmx` `*.7z` `*.zip` 

Tips: The archive type will automatically import the first PMX file in non decompression mode.

  > Support texture formats：`*.png` `*.bmp` `*.dds` `*.tga` etc..

Genshin Impact official model url: https://www.aplaybox.com/u/680828836

  ## Screen-shot

![demo2](screen-shot/demo-01.png)

![demo1](screen-shot/demo-02.png)

  ## Runtime Environment

> Windows7 (or higer) `.NET Framework 4.8`

  ## Usage

Click to select the model file or drag the file to the window.

  ## Download
  [![GitHub downloads](https://img.shields.io/github/downloads/emako/genshin-model-viewer/total)](https://github.com/emako/genshin-model-viewer/releases)
  [![GitHub downloads](https://img.shields.io/github/downloads/emako/genshin-model-viewer/latest/total)](https://github.com/emako/genshin-model-viewer/releases)

  > Download page: https://github.com/emako/genshin-model-viewer/releases
  >
  > Baidu CloudDisk: https://pan.baidu.com/s/1zyV4h6zHmqeWdlbeYyY0-A `7234`

  ## Dependency Support

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

  ## Build

Dev based on WPF.

Toolkit: `VS2022` `7z` `dotNET_Reactor`

Where `dotnet_Reactor ` is used to compile and synthesize into a single exe version.

  > Reference: [app_build.bat](app_build.bat)

  ## TODO

- [ ] Support importing folder.

  - [ ] Supports multiple PMX files when importing folders or archive files, and provides options.
  - [ ] Supports full screen mode and rewriting the form title bar.
  - [ ] Supports more interactive ui buttons.

  ## Known Problems

  1. When importing DirectX's DSS as an archive file, the import may fail.

  > Welcome to propose [Issues](https://github.com/emako/genshin-model-viewer/issues).

  ## Changelog

  **v1.0 **@2022-01-20

  > 1. First edition
