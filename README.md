# csharp-nine-adt

私か書いたQiita記事 [C# 9.0でHaskellの代数的データ型(ADT)的な書き方をする](https://qiita.com/yuchiki1000yen/items/b61e9ad6f2c3716635f4)で説明に用いたプログラムです。

## 前提条件

- [mise](https://mise.jdx.dev/) がインストールされていること

### mise のインストール

mise がまだインストールされていない場合は、公式ドキュメントを参照してください。

https://mise.jdx.dev/getting-started.html

## セットアップ

リポジトリをクローンした後、以下のコマンドで必要な dotnet SDK をインストールします。

```sh
mise install
```

これにより、`.mise.toml` に定義された dotnet SDK バージョン (10.0.201) が自動的にインストールされます。

## 実行方法

以下のコマンドを実行する。

```sh
make
```
