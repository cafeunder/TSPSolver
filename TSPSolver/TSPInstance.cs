
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace TSPSolver {
	public class TSPInstance {
		// 次元数
		public int Dimension { get; private set; }
		// 各都市の座標
		private double[][] Coordinate { get; set; }

		public TSPInstance(string filepath) {
			using (StreamReader sr = new StreamReader(File.OpenRead(filepath))) {
				bool read_header = true;
				Regex delimiter = new Regex(@"\s*:\s*");

				// ヘッダー読み込み
				while (read_header && !sr.EndOfStream) {
					string line = sr.ReadLine();
					string[] record = delimiter.Split(line);

					switch (record[0]) {
					case "DIMENSION":
						this.Dimension = int.Parse(record[1]);
						break;
					case "NODE_COORD_SECTION":
						read_header = false;
						break;
					}
				}

				if (this.Dimension == 0) {
					throw new System.Exception("入力されたインスタンスファイルに次元数の指定がありません．");
				}
				this.Coordinate = new double[this.Dimension][];

				delimiter = new Regex(@"\s+");
				// 座標読み込み
				for (int i = 0; i < this.Dimension && !sr.EndOfStream; i++) {
					string line = sr.ReadLine();
					string[] record = delimiter.Split(line);

					this.Coordinate[i] = new double[2];
					this.Coordinate[i][0] = double.Parse(record[1]);
					this.Coordinate[i][1] = double.Parse(record[2]);
				}
			}
		}

		public int CalcDistance(int v0, int v1) {
			double dx = this.Coordinate[v0][0] - this.Coordinate[v1][0];
			double dy = this.Coordinate[v0][1] - this.Coordinate[v1][1];

			return (int)(Math.Sqrt(dx * dx + dy * dy) + 0.5);
		}
	}
}
