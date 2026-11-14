.PHONY: default build run clean

EXPR ?= 3 * 2 + 20 / 5

default: run

build:
	mise exec -- dotnet build

run:
	mise exec -- dotnet run --project csharp-15-adt.csproj -- "$(EXPR)"

clean:
	mise exec -- dotnet clean
