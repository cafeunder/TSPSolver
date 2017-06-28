using System;
using TSPSolver.neighborList;
using TSPSolver.structure;

namespace TSPSolver.solver {
	/*
	 * �ߖT���X�g����ADontLookBit�Ȃ�
	 */
	public class Neighbor2opt : Solver {
		// �ߖT���X�g
		NeighborList neighborList;

		public Neighbor2opt(NeighborList neighborList) {
			this.neighborList = neighborList;
		}

		override public int[] Run(TSPInstance instance) {
			// �c�A�[�𐶐�
			Tour tour = new Tour(instance.Dimension);
#if DEBUG
			int length = instance.CalcTourLength(tour.NodeArray);
#endif

			// �I����⃊�X�g�𐶐�
			SelectNodeList selectNodeList = new SelectNodeList(instance.Dimension);

			// �S�ẴG�b�W�����P�s�\�ɂȂ�܂ő�����
			while (selectNodeList.Size != 0) {
				int v = selectNodeList.GetRand();
				int vn = tour.nextID(v);
				int dist = instance.CalcDistance(v, vn);

				// �����_���ȃm�[�hs����n�߂đS�Ẵm�[�h�ɐڑ����Ă���G�b�W����������
				for (int i = 0; i < this.neighborList.NeighborNum; i++) {
					int w = this.neighborList.NeighborNodes[v][i];
					// v - vn���� v - w�̕��������Ȃ�I��
					if (this.neighborList.DistanceTable[v][i] >= dist) {
						break;
					}

					int wn = tour.nextID(w);
					// (v, vn)��(w, wn)���폜����
					int remove_gain = dist + instance.CalcDistance(w, wn);
					// (v, w)��(vn, wn)��ǉ�����
					int add_gain = this.neighborList.DistanceTable[v][i] + instance.CalcDistance(vn, wn);

					if (add_gain < remove_gain) {
						tour.flip(v, w, true);
						selectNodeList.Add(w);
#if DEBUG
						length += add_gain - remove_gain;
						Console.WriteLine(length + ", " + instance.CalcTourLength(tour.NodeArray));
#endif
						goto SUCCESS;
					}
				}
				// ���P���s
				selectNodeList.Remove(v);
			SUCCESS:;
			}

			return tour.NodeArray;
		}
	}
}
