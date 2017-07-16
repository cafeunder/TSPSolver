
using System;
using TSPSolver.solver;
using TSPSolver.common;
using TSPSolver.solver.util;
using TSPSolver.structure;

namespace TSPSolver {
	public class Tester {
		public static void Main(string[] args) {
			/*
			SRandom.Intialize(0);
			string instanceName = "ca4663";

			TSPInstance instance = new TSPInstance(@"data/" + instanceName + ".tsp");
			NeighborList neighborList = new NeighborList();
			neighborList.ReadFrom(@"data/" + instanceName + ".neighbor");
			Solver solver = new NeighborDLB2opt(neighborList);
			Solver solver = new Naive2opt();

			int[] tour = solver.Run(instance);
			Console.WriteLine("length : " + instance.CalcTourLength(tour));
			*/
			SRandom.Intialize(2);
			Tour tour = new TwoLevelTreeTour(14);
			Console.WriteLine(tour);

			SRandom.Intialize(2);
			tour = new CityArrayTour(14);
			Console.WriteLine(tour);
		}
	}
}
