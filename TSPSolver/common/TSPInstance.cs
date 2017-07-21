
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace TSPSolver.common {
	/// <summary>
	/// TSPのインスタンスを表すクラス
	/// </summary>
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
				this.Coordinate = new double[2][];
				for (int i = 0; i < this.Coordinate.Length; i++) {
					this.Coordinate[i] = new double[this.Dimension];
				}

				delimiter = new Regex(@"\s+");
				// 座標読み込み
				for (int i = 0; i < this.Dimension && !sr.EndOfStream; i++) {
					string line = sr.ReadLine();
					string[] record = delimiter.Split(line);

					this.Coordinate[0][i] = double.Parse(record[1]);
					this.Coordinate[1][i] = double.Parse(record[2]);
				}
			}
		}

		public int CalcDistance(int v0, int v1) {
			double dx = this.Coordinate[0][v0] - this.Coordinate[0][v1];
			double dy = this.Coordinate[1][v0] - this.Coordinate[1][v1];

			return (int)(Math.Sqrt(dx * dx + dy * dy) + 0.5);
		}

		public int CalcTourLength(int[] tour) {
			int result = 0;

			// d(0,1) + d(1,2) + d(2,3) + ... + d(n-2, n-1)
			for (int i = 0; i < tour.Length - 1; i++) {
				result += this.CalcDistance(tour[i], tour[i + 1]);
			}

			// + d(n-1, 0)
			result += this.CalcDistance(tour[tour.Length - 1], tour[0]);
			return result;
		}
	}
}
