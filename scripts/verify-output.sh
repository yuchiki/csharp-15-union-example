#!/usr/bin/env bash

set -euo pipefail

readonly expected=$'10\nAdd(Mul(CInt(3), CInt(2)), Div(CInt(20), CInt(5)))'
actual="$(dotnet run --project csharp-15-adt.csproj --configuration Release --no-build)"
readonly actual

if [[ "$actual" != "$expected" ]]; then
  printf 'Expected:\n%s\n\nActual:\n%s\n' "$expected" "$actual"
  exit 1
fi

printf '%s\n' "$actual"
