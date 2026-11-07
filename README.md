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

各ケースから `Expr` への暗黙変換があり、コンパイラーは union の全ケースを把握します。そのため、全ケースを処理する `switch` 式に `_` は不要です。ケースを追加したのに `switch` へ追加し忘れた場合もコンパイラーが検出します。

各ケースを `sealed record` にしているのは、`CInt` や `Add` の派生型によってケースの内側へ別の選択肢が追加されることを防ぎ、ADT の選択肢を `Expr` に列挙した5種類へ閉じるためです。公式資料の例のように `sealed` を省略しても union としては動作しますが、このサンプルでは閉じた式の種類を表す意図を優先しています。

詳しくは Microsoft の公式資料を参照してください。

- [C# 15 の新機能](https://learn.microsoft.com/dotnet/csharp/whats-new/)
- [Union 型 (C# リファレンス)](https://learn.microsoft.com/dotnet/csharp/language-reference/builtin-types/union)
- [C# union 型のチュートリアル](https://learn.microsoft.com/dotnet/csharp/whats-new/tutorials/unions)

## 実行方法

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
