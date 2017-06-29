using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TSPSolver.neighborList {
	/// <summary>
	/// �ߖT���X�g��\���N���X
	/// </summary>
	public class NeighborList {
		// �C���X�^���X�̎�����
		private int dimension;
		// �ߖT��
		public int NeighborNum { get; private set; }
		/// <summary>
		/// �C���f�b�N�X�ɑΉ�����s�s�̋ߖT�s�s���i�[����z��
		/// </summary>
		public int[][] NeighborNodes { get; private set; }
		/// <summary>
		/// �ߖT�s�s�Ƃ̋������i�[����z��
		/// </summary>
		public int[][] DistanceTable { get; private set; }

		// �f�t�H���g�R���X�g���N�^
		public NeighborList() { }
		
		/// <summary>
		/// Initialize���Ăяo���R���X�g���N�^
		/// </summary>
		public NeighborList(TSPInstance instance, int neighborNum) {
			this.Initialize(instance, neighborNum);
		}

		/// <summary>
		/// �ߖT���X�g�����
		/// </summary>
		public void Initialize(TSPInstance instance, int neighborNum) {
			this.dimension = instance.Dimension;
			this.NeighborNodes = new int[instance.Dimension][];
			this.DistanceTable = new int[instance.Dimension][];
			this.NeighborNum = (instance.Dimension - 1 < neighborNum) ? instance.Dimension - 1 : neighborNum;

			for (int i = 0; i < instance.Dimension; i++) {
				this.NeighborNodes[i] = new int[this.NeighborNum];
				this.DistanceTable[i] = new int[this.NeighborNum];
				this.NeighborNodes[i][0] = -1;
				this.DistanceTable[i][0] = int.MaxValue;
				int count = 0;
			
				for (int j = 0; j < instance.Dimension; j++) {
					if (i == j) { continue; }
					int distance = instance.CalcDistance(i, j);

					// �z��̖��� = �ł������������m�[�h�Ƌ������r
					if (distance < this.DistanceTable[i][count]) {
						// �z�񂪂����ς��łȂ��Ȃ疖���ɑ}���A�����ς��Ȃ疖�����㏑��
						int insert = (count < this.NeighborNum - 1) ? count + 1 : count;
						this.NeighborNodes[i][insert] = j;
						this.DistanceTable[i][insert] = distance;

						// ����̈ʒu�܂ł��炷
						for (int k = insert; k > 0; k--) {
							// �������O�̗v�f������������������� ����������̈ʒu
							if (this.DistanceTable[i][k - 1] <= this.DistanceTable[i][k]) {
								break;
							}

							// �ߖT�z��̃X���b�v
							int temp = this.NeighborNodes[i][k - 1];
							this.NeighborNodes[i][k - 1] = this.NeighborNodes[i][k];
							this.NeighborNodes[i][k] = temp;

							// �����e�[�u���̃X���b�v
							temp = this.DistanceTable[i][k - 1];
							this.DistanceTable[i][k - 1] = this.DistanceTable[i][k];
							this.DistanceTable[i][k] = temp;
						}

						if (count < this.NeighborNum - 1) {
							count++;
						}
					}
				}				
			}
		}

		public void WriteTo(string filepath) {
			using (BinaryWriter bw = new BinaryWriter(File.OpenWrite(filepath))) {
				bw.Write(this.dimension);
				bw.Write(this.NeighborNum);
				for (int i = 0; i < this.NeighborNodes.Length; i++) {
					for (int j = 0; j < this.NeighborNodes[i].Length; j++) {
						bw.Write(this.NeighborNodes[i][j]);
						bw.Write(this.DistanceTable[i][j]);
					}
				}
			}
		}

		public void ReadFrom(string filepath) {
			using (BinaryReader br = new BinaryReader(File.OpenRead(filepath))) {
				this.dimension = br.ReadInt32();
				this.NeighborNum = br.ReadInt32();
				this.NeighborNodes = new int[this.dimension][];
				this.DistanceTable = new int[this.dimension][];

				for (int i = 0; i < this.NeighborNodes.Length; i++) {
					this.NeighborNodes[i] = new int[this.NeighborNum];
					this.DistanceTable[i] = new int[this.NeighborNum];

					for (int j = 0; j < this.NeighborNodes[i].Length; j++) {
						this.NeighborNodes[i][j] = br.ReadInt32();
						this.DistanceTable[i][j] = br.ReadInt32();
					}
				}
			}
		}
	}
}
