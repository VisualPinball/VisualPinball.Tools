# Table Script

This is a command line tool that simply:

- Reads all `.vpx` files in a folder (or a single file)
- Optionally takes in an output folder

It then:

- Extracts all input table scripts
- Converts CR/LF to NL on Unix systems 
- Saves the table scripts to the output folder if specified, otherwise outputs to the console.

## Usage

Usage is pretty simple:

```bash
VisualPinball.TableScript <.vpx or input folder> [<output folder>]
```

## Compilation

To get a single binary on Windows:

```bash
dotnet publish -r win-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true
```

To get a single binary on MacOS:

```bash
dotnet publish -r osx-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true
```


To get a single binary on Linux:

```bash
dotnet publish -r linux-x64 -c Release /p:PublishSingleFile=true /p:PublishTrimmed=true
```
