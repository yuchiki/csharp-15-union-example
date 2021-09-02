.PHONY: default build run clean

default: run

build:
	dotnet build

run: build
	dotnet bin/Debug/net5.0/csharp-nine-adt.dll

clean:
	rm -rf bin obj
