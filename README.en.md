・[English](README.en.md) ・[中文](README.md) ・[日本語](README.jp.md)

  # <img src="src/GenshinModelViewer/Resources/UI_AvatarIcon_Side_Yunjin.png" width = "56" height = "56" alt="" align="left" /> Genshin Model Viewer

It also supports the preview of DMM models in other PMX formats.

  > Attributes: Support arbitrary rotation and translation! Support model threading up!

  ## Model format

  > Support model file format: `*.pmx` `*.7z` `*.zip` 

Tips: The archive type will automatically import the first PMX file in non decompression mode.

  > Support model file ref texture formats：`*.png` `*.bmp` `*.dds` `*.tga` etc..

[Model Download](https://www.aplaybox.com/u/680828836)

  ## Usages

![demo2](screen-shot/demo-01.png)

![demo1](screen-shot/demo-02.png)

> Click to select the model file or drag the file to the window.

| Input        | Operation  |
| ------------ | ---------- |
| Left Mouse   | Pan        |
| Middle Mouse | Pan & Zoom |
| Right Mouse  | Rotate     |

  ## Runtime Environment

`Win10 x64` `.NET 6`

  ## Download
  [![GitHub downloads](https://img.shields.io/github/downloads/emako/genshin-model-viewer/total)](https://github.com/emako/genshin-model-viewer/releases)
  [![GitHub downloads](https://img.shields.io/github/downloads/emako/genshin-model-viewer/latest/total)](https://github.com/emako/genshin-model-viewer/releases)

  > Download page: https://github.com/emako/genshin-model-viewer/releases
  >

  ## TODO

- [ ] Support importing folder.


- [ ] Supports multiple PMX files when importing folders or archive files, and provides options.
- [ ] Supports full screen mode and rewriting the form title bar.
- [ ] Supports more interactive ui buttons.

## FAQs

> Texture error after importing model?

- Texture added with the Sphere algorithm are not supported. At the same time, the DMM model officially released by the original Genshin Impact not use Sphere.

---

Welcome to propose [Issues](https://github.com/emako/genshin-model-viewer/issues).

## Changelog

[CHANGELOG.md](CHANGELOG.md)

  ## Dependency

  - [HelixToolkit](https://github.com/helix-toolkit/helix-toolkit)
- [QuickLook.Plugin.HelixViewer](https://github.com/ShiinaManatsu/QuickLook.Plugin.HelixViewer)

- [Pfim](https://github.com/nickbabcock/Pfim)

- [SevenZipSharp](https://github.com/squid-box/SevenZipSharp)
- [7-Zip](https://www.7-zip.org/)

## License

[LICENSE](LICENSE)

