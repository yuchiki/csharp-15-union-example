using System;
namespace csharp_nine_adt
{
    class Program
    {
        static void Main(string[] args)
        {
            // 3 * 2 + 20 / 5 = 10
            Console.WriteLine(calc(myExpr1));

            // いい感じの文字列化がなされる
            Console.WriteLine(myExpr1);

            // (参照ではなくて)値に基づいて等価性比較がなされるので True
            Console.WriteLine(myExpr1 == myExpr2);
        }

        static Expr myExpr1 =
            new Add(
                new Mul(
                    new CInt(3),
                    new CInt(2)),
                new Div(
                    new CInt(20),
                    new CInt(5)));

        static Expr myExpr2 =
            new Add(
                new Mul(
                    new CInt(3),
                    new CInt(2)),
                new Div(
                    new CInt(20),
                    new CInt(5)));

        static int calc(Expr expr) => expr switch
        {
            CInt(var i) => i,
            Add(var l, var r) => calc(l) + calc(r),
            Sub(var l, var r) => calc(l) - calc(r),
            Mul(var l, var r) => calc(l) * calc(r),
            Div(var l, var r) => calc(l) / calc(r),
            _ => throw new InvalidOperationException("unreachable")
        };


        abstract record Expr();
        sealed record CInt(int value) : Expr;
        sealed record Add(Expr left, Expr right) : Expr;
        sealed record Sub(Expr left, Expr right) : Expr;
        sealed record Mul(Expr left, Expr right) : Expr;
        sealed record Div(Expr left, Expr right) : Expr;
    }
}
