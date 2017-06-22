
using System;
using System.Diagnostics;
using TSPSolver.structure;

namespace TSPSolver {
	public class Program {
		public static void Main(string[] args) {
			TSPInstance instance = new TSPInstance(@"data/ca4663.tsp");

			Tour tour = new Tour(7);
		}
	}
}
