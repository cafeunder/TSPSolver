using System;
using TSPSolver.structure;
using TSPSolver.common;
using TSPSolver.solver.util;

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
			
			// 全てのエッジが改善不可能になるまで続ける
			int si = SRandom.Instance.NextInt(instance.Dimension);
			for (int i = 0; i < instance.Dimension; i++) {
				int v = (si + i) % instance.Dimension;

				// 正順と逆順のエッジを確かめる
				for (int d = 0; d < 2; d++) {
					bool forward = (d == 0);
					int vn = (forward) ? tour.NextID(v) : tour.PrevID(v);
					int dist = instance.CalcDistance(v, vn);

					// 最近傍のノードから見ていく
					for (int j = 0; j < this.neighborList.NeighborNum; j++) {
						int w = this.neighborList.NeighborNodes[v][j];
						// v - vnよりも v - wの方が長いなら終了
						if (this.neighborList.DistanceTable[v][j] >= dist) {
							break;
						}

						int wn = (forward) ? tour.NextID(w) : tour.PrevID(w);
						// (v, vn)と(w, wn)を削除する
						int remove_gain = dist + instance.CalcDistance(w, wn);
						// (v, w)と(vn, wn)を追加する
						int add_gain = this.neighborList.DistanceTable[v][j] + instance.CalcDistance(vn, wn);

						if (add_gain < remove_gain) {
							tour.Flip(v, w, forward);
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
				}
			FINISH:;
			}

			return tour.NodeArray;
		}
	}
}
