using System;
using System.Text;
using TSPSolver.common;

namespace TSPSolver.structure {
	public class CityArrayTour : Tour {
		// �s�s�z��
		public int[] cityArray;
		// ����s�si��cityArray��̂ǂ̈ʒu�ɂ��邩��\���z��
		private int[] indexArray;

		public CityArrayTour(int cityNum) {
			this.cityArray = new int[cityNum];
			this.indexArray = new int[cityNum];
			for (int i = 0; i < this.cityArray.Length; i++) {
				this.cityArray[i] = i;
				this.indexArray[i] = i;
			}

			// �t�B�b�V���[�@��p���Ĕz����V���b�t��
 			for (int i = this.cityArray.Length - 1; i > 0; i--) {
				int j = SRandom.Instance.NextInt(i + 1);
				Common.Swap(ref indexArray[cityArray[i]], ref indexArray[cityArray[j]]);
				Common.Swap(ref cityArray[i], ref cityArray[j]);
			}
		}

		/// <summary>
		/// ����s�s�̎��ɗ���s�s��Ԃ�
		/// </summary>
		override public int NextID(int city) {
			// city�̎��̃C���f�b�N�X�̓s�s��Ԃ�
			// �������A�C���f�b�N�X���z��̒����𒴂����ꍇ��0�ɖ߂�
			if (this.indexArray[city] < this.cityArray.Length - 1) {
				return this.cityArray[this.indexArray[city] + 1];
			}
			return this.cityArray[0];
		}

		/// <summary>
		/// ����s�s�̑O�ɂ���s�s��Ԃ�
		/// </summary>
		override public int PrevID(int city) {
			// city�̑O�̃C���f�b�N�X�̓s�s��Ԃ�
			// �������A�C���f�b�N�X��0���������ꍇ�͔z��̒���-1�Ƃ���
			if (this.indexArray[city] > 0) {
				return this.cityArray[this.indexArray[city] - 1];
			}
			return this.cityArray[this.cityArray.Length - 1];
		}

		/// <summary>
		/// �G�b�W�̓���ւ��ɑ�������z��̔��]���s��
		/// </summary>
		override public void Flip(int va, int vb, int vc, int vd) {
			// va - vb      va   vb
			//          ->     X     �ƍl����
			// vc - vd      vc   vd
			int ia = this.indexArray[va], ib = this.indexArray[vb],
				ic = this.indexArray[vc], id = this.indexArray[vd];

			// va,vd�ԂƁAvb,vc�Ԃ̃C���f�b�N�X�̋������v�Z
			int length_ad = (ia - id);
			if (length_ad < 0) { length_ad += this.cityArray.Length; }
			int length_cb = (ic - ib);
			if (length_cb < 0) { length_cb += this.cityArray.Length; }

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
				Common.Swap(ref indexArray[cityArray[head]], ref indexArray[cityArray[tail]]);
				Common.Swap(ref cityArray[head], ref cityArray[tail]);

				head = head + 1;
				if (head == this.cityArray.Length) { head = 0; }
				tail = tail - 1;
				if (tail == -1) { tail = this.cityArray.Length - 1;}
			}
		}

		/// <summary>
		/// �s�s��K�⏇�ɕ��ׂ��z���Ԃ�
		/// </summary>
		override public int[] GetTour() {
			return this.cityArray;
		}

		/// <summary>
		/// �f�o�b�O�p���O
		/// </summary>
		public void DebugLog() {
			Console.WriteLine("city : " + this);
			string result = "indx : ";
			foreach (int n in this.indexArray) {
				result += n + ",";
			}
			Console.WriteLine(result);
		}
	}
}
