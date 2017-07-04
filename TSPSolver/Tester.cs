﻿
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
			Solver solver = new NeighborDLB2opt(neighborList);

			int[] tour = solver.Run(instance);
			Console.WriteLine("length : " + instance.CalcTourLength(tour));
		}
	}
}
