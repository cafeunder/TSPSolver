
using System;

namespace TSPSolver {
	public class Program {
		public static void Main(string[] args) {
			TSPInstance instance = new TSPInstance(@"data/ca4663.tsp");

			for (int i = 0; i < instance.Dimension; i++) {
				Console.WriteLine(instance.Coordinate[i][0] + ", " + instance.Coordinate[i][1]);
			}
		}
	}
}
