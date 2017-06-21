using System.Collections.Generic;
using TSPSolver.structure;

namespace TSPSolver.solver {
	public class NaiveSolver {
		/// <summary>
		/// ノード間の距離テーブル
		/// </summary>
		private int[][] distTable;

		public void Solve(TSPInstance instance) {
			Tour tour = new Tour(instance.Dimension);

			// 距離をあらかじめ計算しテーブルとして保持
			this.distTable = new int[instance.Dimension][];
			for (int i = 0; i < instance.Dimension; i++) {
				this.distTable[i] = new int[instance.Dimension];
				for (int j = 0; j < instance.Dimension; j++) {
					this.distTable[i][j] = instance.CalcDistance(i, j);
				}
			}

			// 検索順スタックを生成
			Stack<int> nodeStack = new Stack<int>(instance.Dimension);
			foreach (int node in tour.NodeArray) {
				nodeStack.Push(node);
			}

			// 全てのエッジが改善不可能になるまで続ける
			while (nodeStack.Count != 0) {
				int v = nodeStack.Pop();

				// ランダムなノードsから始めて全てのノードに接続しているエッジを検索する
				int s = SRandom.Instance.NextInt(instance.Dimension);
				for (int i = 0; i < instance.Dimension; i++) {
					int w = (s + i) % instance.Dimension;

					// (v, v+1)と(w, w+1)を削除する
					int remove_gain = this.distTable[v][tour.nextID(v)] + this.distTable[w][tour.nextID(w)];
					// (v, w)と(v+1, w+1)を追加する
					int add_gain = this.distTable[v+1][tour.nextID(v+1)] + this.distTable[w+1][tour.nextID(w+1)];

					if (add_gain < remove_gain) {

					}
				}
			}
		}
	}
}
