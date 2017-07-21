
using System;
using TSPSolver.solver;
using TSPSolver.common;
using TSPSolver.solver.util;
using TSPSolver.structure;

namespace TSPSolver {
	public class Tester {
		public static void Main(string[] args) {
			string instanceName = "bm33708";

			TSPInstance instance = new TSPInstance(@"data/" + instanceName + ".tsp");
			NeighborList neighborList = new NeighborList();
			neighborList.ReadFrom(@"data/" + instanceName + ".neighbor");
			InverseNeighborList invNeighborList = new InverseNeighborList(neighborList);
			Solver solver = new NeighborInvDLB2opt(neighborList, invNeighborList);
			// Solver solver = new Naive2opt();

			(int selectNum, int[] tour) = solver.Run(instance);
			Console.WriteLine("length : " + instance.CalcTourLength(tour));
			/*
			SRandom.Intialize(2);
			for (int i = 0; i < 1000; i++) {
				// int size = SRandom.Instance.NextInt(10000) + 10;
				int size = 13;
				TwoLevelTreeTour tour = new TwoLevelTreeTour(size);

				int va = SRandom.Instance.NextInt(size);
				int vc;
				do {
					vc = SRandom.Instance.NextInt(size);
				} while(vc == va || vc == tour.NextID(va) || va == tour.NextID(vc));

				int[] before = tour.GetTour();
				tour.Flip(va, tour.NextID(va), vc, tour.NextID(vc));
				int[] after = tour.GetTour();

				bool miss = false;
				for (int j = 0; j < size; j++) {
					if (before[j] != after[j]) { miss = true; }
				}

				if (miss) {
					Console.WriteLine("死");
				}
			}
			*/
		}
	}
}
