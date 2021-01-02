using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using VisualPinball.Engine.VPT;
using VisualPinball.Engine.VPT.Table;

namespace VisualPinball.MaterialPatcher
{
	class Program
	{
		static void Main(string[] args)
		{
			// validate inputs
			try {
				if (args.Length != 3) {
					throw new ArgumentException("USAGE: VisualPinball.MaterialPatcher.exe <.mat file> <input file or folder> <output folder>");
				}

				// material file
				var materialFile = args[0];
				if (!File.Exists(materialFile)) {
					throw new ArgumentException($"Cannot find material file at \"{materialFile}\"");
				}

				// input file(s)
				if (!FileOrDirectoryExists(args[1])) {
					throw new ArgumentException($"Non-existent input path: \"{args[1]}\".");
				}
				var inputAttr = File.GetAttributes(args[1]);
				var inputFiles = (inputAttr & FileAttributes.Directory) == FileAttributes.Directory
					? Directory.GetFiles(args[1], "*.vpx")
					: new[] {args[1]};
				if (inputFiles.Length == 0) {
					throw new ArgumentException($"No .vpx files found at \"{args[1]}\"");
				}

				// output folder
				var outputDir = args[2];
				if (!Directory.Exists(outputDir)) {
					throw new ArgumentException($"Invalid output folder \"{outputDir}\".");
				}

				// read materials
				var inputMaterials = MaterialReader.Load(materialFile).ToArray();
				Console.WriteLine($"Looking for materials: [ {string.Join(", ", inputMaterials.Select(m => m.Name))} ]");


				// apply materials to each table
				var stopWatch = new Stopwatch();
				foreach (var inputFile in inputFiles) {
					stopWatch.Start();
					var outputFilePath = Path.Join(outputDir, Path.GetFileName(inputFile));
					Console.WriteLine($"Processing \"{inputFile}\"...");

					var inputTable = TableLoader.Load(inputFile);
					var numMaterials = 0;
					foreach (var inputMaterial in inputMaterials) {
						var outputMaterial = inputTable.GetMaterial(inputMaterial.Name);
						if (outputMaterial != null) {
							outputMaterial.BaseColor = inputMaterial.BaseColor;
							outputMaterial.ClearCoat = inputMaterial.ClearCoat;
							outputMaterial.EdgeAlpha = inputMaterial.EdgeAlpha;
							outputMaterial.Elasticity = inputMaterial.Elasticity;
							outputMaterial.ElasticityFalloff = inputMaterial.ElasticityFalloff;
							outputMaterial.Friction = inputMaterial.Friction;
							outputMaterial.Glossiness = inputMaterial.Glossiness;
							outputMaterial.GlossyImageLerp = inputMaterial.GlossyImageLerp;
							outputMaterial.IsMetal = inputMaterial.IsMetal;
							outputMaterial.IsOpacityActive = inputMaterial.IsOpacityActive;
							outputMaterial.Opacity = inputMaterial.Opacity;
							outputMaterial.Roughness = inputMaterial.Roughness;
							outputMaterial.ScatterAngle = inputMaterial.ScatterAngle;
							outputMaterial.Thickness = inputMaterial.Thickness;
							outputMaterial.WrapLighting = inputMaterial.WrapLighting;
							outputMaterial.UpdateData();
							numMaterials++;
						}
					}

					new TableWriter(inputTable).WriteTable(outputFilePath);
					Console.WriteLine($"Written to \"{outputFilePath}\" with {numMaterials} updated materials in {stopWatch.ElapsedMilliseconds}ms.");

					stopWatch.Stop();
					stopWatch.Reset();
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
