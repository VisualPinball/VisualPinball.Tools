# Material Patcher

This is a command line tool that simply:

- Reads a `.mat` file, which is a collection of materials exported from Visual Pinball
- Reads all `.vpx` files in a folder (or a single file)
- Takes in an output folder

It then:

- Applies all material properties to the materials of all input tables with the same name
- Saves the update table to the output folder

## Usage

Usage is pretty simple:

```bash
VisualPinball.MaterialPatcher.exe <path to .mat> <.vpx or input folder> <output folder>
```