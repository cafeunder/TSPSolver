
using System;
using TSPSolver.solver;
using TSPSolver.common;
using TSPSolver.solver.util;
using TSPSolver.structure;

namespace TSPSolver {
	public class BestTourCalculator {
		public static void Main(string[] args) {
			string instanceName = "output";

			TSPInstance instance = new TSPInstance(@"data/" + instanceName + ".tsp");
			NeighborList neighborList = new NeighborList(instance, 50);

			InverseNeighborList invNeighborList = new InverseNeighborList(neighborList);
			Solver solver = new NeighborInvDLB2opt(neighborList, invNeighborList);

			(int selectNum, int[] tour) = solver.Run(instance);
			Console.WriteLine("length : " + instance.CalcTourLength(tour));
			Tour.WriteTo(@"data/" + instanceName + ".tour", tour);
		}
	}
}
