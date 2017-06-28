using System;
using TSPSolver.structure;

namespace TSPSolver.solver {
	/*
	 * �ߖT���X�g����ADontLookBit�Ȃ�
	 */
	public class Neighbor2opt {
		public static int[] Solve(TSPInstance instance) {
			// �c�A�[�𐶐�
			Tour tour = new Tour(instance.Dimension);
#if DEBUG
			int length = instance.CalcTourLength(tour.NodeArray);
#endif

			// �m�[�h�Ԃ̋����e�[�u��
			int[][] distTable = new int[instance.Dimension][];
			for (int i = 0; i < instance.Dimension; i++) {
				distTable[i] = new int[instance.Dimension];
				for (int j = 0; j < instance.Dimension; j++) {
					distTable[i][j] = instance.CalcDistance(i, j);
				}
			}

			// �I����⃊�X�g�𐶐�
			SelectNodeList selectNodeList = new SelectNodeList(instance.Dimension);

			// �S�ẴG�b�W�����P�s�\�ɂȂ�܂ő�����
			while (selectNodeList.Size != 0) {
				int v = selectNodeList.GetRand();
				int vn = tour.nextID(v);

				// �����_���ȃm�[�hs����n�߂đS�Ẵm�[�h�ɐڑ����Ă���G�b�W����������
				int s = SRandom.Instance.NextInt(instance.Dimension);
				for (int i = 0; i < instance.Dimension; i++) {
					int w = (s + i) % instance.Dimension;
					if (w == v) { continue; }

					int wn = tour.nextID(w);
					// (v, vn)��(w, wn)���폜����
					int remove_gain = distTable[v][vn] + distTable[w][wn];
					// (v, w)��(vn, wn)��ǉ�����
					int add_gain = distTable[v][w] + distTable[vn][wn];

					if (add_gain < remove_gain) {
						tour.flip(v, w, true);
						selectNodeList.Add(w);
#if DEBUG
						length += add_gain - remove_gain;
						Console.WriteLine(length);
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
