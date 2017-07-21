using System;
using TSPSolver.structure;
using TSPSolver.common;
using TSPSolver.solver.util;

namespace TSPSolver.solver {
	/// <summary>
	/// �ߖT���X�g����ADontLookBit����
	/// </summary>
	public class NeighborInvDLB2opt : Solver {
		// �ߖT���X�g
		private NeighborList neighborList;
		private InverseNeighborList invNeighborList;

		public NeighborInvDLB2opt(NeighborList neighborList, InverseNeighborList invNeighborList) {
			this.neighborList = neighborList;
			this.invNeighborList = invNeighborList;
		}

		override public (int, int[]) Run(TSPInstance instance) {
			int selectCount = 0;
			Tour tour = new CityArrayTour(instance.Dimension);
			SelectNodeList selectNodeList = new SelectNodeList(instance.Dimension);

#if DEBUG
			int length = instance.CalcTourLength(tour.GetTour());
#endif			
			while (selectNodeList.Size != 0) {
				int v = selectNodeList.GetRand();
				selectCount++;

				// �����Ƌt���̃G�b�W���m���߂�
				for (int d = 0; d < 2; d++) {
					bool forward = (d == 0);
					int vn = (forward) ? tour.NextID(v) : tour.PrevID(v);
					int dist = instance.CalcDistance(v, vn);

					// �ŋߖT�̃m�[�h���猩�Ă���
					for (int j = 0; j < this.neighborList.NeighborNum; j++) {
						int w = this.neighborList.NeighborNodes[v][j];
						// v - vn���� v - w�̕��������Ȃ�I��
						if (this.neighborList.DistanceTable[v][j] >= dist) {
							break;
						}

						int wn = (forward) ? tour.NextID(w) : tour.PrevID(w);
						// (v, vn)��(w, wn)���폜����
						int remove_gain = dist + instance.CalcDistance(w, wn);
						// (v, w)��(vn, wn)��ǉ�����
						int add_gain = this.neighborList.DistanceTable[v][j] + instance.CalcDistance(vn, wn);

						if (add_gain < remove_gain) {
							if (forward) {
								tour.Flip(v, vn, w, wn);
							} else {
								tour.Flip(vn, v, wn, w);
							}
							// DLB�ǉ�
							selectNodeList.Add(w);
							selectNodeList.Add(vn);
							selectNodeList.Add(wn);
							invNeighborList.AddNearestNodes(selectNodeList, v);
							invNeighborList.AddNearestNodes(selectNodeList, vn);
							invNeighborList.AddNearestNodes(selectNodeList, w);
							invNeighborList.AddNearestNodes(selectNodeList, wn);
#if DEBUG
							length += add_gain - remove_gain;
							Console.WriteLine(length + ", " + instance.CalcTourLength(tour.GetTour()));
#endif
							goto FINISH;
						}
					}
				}
				selectNodeList.Remove(v);
			FINISH:;
			}

			return (selectCount, tour.GetTour());
		}
	}
}
