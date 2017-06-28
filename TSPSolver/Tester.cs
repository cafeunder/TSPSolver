
using System;
using TSPSolver.neighborList;
using TSPSolver.solver;

namespace TSPSolver {
	public class Tester {
		public static void Main(string[] args) {
			SRandom.Intialize(0);
			TSPInstance instance = new TSPInstance(@"data/ca4663.tsp");
			Solver solver = new Neighbor2opt(new NeighborList(instance, 50));

			int[] tour = solver.Run(instance);
			Console.WriteLine("length : " + instance.CalcTourLength(tour));
		}
	}
}
