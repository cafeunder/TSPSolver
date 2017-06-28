using System;
using TSPSolver.structure;

namespace TSPSolver.solver {
	/*
	 * �ߖT���X�g�Ȃ��ADontLookBit�Ȃ�
	 */
	public class Naive2opt : Solver {
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

				// �����_���ȃm�[�hs����n�߂đS�Ẵm�[�h�ɐڑ����Ă���G�b�W����������
				int s = SRandom.Instance.NextInt(instance.Dimension);
				for (int i = 0; i < instance.Dimension; i++) {
					int w = (s + i) % instance.Dimension;
					if (w == v) { continue; }

					int wn = tour.nextID(w);
					// (v, vn)��(w, wn)���폜����
					int remove_gain = instance.CalcDistance(v, vn) + instance.CalcDistance(w, wn);
					// (v, w)��(vn, wn)��ǉ�����
					int add_gain = instance.CalcDistance(v, w) + instance.CalcDistance(vn, wn);

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
