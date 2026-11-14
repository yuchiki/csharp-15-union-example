.PHONY: default build run clean

EXPR ?= 3 * 2 + 20 / 5

default: run

build:
	dotnet build

run:
	dotnet run --project csharp-15-adt.csproj -- "$(EXPR)"

clean:
	dotnet clean
