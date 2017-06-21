

namespace TSPSolver.structure {
	public class Tour {
		public int[] NodeArray{ get; private set; }
		private int[] orderArray;

		public Tour(int nodeNum) {
			this.NodeArray = new int[nodeNum];
			this.orderArray = new int[nodeNum];
			for (int i = 0; i < this.NodeArray.Length; i++) {
				this.NodeArray[i] = i;
			}

			// �t�B�b�V���[�@��p���Ĕz����V���b�t��
 			for (int i = this.NodeArray.Length - 1; i > 0; i--) {
				int j = SRandom.Instance.NextInt(i + 1);
				int swap = this.NodeArray[i];
				this.NodeArray[i] = this.NodeArray[j];
				this.NodeArray[j] = swap;

				// this.nodeArray[i]�̃C���f�b�N�X�͊m�肵���̂ŃI�[�_�[�z��ɒǉ�
				this.orderArray[this.NodeArray[i]] = i; 
			}
		}

		/// <summary>
		/// ����m�[�h�̎��ɗ���m�[�h��Ԃ�
		/// </summary>
		public int nextID(int node) {
			// node�̎��̃C���f�b�N�X�̃m�[�h��Ԃ�
			// �������A�C���f�b�N�X���z��̒����𒴂����ꍇ��0�ɖ߂�
			return this.NodeArray[(this.orderArray[node] + 1) % this.NodeArray.Length];
		}
	}
}
