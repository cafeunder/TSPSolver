
using System;
using TSPSolver.solver;

namespace TSPSolver {
	public class Tester {
		public static void Main(string[] args) {
			SRandom.Intialize(0);
			TSPInstance instance = new TSPInstance(@"data/ca4663.tsp");
			int[] tour = Naive2opt.Solve(instance);
			Console.WriteLine("length : " + instance.CalcTourLength(tour));
		}
	}
}
