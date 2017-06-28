using System;
using TSPSolver.neighborList;
using TSPSolver.structure;

namespace TSPSolver.solver {
	/*
	 * 近傍リストあり、DontLookBitなし
	 */
	public class Neighbor2opt : Solver {
		// 近傍リスト
		NeighborList neighborList;

		public Neighbor2opt(NeighborList neighborList) {
			this.neighborList = neighborList;
		}

		override public int[] Run(TSPInstance instance) {
			// ツアーを生成
			Tour tour = new Tour(instance.Dimension);
#if DEBUG
			int length = instance.CalcTourLength(tour.NodeArray);
#endif

			// 選択候補リストを生成
			SelectNodeList selectNodeList = new SelectNodeList(instance.Dimension);

			// 全てのエッジが改善不可能になるまで続ける
			while (selectNodeList.Size != 0) {
				int v = selectNodeList.GetRand();
				int vn = tour.nextID(v);
				int dist = instance.CalcDistance(v, vn);

				// ランダムなノードsから始めて全てのノードに接続しているエッジを検索する
				for (int i = 0; i < this.neighborList.NeighborNum; i++) {
					int w = this.neighborList.NeighborNodes[v][i];
					// v - vnよりも v - wの方が長いなら終了
					if (this.neighborList.DistanceTable[v][i] >= dist) {
						break;
					}

					int wn = tour.nextID(w);
					// (v, vn)と(w, wn)を削除する
					int remove_gain = dist + instance.CalcDistance(w, wn);
					// (v, w)と(vn, wn)を追加する
					int add_gain = this.neighborList.DistanceTable[v][i] + instance.CalcDistance(vn, wn);

					if (add_gain < remove_gain) {
						tour.flip(v, w, true);
						selectNodeList.Add(w);
#if DEBUG
						length += add_gain - remove_gain;
						Console.WriteLine(length + ", " + instance.CalcTourLength(tour.NodeArray));
#endif
						goto SUCCESS;
					}
				}
				// 改善失敗
				selectNodeList.Remove(v);
			SUCCESS:;
			}

			return tour.NodeArray;
		}
	}
}
