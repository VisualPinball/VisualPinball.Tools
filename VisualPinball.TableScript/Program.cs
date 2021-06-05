using System;
using System.IO;
using VisualPinball.Engine.VPT.Table;

namespace VisualPinball.TableScript
{
	class Program
	{
		static void Main(string[] args)
		{
			// validate inputs
			try {
				if (args.Length < 1) {
					throw new ArgumentException("USAGE: VisualPinball.TableScript <input file or folder> [<output folder>]");
				}

				// input file(s)
				if (!FileOrDirectoryExists(args[0])) {
					throw new ArgumentException($"Non-existent input path: \"{args[0]}\".");
				}
				var inputAttr = File.GetAttributes(args[0]);
				var inputFiles = (inputAttr & FileAttributes.Directory) == FileAttributes.Directory
					? Directory.GetFiles(args[0], "*.vpx")
					: new[] {args[0]};
				if (inputFiles.Length == 0) {
					throw new ArgumentException($"No .vpx files found at \"{args[0]}\"");
				}

				// output folder
				string outputDir = null;

				if (args.Length > 1) {
					outputDir = args[1];
					if (!Directory.Exists(outputDir)) {
						throw new ArgumentException($"Invalid output folder \"{outputDir}\".");
					}
				}

				foreach (var inputFile in inputFiles) {
					Console.WriteLine($"Processing \"{inputFile}\"...");

					var inputTable = TableLoader.Load(inputFile, false);

					var code = inputTable.Data.Code;

					if (Environment.NewLine == "\n") {
						code = code.Replace("\r\r\n", "\n");
						code = code.Replace("\r\n", "\n");
						code = code.Replace("\r", "\n");
					}

					if (outputDir != null) {
						var outputFilePath = Path.Join(outputDir, Path.GetFileNameWithoutExtension(inputFile) + ".vbs");

						File.WriteAllText(outputFilePath, code);
					}
					else {
						Console.WriteLine(code);
                    }
				}

			} catch (ArgumentException e) {
				Console.WriteLine(e.Message);

			} catch (Exception e) {
				Console.WriteLine(e);
			}
		}

		private static bool FileOrDirectoryExists(string name)
		{
			return Directory.Exists(name) || File.Exists(name);
		}
	}
}
