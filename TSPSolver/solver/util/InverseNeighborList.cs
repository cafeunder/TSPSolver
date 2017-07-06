using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TSPSolver.solver;

namespace TSPSolver.solver.util {
	/// <summary>
	/// �ߖT���X�g��\���N���X
	/// </summary>
	public class InverseNeighborList {
		// �C���X�^���X�̎�����
		private int dimension;
		// �ߖT��
		public int NeighborNum { get; private set; }
		/// <summary>
		/// NeighborNodes[i] (int[]) : �C���f�b�N�Xi�ɑΉ�����s�s��k�ߖT�ȓ��Ɏ��s�s�̔z��
		/// </summary>
		public int[][] InvNeighborNodes { get; private set; }

		// �f�t�H���g�R���X�g���N�^
		public InverseNeighborList() { }
		
		/// <summary>
		/// Initialize���Ăяo���R���X�g���N�^
		/// </summary>
		public InverseNeighborList(NeighborList neighborList) {
			this.Initialize(neighborList);
		}

		/// <summary>
		/// �t�ߖT���X�g�����
		/// </summary>
		public void Initialize(NeighborList neighborList) {
			this.dimension = neighborList.NeighborNodes.Length;
			this.NeighborNum = neighborList.NeighborNum;

			// �z��̏�����
			int[] elm_num = new int[this.dimension];
			this.InvNeighborNodes = new int[this.dimension][];
			for (int i = 0; i < this.dimension; i++) {
				this.InvNeighborNodes[i] = new int[this.NeighborNum * this.NeighborNum];
			}

			// �ߖT���X�g�����ċt�ߖT���X�g�𖄂߂�
			for (int i = 0; i < dimension; i++) {
				for (int j = 0; j < this.NeighborNum; j++) {
					int v = neighborList.NeighborNodes[i][j];
					// i��j�ߖT��v -> v��j�ߖT�Ɏ��s�si
					this.InvNeighborNodes[v][elm_num[v]] = i;
					elm_num[v]++;
					if (elm_num[v] < this.NeighborNum * this.NeighborNum) {
						this.InvNeighborNodes[v][elm_num[v]] = -1;
					}
				}
			}
		}

		/// <summary>
		/// �m�[�hv��k�ߖT�ȓ��Ɏ��m�[�h�S�Ă�selectNodeList�ɒǉ�����
		/// </summary>
		/// <param name="selectNodeList">�I����⃊�X�g</param>
		/// <param name="v">�m�[�h</param>
		public void AddNearestNodes(SelectNodeList selectNodeList, int v) {
			for (int i = 0; i < this.InvNeighborNodes[v].Length && this.InvNeighborNodes[v][i] != -1; i++) {
				selectNodeList.Add(this.InvNeighborNodes[v][i]);
			}
		}

		public void WriteTo(string filepath) {
			using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(filepath))) {
				bw.Write(this.dimension);
				bw.Write(this.NeighborNum);
				for (int i = 0; i < this.InvNeighborNodes.Length; i++) {
					for (int j = 0; j < this.InvNeighborNodes[i].Length; j++) {
						bw.Write(this.InvNeighborNodes[i][j]);
					}
				}
			}
		}

		public void ReadFrom(string filepath) {
			using (BinaryReader br = new BinaryReader(File.OpenRead(filepath))) {
				this.dimension = br.ReadInt32();
				this.NeighborNum = br.ReadInt32();
				this.InvNeighborNodes = new int[this.dimension][];

				for (int i = 0; i < this.InvNeighborNodes.Length; i++) {
					this.InvNeighborNodes[i] = new int[this.NeighborNum];
					for (int j = 0; j < this.InvNeighborNodes[i].Length; j++) {
						this.InvNeighborNodes[i][j] = br.ReadInt32();
					}
				}
			}
		}
	}
}
