using System;
using System.Diagnostics;
using TSPSolver.common;
using TSPSolver.solver;
using TSPSolver.solver.util;

namespace TSPSolver {
	public class Benchmarker {
		public static string[] INSTANCES = {
			"ca4663",
			"fi10639",
			"bm33708",
			"ch71009",
			"mona-lisa100K"
		};

		/// <summary>
		/// ソルバを起動する
		/// </summary>
		public static void Run(TSPInstance instance, Solver solver) {
			// 試行回数
			int trial_num = 1;
			// 経路長の合計、経路長の2乗の合計
			double length_sum = 0;
			double length_sum2 = 0;
			// 計算時間の2乗の合計
			double time_sum = 0;
			double time_sum2 = 0;
			// 選択回数の合計、選択回数の2乗の合計
			double select_sum = 0;
			double select_sum2 = 0;

			Stopwatch sw = new Stopwatch();
			for (int i = 0; i < trial_num; i++) {
				sw.Start();
				// 計算する
				(int selectNum, int[] tour) = solver.Run(instance);
				sw.Stop();

				// 経路長の計上
				int length = instance.CalcTourLength(tour);
				length_sum += length;
				length_sum2 += (double)length * length;

				// 計算時間の計上
				time_sum += sw.ElapsedMilliseconds;
				time_sum2 += (double)sw.ElapsedMilliseconds * sw.ElapsedMilliseconds;

				// 選択回数の計上
				select_sum += selectNum;
				select_sum2 += selectNum * (double)selectNum;
				sw.Reset();
			}

			double length_ave = (length_sum / trial_num);
			double length_sd = Math.Sqrt(length_sum2 / (trial_num - 1.0) - (trial_num) / (trial_num - 1.0) * length_ave * length_ave);
			double time_ave = (time_sum / trial_num);
			double time_sd = Math.Sqrt(time_sum2 / (trial_num - 1.0) - (trial_num) / (trial_num - 1.0) * time_ave * time_ave);
			double select_ave = (select_sum / trial_num);
			double select_sd = Math.Sqrt(select_sum2 / (trial_num - 1.0) - (trial_num) / (trial_num - 1.0) * select_ave * select_ave);

			Console.WriteLine("result : ave[sd]");
			Console.WriteLine("length : " + length_ave + "[" + length_sd + "]");
			Console.WriteLine("time   : " + time_ave + "[" + time_sd + "]");
			Console.WriteLine("select   : " + select_ave + "[" + select_sd + "]");
		}

		/// <summary>
		/// 近傍リストを作成する
		/// </summary>
		public static void RunNeighbor(string instanceName) {
			TSPInstance instance = new TSPInstance(@"data/" + instanceName + ".tsp");

			// 試行回数
			int trial_num = 10;
			// 計算時間の2乗の合計
			double time_sum = 0;
			double time_sum2 = 0;

			Stopwatch sw = new Stopwatch();
			for (int i = 0; i < trial_num; i++) {
				sw.Start();
				// 近傍リストを作る
				NeighborList neighborList = new NeighborList(instance, 50);
				sw.Stop();

				// 計算時間の計上
				time_sum += sw.ElapsedMilliseconds;
				time_sum2 += (double)sw.ElapsedMilliseconds * sw.ElapsedMilliseconds;
				sw.Reset();
			}

			double time_ave = (time_sum / trial_num);
			double time_sd = Math.Sqrt(time_sum2 / (trial_num - 1.0) - (trial_num) / (trial_num - 1.0) * time_ave * time_ave);
			Console.WriteLine("result : ave[sd]");
			Console.WriteLine("time   : " + time_ave + "[" + time_sd + "]");
		}

		/// <summary>
		/// 逆近傍リストを作成する
		/// </summary>
		public static void RunInvNeighbor(string instanceName) {
			TSPInstance instance = new TSPInstance(@"data/" + instanceName + ".tsp");

			// 近傍リスト初期化
			NeighborList neighborList = new NeighborList();
			neighborList.ReadFrom(@"data/" + instanceName + ".neighbor");

			// 試行回数
			int trial_num = 10;
			// 計算時間の2乗の合計
			double time_sum = 0;
			double time_sum2 = 0;

			Stopwatch sw = new Stopwatch();
			for (int i = 0; i < trial_num; i++) {
				sw.Start();
				// 逆近傍リストを作る
				InverseNeighborList invNeighborList = new InverseNeighborList(neighborList);
				sw.Stop();

				// 計算時間の計上
				time_sum += sw.ElapsedMilliseconds;
				time_sum2 += (double)sw.ElapsedMilliseconds * sw.ElapsedMilliseconds;
				sw.Reset();
			}

			double time_ave = (time_sum / trial_num);
			double time_sd = Math.Sqrt(time_sum2 / (trial_num - 1.0) - (trial_num) / (trial_num - 1.0) * time_ave * time_ave);
			Console.WriteLine("result : ave[sd]");
			Console.WriteLine("time   : " + time_ave + "[" + time_sd + "]");
		}

		public static void Main(string[] args) {
			foreach (string instanceName in INSTANCES) {
				Console.WriteLine(instanceName);
				TSPInstance instance = new TSPInstance(@"data/" + instanceName + ".tsp");

				// Benchmarker.Run(instance, new Naive2opt());
				/*
				NeighborList neighborList = new NeighborList();
				neighborList.ReadFrom(@"data/" + instanceName + ".neighbor");
				Benchmarker.Run(instance, new Neighbor2opt(neighborList));
				*/
				/*
				NeighborList neighborList = new NeighborList();
				neighborList.ReadFrom(@"data/" + instanceName + ".neighbor");
				Benchmarker.Run(instance, new NeighborDLB2opt(neighborList));
				*/
				/*
				NeighborList neighborList = new NeighborList();
				neighborList.ReadFrom(@"data/" + instanceName + ".neighbor");
				InverseNeighborList invNeighborList = new InverseNeighborList(neighborList);
				Benchmarker.Run(instance, new NeighborInvDLB2opt(neighborList, invNeighborList));
				*/

				// Benchmarker.RunNeighbor(instanceName);
				Benchmarker.RunInvNeighbor(instanceName);
			}
		}
	} 
}
