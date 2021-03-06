using System;
using System.Collections.Generic;
using System.Text;
using TSPSolver.solver.util;
using TSPSolver.common;

namespace TSPSolver {
	/// <summary>
	/// Bechmarkerで使う近傍ファイルを作成するクラス
	/// </summary>
	public class NeighborListFileMaker {
		public static void Main(string[] args) {
			foreach (string instanceName in Benchmarker.INSTANCES) {
				TSPInstance instance = new TSPInstance(@"data/" + instanceName + ".tsp");
				NeighborList neighbor_list = new NeighborList(instance, 50);
				InverseNeighborList inv_neighbor_list = new InverseNeighborList(neighbor_list);
				neighbor_list.WriteTo(@"data/" + instanceName + ".neighbor");
			}
		}
	}
}
