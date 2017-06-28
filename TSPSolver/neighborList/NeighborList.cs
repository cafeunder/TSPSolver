using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TSPSolver.neighborList {
	public class NeighborList {
		private int dimension;
		public int NeighborNum { get; private set; }
		public int[][] NeighborNodes { get; private set; }
		public int[][] DistanceTable { get; private set; }

		public NeighborList(TSPInstance instance, int neighborNum) {
			this.dimension = instance.Dimension;
			this.NeighborNodes = new int[instance.Dimension][];
			this.DistanceTable = new int[instance.Dimension][];
			this.NeighborNum = (instance.Dimension < neighborNum) ? instance.Dimension : neighborNum;

			for (int i = 0; i < instance.Dimension; i++) {
				this.NeighborNodes[i] = new int[this.NeighborNum];
				this.DistanceTable[i] = new int[this.NeighborNum];
				this.DistanceTable[i][0] = int.MaxValue;
				int count = 0;
			
				for (int j = 0; j < instance.Dimension; j++) {
					if (i == j) { continue; }
					int distance = instance.CalcDistance(i, j);

					// 配列の末尾 = 最も距離が遠いノードと距離を比較
					if (distance < this.DistanceTable[i][count]) {
						// 配列がいっぱいでないなら末尾に挿入、いっぱいなら末尾を上書き
						int insert = (count < this.NeighborNum - 1) ? count + 1 : count;
						this.NeighborNodes[i][insert] = j;
						this.DistanceTable[i][insert] = distance;

						// 所定の位置までずらす
						for (int k = insert; k > 0; k--) {
							// いっこ前の要素が自分よりも小さければ そこが所定の位置
							if (this.DistanceTable[i][k - 1] < this.DistanceTable[i][k]) {
								break;
							}

							// 近傍配列のスワップ
							int temp = this.NeighborNodes[i][k - 1];
							this.NeighborNodes[i][k - 1] = this.NeighborNodes[i][k];
							this.NeighborNodes[i][k] = temp;

							// 距離テーブルのスワップ
							temp = this.DistanceTable[i][k - 1];
							this.DistanceTable[i][k - 1] = this.DistanceTable[i][k];
							this.DistanceTable[i][k] = temp;
						}

						if (count < this.NeighborNum - 1) {
							count++;
						}
					}
				}				
			}
		}

		public void WriteTo(string filepath) {
			using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(filepath))) {
				bw.Write(this.dimension);
				bw.Write(this.NeighborNum);
				for (int i = 0; i < this.NeighborNodes.Length; i++) {
					for (int j = 0; j < this.NeighborNodes[i].Length; j++) {
						bw.Write(this.NeighborNodes[i][j]);
						bw.Write(this.DistanceTable[i][j]);
					}
				}
			}
		}

		public void ReadFrom(string filepath) {
			using (BinaryReader br = new BinaryReader(File.OpenRead(filepath))) {
				this.dimension = br.ReadInt32();
				this.NeighborNum = br.ReadInt32();
				this.NeighborNodes = new int[this.dimension][];
				this.DistanceTable = new int[this.dimension][];

				for (int i = 0; i < this.NeighborNodes.Length; i++) {
					this.NeighborNodes[i] = new int[this.NeighborNum];
					this.DistanceTable[i] = new int[this.NeighborNum];

					for (int j = 0; j < this.NeighborNodes[i].Length; j++) {
						this.NeighborNodes[i][j] = br.ReadInt32();
						this.DistanceTable[i][j] = br.ReadInt32();
					}
				}
			}
		}
	}
}
