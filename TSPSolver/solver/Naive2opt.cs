using System;
using TSPSolver.common;
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

			// ランダムなノードsから始めて全てのノードに接続しているエッジを検索する
			int si = SRandom.Instance.NextInt(instance.Dimension);
			for (int i = 0; i < instance.Dimension; i++) {
				int v = (si + i) % instance.Dimension;
				int vn = tour.NextID(v);

				// ランダムなノードsから始めて全てのノードに接続しているエッジを検索する
				int sj = SRandom.Instance.NextInt(instance.Dimension);
				for (int j = 0; j < instance.Dimension; j++) {
					int w = (sj + j) % instance.Dimension;
					if (w == v) { continue; }
					int wn = tour.NextID(w);

					// (v, vn)と(w, wn)を削除する
					int remove_gain = instance.CalcDistance(v, vn) + instance.CalcDistance(w, wn);
					// (v, w)と(vn, wn)を追加する
					int add_gain = instance.CalcDistance(v, w) + instance.CalcDistance(vn, wn);

					if (add_gain < remove_gain) {
						tour.Flip(v, vn, w, wn);
#if DEBUG
						length += add_gain - remove_gain;
						Console.WriteLine(length + ", " + instance.CalcTourLength(tour.NodeArray));
#endif
						// 最初からやりなおす
						i = 0;
						si = SRandom.Instance.NextInt(instance.Dimension);
						goto FINISH;
					}
				}
			FINISH:;
			}

			return tour.NodeArray;
		}
	}
}
