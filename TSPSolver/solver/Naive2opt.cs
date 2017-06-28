using System;
using TSPSolver.structure;

namespace TSPSolver.solver {
	/*
	 * 近傍リストなし、DontLookBitなし
	 */
	public class Naive2opt : Solver {
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

				// ランダムなノードsから始めて全てのノードに接続しているエッジを検索する
				int s = SRandom.Instance.NextInt(instance.Dimension);
				for (int i = 0; i < instance.Dimension; i++) {
					int w = (s + i) % instance.Dimension;
					if (w == v) { continue; }

					int wn = tour.nextID(w);
					// (v, vn)と(w, wn)を削除する
					int remove_gain = instance.CalcDistance(v, vn) + instance.CalcDistance(w, wn);
					// (v, w)と(vn, wn)を追加する
					int add_gain = instance.CalcDistance(v, w) + instance.CalcDistance(vn, wn);

					if (add_gain < remove_gain) {
						tour.flip(v, w, true);
						selectNodeList.Add(w);
#if DEBUG
						length += add_gain - remove_gain;
						Console.WriteLine(length);
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
