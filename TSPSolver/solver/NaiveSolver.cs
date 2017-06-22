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

			// 間違えてる！！！スタックだと重複でプッシュされる恐れあり
			// 検索順スタックを生成
			Stack<int> nodeStack = new Stack<int>(instance.Dimension);
			foreach (int node in tour.NodeArray) {
				nodeStack.Push(node);
			}

			// 全てのエッジが改善不可能になるまで続ける
			while (nodeStack.Count != 0) {
				int v = nodeStack.Pop();
				int vn = tour.nextID(v);

				// ランダムなノードsから始めて全てのノードに接続しているエッジを検索する
				int s = SRandom.Instance.NextInt(instance.Dimension);
				for (int i = 0; i < instance.Dimension; i++) {
					int w = (s + i) % instance.Dimension;
					if (w == v) { continue; }

					int wn = tour.nextID(w);
					// (v, vn)と(w, wn)を削除する
					int remove_gain = this.distTable[v][vn] + this.distTable[w][wn];
					// (v, w)と(vn, wn)を追加する
					int add_gain = this.distTable[v][w] + this.distTable[vn][wn];

					if (add_gain < remove_gain) {
						tour.flip(v, w, true);
					}
				}
			}
		}
	}
}
