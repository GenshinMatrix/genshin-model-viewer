・[English](README.en.md) ・[中文](README.md) ・[日本語](README.jp.md)

  # <img src="src/GenshinModelViewer/Resources/UI_AvatarIcon_Side_Yunjin.png" width = "56" height = "56" alt="" align="left" /> 原神モデルビューア

他のPMX形式DMMモデルのプレビューもサポートされています。

  > 目玉：任意回転移動可！モデル内部観察可！

  ## モデルのフォーマット

  > サポートされているモデルのフォーマット： `*.pmx` `*.7z` `*.zip` 

※アーカイブタイプのファイルは自動的に非解凍方式を最初の`*.pmx`の方を導入する。

  > サポートされているテクスチャのフォーマット：`*.png` `*.bmp` `*.dds` `*.tga` など。

[モデルのダウンロード](https://www.aplaybox.com/u/680828836)

  ## 使い方

![demo2](screen-shot/demo-01.png)

![demo1](screen-shot/demo-02.png)

> モデルファイルを選択する、又はウィンドウにドラッグ&ドロップして始まる。

| ソースインプット       | 效果                        |
| ---------------------- | --------------------------- |
| マウスの左ボタン       | トランスレーション          |
| マウスの真ん中のボタン | トランスレーション & ズーム |
| マウスの右ボタン       | 回転                        |

  ## 実行環境

`Win10 x64` `.NET 6`

  ## ダウンロード
  [![GitHub downloads](https://img.shields.io/github/downloads/emako/genshin-model-viewer/total)](https://github.com/emako/genshin-model-viewer/releases)
  [![GitHub downloads](https://img.shields.io/github/downloads/emako/genshin-model-viewer/latest/total)](https://github.com/emako/genshin-model-viewer/releases)

  > ダウンロードページ：https://github.com/emako/genshin-model-viewer/releases
  >

  ## タスク

- [ ] インポートフォルダのサポート。


- [ ] フォルダまたはアーカイブのインポート時に複数のpmxファイルの中から１つ選択できる機能。
- [ ] フルスクリーンモードとWindowのタイトルバーのリペイントをサポート。
- [ ] 更なるUIボタンの追加。

## よくある質問

> モデルのとある部分のテクスチャのレンダリング異常？

- Sphereアルゴリズムが組み込まれたモデルはサポートされていません。この場合はモデルのテクスチャ異常として表現されますが、現在原神公式リリースされているDMMモデルではこのような方法は使用されていないので、原神のモデルにはこのような問題はないはずです。

---

[Issues](https://github.com/emako/genshin-model-viewer/issues) へようこそ。

## 変更履歴

[CHANGELOG.md](CHANGELOG.md)

## 依存ライブラリ

- [HelixToolkit](https://github.com/helix-toolkit/helix-toolkit)
- [QuickLook.Plugin.HelixViewer](https://github.com/ShiinaManatsu/QuickLook.Plugin.HelixViewer)

- [Pfim](https://github.com/nickbabcock/Pfim)

- [SevenZipSharp](https://github.com/squid-box/SevenZipSharp)
- [7-Zip](https://www.7-zip.org/)

## ライセンス

[LICENSE](LICENSE)

