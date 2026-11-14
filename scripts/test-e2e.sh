#!/usr/bin/env bash

set -euo pipefail

test_dir="$(mktemp -d)"
readonly test_dir
trap 'rm -rf "$test_dir"' EXIT

check() {
  local expected_output="$1" expected_error="$2" expected_status="$3"
  local status=0
  shift 3
  dotnet run --project csharp-15-adt.csproj --configuration Release --no-build -- "$@" \
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

check $'10\n' '' 0 '3 * 2 + 20 / 5'
check $'20\n' '' 0 $'(2 +\n3) * 4\n'
check '' $'Division by zero.\n' 1 '1 / 0'
check $'14\n' '' 0 '2 + 3 * 4'
check '' $'Pass the expression as one quoted argument.\n' 1 '1' '+' '2'
check '' $'Expected an integer or \'(\' at position 3.\n' 1 '1 +'
check '' $'Expected an integer or \'(\' at position 0.\n' 1 ''
check '' $'Pass the expression as one quoted argument.\n' 1

printf 'All E2E tests passed.\n'
