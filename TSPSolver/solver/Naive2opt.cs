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

			// �����_���ȃm�[�hs����n�߂đS�Ẵm�[�h�ɐڑ����Ă���G�b�W����������
			int si = SRandom.Instance.NextInt(instance.Dimension);
			for (int i = 0; i < instance.Dimension; i++) {
				int v = (si + i) % instance.Dimension;
				
				// �����Ƌt���̃G�b�W���m���߂�
				for (int d = 0; d < 2; d++) {
					bool forward = (d == 0);
					int vn = (forward) ? tour.NextID(v) : tour.PrevID(v);

					// �����_���ȃm�[�hs����n�߂đS�Ẵm�[�h�ɐڑ����Ă���G�b�W����������
					int sj = SRandom.Instance.NextInt(instance.Dimension);
					for (int j = 0; j < instance.Dimension; j++) {
						int w = (sj + j) % instance.Dimension;
						if (w == v) { continue; }

						int wn = (forward) ? tour.NextID(w) : tour.PrevID(w);
						// (v, vn)��(w, wn)���폜����
						int remove_gain = instance.CalcDistance(v, vn) + instance.CalcDistance(w, wn);
						// (v, w)��(vn, wn)��ǉ�����
						int add_gain = instance.CalcDistance(v, w) + instance.CalcDistance(vn, wn);

						if (add_gain < remove_gain) {
							tour.Flip(v, w, forward);
#if DEBUG
							length += add_gain - remove_gain;
							Console.WriteLine(length + ", " + instance.CalcTourLength(tour.NodeArray));
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

			return tour.NodeArray;
		}
	}
}
