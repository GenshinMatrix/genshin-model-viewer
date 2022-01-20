・[English](README.en.md) ・[中文](README.md) ・[日本語](README.jp.md)

  # <img src="src/GenshinModelViewer/Resources/YunjinSideFace.png" width = "56" height = "56" alt="" align="left" /> 原神モデルビューア

他のPMX形式DMMモデルのプレビューもサポートされています。

  > 目玉：任意回転移動可！モデル内部観察可！

  ## モデルのフォーマット

  > サポートされているモデルのフォーマット： `*.pmx` `*.7z` `*.zip` 

※アーカイブタイプのファイルは自動的に非解凍方式を最初の`*.pmx`の方を導入する。

  > サポートされているテクスチャのフォーマット：`*.png` `*.bmp` `*.dds` `*.tga` など。

原神公式モデルアドレス：https://www.aplaybox.com/u/680828836

  ## スクリーンショット

![demo2](screen-shot/demo-01.png)

![demo1](screen-shot/demo-02.png)

  ## 実行環境

> Windows7 (又はそれ以上) `.NET Framework 4.8`

  ## 使い方

モデルファイルを選択する、又はウィンドウにドラッグ&ドロップして始まる。

  ## ダウンロード
  [![GitHub downloads](https://img.shields.io/github/downloads/emako/genshin-model-viewer/total)](https://github.com/emako/genshin-model-viewer/releases)
  [![GitHub downloads](https://img.shields.io/github/downloads/emako/genshin-model-viewer/latest/total)](https://github.com/emako/genshin-model-viewer/releases)

  > ダウンロードページ：https://github.com/emako/genshin-model-viewer/releases
  >
  > Baiduクラウドディスク：https://pan.baidu.com/s/1zyV4h6zHmqeWdlbeYyY0-A `7234`

  ## 依存とサポート

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

  ## ビルド

WPFに基づいた開発。

必要なツール：`VS2022` `7z` `dotNET_Reactor`

そのうち`dotNET_Reactor`は、単一exeに合成するために使用されます。

  > 参照：[app_build.bat](app_build.bat)

  ## タスク

- [ ] インポートフォルダのサポート。


- [ ] フォルダまたはアーカイブのインポート時に複数のpmxファイルの中から１つ選択できる機能。
- [ ] フルスクリーンモードとWindowのタイトルバーのリペイントをサポート。
- [ ] 更なるUIボタンの追加。

## 既知の問題

1. アーカイブ形式で`*.dss`をインポートするのDirectX形式の読み込みに失敗する場合があります。

> [Issues](https://github.com/emako/genshin-model-viewer/issues) へようこそ。

## 変更履歴

**v1.0 **@2022-01-20

> 1. 初版
