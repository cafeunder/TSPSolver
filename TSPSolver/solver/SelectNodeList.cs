
namespace TSPSolver.solver {
	/// <summary>
	/// �I����⃊�X�g����������N���X
	/// </summary>
	public class SelectNodeList {
		// �v�f���i�[����z��
		private int[] elmArray;
		// �v�f�̃C���f�b�N�X���i�[����z��
		private int[] indexArray;
		// �v�f���z�񒆂ɑ��݂��邩�ǂ����̃t���O���Ǘ�����z��
		private bool[] existArray;
		// �z��̃T�C�Y
		public int Size { get; set; }

		public SelectNodeList(int dimension) {
			this.Size = dimension;
			this.elmArray = new int[this.Size];
			this.indexArray = new int[this.Size];
			this.existArray = new bool[this.Size];

			for (int i = 0; i < this.Size; i++) {
				this.elmArray[i] = i;
				this.indexArray[i] = i;
				this.existArray[i] = true;
			}
		}

		/// <summary>
		/// �v�f��ǉ�����
		/// </summary>
		public void Add(int elm) {
			if (this.existArray[elm]) { return; }
			this.existArray[elm] = true;
			this.elmArray[this.Size] = elm;
			this.indexArray[elm] = this.Size;
			this.Size++;
		}

		/// <summary>
		/// �v�f���폜����
		/// </summary>
		/// <param name="elm"></param>
		public void Remove(int elm) {
			if (!this.existArray[elm]) { return; }
			this.existArray[elm] = false;
			//�����̈ʒu���A�폜����v�f�̈ʒu�ōX�V
			this.indexArray[this.elmArray[this.Size - 1]] = this.indexArray[elm];
			//�폜�����v�f�����݂���C���f�b�N�X�ɁA������ǉ�
			this.elmArray[this.indexArray[elm]] = this.elmArray[this.Size - 1];
			this.Size--;
		}

		/// <summary>
		/// �z�񒆂̗v�f�������_���Ɏ擾����
		/// </summary>
		public int GetRand() {
			return this.elmArray[SRandom.Instance.NextInt(this.Size)];
		}
	}
}
