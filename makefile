.PHONY: default build run clean

default: run

build:
	dotnet build

run:
	dotnet run --project csharp-15-adt.csproj

clean:
	dotnet clean
