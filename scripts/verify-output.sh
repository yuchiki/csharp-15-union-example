#!/usr/bin/env bash

set -euo pipefail

readonly expected='10'
actual="$(dotnet run --project csharp-15-adt.csproj --configuration Release --no-build -- '3 * 2 + 20 / 5')"
readonly actual

if [[ "$actual" != "$expected" ]]; then
  printf 'Expected:\n%s\n\nActual:\n%s\n' "$expected" "$actual"
  exit 1
fi

printf '%s\n' "$actual"
