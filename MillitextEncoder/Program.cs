using MillitextEncoder.Properties;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MillitextEncoder {
	class Program {
		static void Main(string[] args) {

			string[] lines = GetInput();
			int pixelHeight = lines.Length * 6 - 1;

			int pixelWidth = lines.Max(s => s.Length * 2);

			string[] font = Resources.font.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

			Console.WriteLine(pixelHeight + " " + pixelWidth);

			using (Image<Rgba32> image = new Image<Rgba32>(Configuration.Default, pixelWidth, pixelHeight, Rgba32.Black)) {
				int xoff = 0, yoff = 0;

				for (int line = 0; line < lines.Length; line++) { // lines
					for (int character = 0; character < lines[line].Length; character++) { // characters
						string charEncode = font[lines[line][character]];
						for (int i = 0; i < 10; i++) {
							int r = ((charEncode[i] - '0') & 0b100) == 0 ? 0 : 255;
							int g = ((charEncode[i] - '0') & 0b010) == 0 ? 0 : 255;
							int b = ((charEncode[i] - '0') & 0b001) == 0 ? 0 : 255;

							image[xoff + i % 2, yoff + i / 2] = new Rgba32((byte)r, (byte)g, (byte)b);
						}
						xoff += 2;
					}
					xoff = 0;
					yoff += 6;
				}

				Console.WriteLine("Enter a file name: (the file will be saved as a PNG)");
				string filename = Console.ReadLine();
				image.Save($"{filename}.png");
			}
		}

		static string[] GetInput() {
			string line;
			List<string> lines = new List<string>();

			Console.WriteLine("Enter lines of ASCII text to encode, submit an empty line to finish:");
			while (!string.IsNullOrWhiteSpace(line = Console.ReadLine())) {
				lines.Add(line);
			}
			return lines.ToArray();
		}
	}
}
