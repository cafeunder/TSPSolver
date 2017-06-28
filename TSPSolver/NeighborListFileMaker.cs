using System;
using System.Collections.Generic;
using System.Text;
using TSPSolver.neighborList;

namespace TSPSolver {
	/// <summary>
	/// Bechmarker�Ŏg���ߖT�t�@�C�����쐬����N���X
	/// </summary>
	public class NeighborListFileMaker {
		public static void Main(string[] args) {
			foreach (string instanceName in Benchmarker.INSTANCES) {
				TSPInstance instance = new TSPInstance(@"data/" + instanceName + ".tsp");
				NeighborList neighborList = new NeighborList(instance, 50);
				neighborList.WriteTo(@"data/" + instanceName + ".neighbor");
			}
		}
	}
}