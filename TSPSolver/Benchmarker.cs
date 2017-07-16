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
		/// �\���o���N������
		/// </summary>
		public static void Run(TSPInstance instance, Solver solver) {
			// ���s��
			int trial_num = 10;
			// �o�H���̍��v�A�o�H����2��̍��v
			double length_sum = 0;
			double length_sum2 = 0;
			// �v�Z���Ԃ�2��̍��v
			double time_sum = 0;
			double time_sum2 = 0;

			Stopwatch sw = new Stopwatch();
			for (int i = 0; i < trial_num; i++) {
				sw.Start();
				// �v�Z����
				int[] tour = solver.Run(instance);
				sw.Stop();

				// �o�H���̌v��
				int length = instance.CalcTourLength(tour);
				length_sum += length;
				length_sum2 += (double)length * length;

				// �v�Z���Ԃ̌v��
				time_sum += sw.ElapsedMilliseconds;
				time_sum2 += (double)sw.ElapsedMilliseconds * sw.ElapsedMilliseconds;
				sw.Reset();
			}

			double length_ave = (length_sum / trial_num);
			double length_sd = Math.Sqrt(length_sum2 / (trial_num - 1.0) - (trial_num) / (trial_num - 1.0) * length_ave * length_ave);
			double time_ave = (time_sum / trial_num);
			double time_sd = Math.Sqrt(time_sum2 / (trial_num - 1.0) - (trial_num) / (trial_num - 1.0) * time_ave * time_ave);

			Console.WriteLine("result : ave[sd]");
			Console.WriteLine("length : " + length_ave + "[" + length_sd + "]");
			Console.WriteLine("time   : " + time_ave + "[" + time_sd + "]");
		}

		/// <summary>
		/// �ߖT���X�g���쐬����
		/// </summary>
		public static void RunNeighbor(string instanceName) {
			TSPInstance instance = new TSPInstance(@"data/" + instanceName + ".tsp");

			// ���s��
			int trial_num = 10;
			// �v�Z���Ԃ�2��̍��v
			double time_sum = 0;
			double time_sum2 = 0;

			Stopwatch sw = new Stopwatch();
			for (int i = 0; i < trial_num; i++) {
				sw.Start();
				// �ߖT���X�g�����
				NeighborList neighborList = new NeighborList(instance, 50);
				sw.Stop();

				// �v�Z���Ԃ̌v��
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
				NeighborList neighborList = new NeighborList();
				neighborList.ReadFrom(@"data/" + instanceName + ".neighbor");
				InverseNeighborList invNeighborList = new InverseNeighborList(neighborList);
				Benchmarker.Run(instance, new NeighborInvDLB2opt(neighborList, invNeighborList));

				// Benchmarker.RunNeighbor(instanceName);
			}
		}
	}
}
