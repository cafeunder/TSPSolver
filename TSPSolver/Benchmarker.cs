using System;
using System.Diagnostics;
using TSPSolver.solver;

namespace TSPSolver {
	public class Benchmarker {
		public static void Main(string[] args) {
			// �g���C���X�^���X
			TSPInstance instance = new TSPInstance(@"data/ca4663.tsp");
			// ���s��
			int trial_num = 10;
			// �o�H���̍��v�A�o�H����2��̍��v
			int length_sum = 0;
			int length_sum2 = 0;
			// �v�Z���Ԃ�2��̍��v
			long time_sum = 0;
			long time_sum2 = 0;

			Stopwatch sw = new Stopwatch();
			for (int i = 0; i < trial_num; i++) {
				sw.Start();
				// �v�Z����
				int[] tour = NaiveSolver.Solve(instance);
				sw.Stop();

				// �o�H���̌v��
				int length = instance.CalcTourLength(tour);
				length_sum += length;
				length_sum2 += length * length;

				// �v�Z���Ԃ̌v��
				time_sum += sw.ElapsedMilliseconds;
				time_sum2 += sw.ElapsedMilliseconds * sw.ElapsedMilliseconds;
				sw.Reset();
			}

			double length_ave = (length_sum / (double)trial_num);
			double length_sd = Math.Sqrt(length_sum2 / (trial_num - 1.0) - (trial_num) / (trial_num - 1.0) * length_ave * length_ave);
			double time_ave = (time_sum / (double)trial_num);
			double time_sd = Math.Sqrt(time_sum2 / (trial_num - 1.0) - (trial_num) / (trial_num - 1.0) * time_ave * time_ave);

			Console.WriteLine("result : ave[sd]");
			Console.WriteLine("length : " + length_ave + "[" + length_sd + "]");
			Console.WriteLine("time   : " + time_ave + "[" + time_sd + "]");
		}
	}
}
