using System;
using System.Diagnostics;
using TSPSolver.solver;

namespace TSPSolver {
	public class Benchmarker {
		private static string[] INSTANCES = {
			"ca4663",
			"fi10639",
			"bm33708",
			"ch71009",
			"mona-lisa100K"
		};

		public static void Run(string instanceName) {
			// 使うインスタンス
			TSPInstance instance = new TSPInstance(@"data/" + instanceName + ".tsp");
			// 試行回数
			int trial_num = 10;
			// 経路長の合計、経路長の2乗の合計
			double length_sum = 0;
			double length_sum2 = 0;
			// 計算時間の2乗の合計
			double time_sum = 0;
			double time_sum2 = 0;

			Stopwatch sw = new Stopwatch();
			for (int i = 0; i < trial_num; i++) {
				sw.Start();
				// 計算する
				int[] tour = Naive2opt.Solve(instance);
				sw.Stop();

				// 経路長の計上
				int length = instance.CalcTourLength(tour);
				length_sum += length;
				length_sum2 += (double)length * length;

				// 計算時間の計上
				time_sum += sw.ElapsedMilliseconds;
				time_sum2 += (double)sw.ElapsedMilliseconds * sw.ElapsedMilliseconds;
				sw.Reset();
			}

			double length_ave = (length_sum / (double)trial_num);
			double length_sd = Math.Sqrt(length_sum2 / (trial_num - 1.0) - (trial_num) / (trial_num - 1.0) * length_ave * length_ave);
			Console.WriteLine(length_sum2 / (trial_num - 1.0));
			Console.WriteLine((trial_num) / (trial_num - 1.0) * length_ave * length_ave);
			double time_ave = (time_sum / (double)trial_num);
			double time_sd = Math.Sqrt(time_sum2 / (trial_num - 1.0) - (trial_num) / (trial_num - 1.0) * time_ave * time_ave);

			Console.WriteLine("result : ave[sd]");
			Console.WriteLine("length : " + length_ave + "[" + length_sd + "]");
			Console.WriteLine("time   : " + time_ave + "[" + time_sd + "]");
		}

		public static void Main(string[] args) {
			foreach (string instanceName in INSTANCES) {
				Benchmarker.Run(instanceName);
			}
		}
	}
}
