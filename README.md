# C# 15 で ADT

[C# 9.0でHaskellの代数的データ型(ADT)的な書き方をする](https://qiita.com/yuchiki1000yen/items/b61e9ad6f2c3716635f4)で使ったサンプルを、C# 15 の union 型で書き直したものです。

## C# 9 版からの変更点

C# 9 版では抽象 `record Expr` と継承で表現していました。C# 15 では各ケースを通常の `record` として宣言し、union 型へまとめられます。

```csharp
internal sealed record CInt(int Value);
internal sealed record Add(Expr Left, Expr Right);
internal sealed record Sub(Expr Left, Expr Right);
internal sealed record Mul(Expr Left, Expr Right);
internal sealed record Div(Expr Left, Expr Right);

internal union Expr(CInt, Add, Sub, Mul, Div);
```

## 実行方法

`Program.cs` では `Parser.Parse("3 * 2 + 20 / 5")` で式木を作り、評価と文字列整形を行います。パーサーは整数（0〜2147483647）、四則演算、括弧、単項の `+` / `-`、空白に対応します。乗除算は加減算より優先され、同じ優先順位の二項演算は左結合です。負号は `Sub(CInt(0), ...)` として表現します。不正な入力は位置付きの `FormatException` になります。

- `Data/`: union とケースのデータ定義
- `Processing/`: パーサー、評価、文字列整形
- `Program.cs`: 文字列のパースから始める使用例

### 前提

- [mise](https://mise.jdx.dev/getting-started.html)

`.mise.toml` で .NET 11 SDK のバージョンを固定しています。mise をインストールしたあと、このリポジトリで次を実行してください。

```console
mise trust
mise install
dotnet --version
```

`11.0.100-rc.3.26522.148` と表示されれば準備完了です。これは現時点で最新の .NET 11 SDK です。`net11.0` を対象にすると C# 15 が既定で選択されるため、`LangVersion` の指定は不要です。

### ビルドと実行

```console
make
```

または次のコマンドで直接実行できます。

```console
dotnet run --project csharp-15-adt.csproj
```

```text
10
Add(Mul(CInt(3), CInt(2)), Div(CInt(20), CInt(5)))
```
