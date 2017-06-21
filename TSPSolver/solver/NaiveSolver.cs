using System.Collections.Generic;
using TSPSolver.structure;

namespace TSPSolver.solver {
	public class NaiveSolver {
		/// <summary>
		/// �m�[�h�Ԃ̋����e�[�u��
		/// </summary>
		private int[][] distTable;

		public void Solve(TSPInstance instance) {
			Tour tour = new Tour(instance.Dimension);

			// ���������炩���ߌv�Z���e�[�u���Ƃ��ĕێ�
			this.distTable = new int[instance.Dimension][];
			for (int i = 0; i < instance.Dimension; i++) {
				this.distTable[i] = new int[instance.Dimension];
				for (int j = 0; j < instance.Dimension; j++) {
					this.distTable[i][j] = instance.CalcDistance(i, j);
				}
			}

			// �������X�^�b�N�𐶐�
			Stack<int> nodeStack = new Stack<int>(instance.Dimension);
			foreach (int node in tour.NodeArray) {
				nodeStack.Push(node);
			}

			// �S�ẴG�b�W�����P�s�\�ɂȂ�܂ő�����
			while (nodeStack.Count != 0) {
				int v = nodeStack.Pop();

				// �����_���ȃm�[�hs����n�߂đS�Ẵm�[�h�ɐڑ����Ă���G�b�W����������
				int s = SRandom.Instance.NextInt(instance.Dimension);
				for (int i = 0; i < instance.Dimension; i++) {
					int w = (s + i) % instance.Dimension;

					// (v, v+1)��(w, w+1)���폜����
					int remove_gain = this.distTable[v][tour.nextID(v)] + this.distTable[w][tour.nextID(w)];
					// (v, w)��(v+1, w+1)��ǉ�����
					int add_gain = this.distTable[v+1][tour.nextID(v+1)] + this.distTable[w+1][tour.nextID(w+1)];

					if (add_gain < remove_gain) {

					}
				}
			}
		}
	}
}
