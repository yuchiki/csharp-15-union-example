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

標準入力全体、または1つのコマンドライン引数を算術式としてパースし、計算結果を標準出力へ返します。引数を指定した場合は標準入力より優先します。パーサーは整数（0〜2147483647）、四則演算、括弧、単項の `+` / `-`、空白に対応します。乗除算は加減算より優先され、同じ優先順位の二項演算は左結合です。除算は整数除算です。不正入力・ゼロ除算・Int32の範囲を超える計算は標準エラーへ報告し、終了コード1を返します。成功時の終了コードは0です。

- `Data/`: union とケースのデータ定義
- `Processing/`: パーサー、評価、文字列整形
- `Program.cs`: 標準入出力を使うエントリーポイント

入力は改行・空白も含め1024文字まで、括弧と単項符号による再帰の深さは64までです。上限超過も入力エラーとして報告します。

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
printf '3 * 2 + 20 / 5\n' | make
```

または次のコマンドで直接実行できます。

```console
printf '3 * 2 + 20 / 5\n' | dotnet run --project csharp-15-adt.csproj
```

```text
10
```

手入力する場合は式を入力した後、EOF（macOS/Linux では Ctrl+D）を送信してください。

引数として直接渡す場合は、シェルが演算子を展開しないよう式全体を引用符で囲みます。

```console
dotnet run --project csharp-15-adt.csproj -- '3 * 2 + 20 / 5'
```

### テスト

```console
mise exec -- dotnet test tests/CSharp15Adt.Tests/CSharp15Adt.Tests.csproj -c Release
mise exec -- bash scripts/test-e2e.sh
```

単体テストではパースと入出力処理を検証し、E2E テストでは実際のプロセスへ標準入力を渡して標準出力・標準エラー・終了コードを比較します。
