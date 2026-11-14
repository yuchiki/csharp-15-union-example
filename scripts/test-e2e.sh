#!/usr/bin/env bash

set -euo pipefail

test_dir="$(mktemp -d)"
readonly test_dir
trap 'rm -rf "$test_dir"' EXIT

check() {
  local input="$1" expected_output="$2" expected_error="$3" expected_status="$4"
  local status=0
  printf '%s' "$input" | dotnet run --project csharp-15-adt.csproj --configuration Release --no-build \
    >"$test_dir/stdout" 2>"$test_dir/stderr" || status=$?

  printf '%s' "$expected_output" >"$test_dir/expected-stdout"
  printf '%s' "$expected_error" >"$test_dir/expected-stderr"
  diff -u "$test_dir/expected-stdout" "$test_dir/stdout"
  diff -u "$test_dir/expected-stderr" "$test_dir/stderr"
  if [[ "$status" != "$expected_status" ]]; then
    printf 'Expected exit status %s, got %s\n' "$expected_status" "$status" >&2
    return 1
  fi
}

check '3 * 2 + 20 / 5' $'Add(Mul(CInt(3), CInt(2)), Div(CInt(20), CInt(5)))\n' '' 0
check $'(2 +\n3) * 4\n' $'Mul(Add(CInt(2), CInt(3)), CInt(4))\n' '' 0
check '1 / 0' $'Div(CInt(1), CInt(0))\n' '' 0
check '1 +' '' $'Expected an integer or \'(\' at position 3.\n' 1
check '' '' $'Expected an integer or \'(\' at position 0.\n' 1

printf 'All E2E tests passed.\n'
