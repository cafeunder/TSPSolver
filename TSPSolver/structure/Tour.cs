
using System;

namespace TSPSolver.structure {
	public class Tour {
		public int[] NodeArray{ get; private set; }
		private int[] indexArray;

		public Tour(int nodeNum) {
			this.NodeArray = new int[nodeNum];
			this.indexArray = new int[nodeNum];
			for (int i = 0; i < this.NodeArray.Length; i++) {
				this.NodeArray[i] = i;
			}

			// �t�B�b�V���[�@��p���Ĕz����V���b�t��
 			for (int i = this.NodeArray.Length - 1; i > 0; i--) {
				int j = SRandom.Instance.NextInt(i + 1);
				int swap = this.NodeArray[i];
				this.NodeArray[i] = this.NodeArray[j];
				this.NodeArray[j] = swap;

				// this.nodeArray[i]�̃C���f�b�N�X�͊m�肵���̂ŃC���f�b�N�X�z��ɒǉ�
				this.indexArray[this.NodeArray[i]] = i; 
			}
		}

		/// <summary>
		/// ����m�[�h�̎��ɗ���m�[�h��Ԃ�
		/// </summary>
		public int nextID(int node) {
			// node�̎��̃C���f�b�N�X�̃m�[�h��Ԃ�
			// �������A�C���f�b�N�X���z��̒����𒴂����ꍇ��0�ɖ߂�
			return this.NodeArray[(this.indexArray[node] + 1) % this.NodeArray.Length];
		}

		/// <summary>
		/// ����m�[�h�̑O�ɂ���m�[�h��Ԃ�
		/// </summary>
		public int prevID(int node) {
			// node�̑O�̃C���f�b�N�X�̃m�[�h��Ԃ�
			// �������A�C���f�b�N�X��0���������ꍇ�͔z��̒���-1�Ƃ���
			return this.NodeArray[(this.indexArray[node] + this.NodeArray.Length - 1) % this.NodeArray.Length];
		}

		/// <summary>
		/// �G�b�W�̓���ւ��ɑ�������z��̔��]���s��
		/// </summary>
		/// <param name="v1">�m�[�h1</param>
		/// <param name="v2">�m�[�h2</param>
		/// <param name="forward">���������H</param>
		public void flip(int v1, int v2, bool forward) {
			// va - vb      va   vb
			//          ->     X     �ƍl����
			// vc - vd      vc   vd
			int ia, ib, ic, id;
			if (forward) {
				ia = this.indexArray[v1];
				ib = (ia + 1) % this.NodeArray.Length;
				ic = this.indexArray[v2];
				id = (ic + 1) % this.NodeArray.Length;
			} else {
				ib = this.indexArray[v1];
				ia = (ib + this.NodeArray.Length - 1) % this.NodeArray.Length;
				id = this.indexArray[v2];
				ic = (id + this.NodeArray.Length - 1) % this.NodeArray.Length;
			}

			// va,vd�ԂƁAvb,vc�Ԃ̃C���f�b�N�X�̋������v�Z
			int length_ad = (ia - id);
			if (length_ad < 0) { length_ad += this.NodeArray.Length; }
			int length_cb = (ic - ib);
			if (length_cb < 0) { length_cb += this.NodeArray.Length; }

			// �C���f�b�N�X���߂��g�𔽓]�ʒu�Ƃ���
			int head, tail, length;
			if (length_ad < length_cb) {
				head = id;
				tail = ia;
				length = length_ad + 1;
			} else {
				head = ib;
				tail = ic;
				length = length_cb + 1;
			}

			// ���߂�ꂽ�ߓ_�ԂŔ��]���s��
			for (int i = 0; i < length / 2; i++) {
				int temp = this.NodeArray[head];
				this.NodeArray[head] = this.NodeArray[tail];
				this.NodeArray[tail] = temp;
				this.indexArray[this.NodeArray[head]] = head;
				this.indexArray[this.NodeArray[tail]] = tail;

				head = (head + 1) % this.NodeArray.Length;
				tail = (tail + this.NodeArray.Length - 1) % this.NodeArray.Length;
			}
		}

		/// <summary>
		/// �I�[�o�[���C�h����ToString���\�b�h
		/// </summary>
		override public string ToString() {
			string result = "";
			foreach (int n in this.NodeArray) {
				result += n + ",";
			}
			return result;
		}

		/// <summary>
		/// �f�o�b�O�p���O
		/// </summary>
		public void DebugLog() {
			Console.WriteLine("node : " + this);
			string result = "indx : ";
			foreach (int n in this.indexArray) {
				result += n + ",";
			}
			Console.WriteLine(result);
		}
	}
}
