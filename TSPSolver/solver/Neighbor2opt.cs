using System;
using TSPSolver.structure;
using TSPSolver.common;
using TSPSolver.solver.util;

namespace TSPSolver.solver {
	/// <summary>
	/// �ߖT���X�g����ADontLookBit�Ȃ�
	/// </summary>
	public class Neighbor2opt : Solver {
		// �ߖT���X�g
		NeighborList neighborList;

		public Neighbor2opt(NeighborList neighborList) {
			this.neighborList = neighborList;
		}

		override public (int, int[]) Run(TSPInstance instance) {
			int selectCount = 0;
			// �c�A�[�𐶐�
			Tour tour = new CityArrayTour(instance.Dimension);
#if DEBUG
			int length = instance.CalcTourLength(tour.GetTour());
#endif
			
			// �S�ẴG�b�W�����P�s�\�ɂȂ�܂ő�����
			int si = SRandom.Instance.NextInt(instance.Dimension);
			for (int i = 0; i < instance.Dimension; i++) {
				int v = (si + i) % instance.Dimension;
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
#if DEBUG
							length += add_gain - remove_gain;
							Console.WriteLine(length + ", " + instance.CalcTourLength(tour.GetTour()));
#endif
							// �ŏ�������Ȃ���
							i = 0;
							si = SRandom.Instance.NextInt(instance.Dimension);
							goto FINISH;
						}
					}
				}
			FINISH:;
			}

			return (selectCount, tour.GetTour());
		}
	}
}
