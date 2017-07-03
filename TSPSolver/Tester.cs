
using System;
using TSPSolver.neighborList;
using TSPSolver.solver;

namespace TSPSolver {
	public class Tester {
		public static void Main(string[] args) {
			SRandom.Intialize(0);
			string instanceName = "ca4663";

			TSPInstance instance = new TSPInstance(@"data/" + instanceName + ".tsp");
			NeighborList neighborList = new NeighborList();
			neighborList.ReadFrom(@"data/" + instanceName + ".neighbor");
			InverseNeighborList invNeighborList = new InverseNeighborList();
			invNeighborList.ReadFrom(@"data/" + instanceName + ".inv.neighbor");
			Solver solver = new NeighborDLB2opt(neighborList, invNeighborList);

			int[] tour = solver.Run(instance);
			Console.WriteLine("length : " + instance.CalcTourLength(tour));
		}
	}
}
