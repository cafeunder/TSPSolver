using System;
using TSPSolver.common;

namespace TSPSolver.structure {
	public class TwoLevelTreeTour : Tour {
		private static void Swap(ref int a, ref int b) {
			int temp = a;
			a = b;
			b = temp;
		}
		private const int NEXT = 0;
		private const int PREV = 1;

		/// <summary>
		/// �s�s��
		/// </summary>
		private int cityNum;
		/// <summary>
		/// �f�t�H���g�̃Z�O�����g��
		/// </summary>
		private int segDefaultNum;
		/// <summary>
		/// �Z�O�����g�̌�
		/// </summary>
		private int segNum;
		/// <summary>
		/// [i] : �Z�O�����gi�̌���
		/// </summary>
		private int[] segOrientation;
		/// <summary>
		/// [i][j] : �Z�O�����gj��i�̌����̂Ƃ��ɁC�擪�̓s�s���玟�ɂ��ǂ�Z�O�����g
		/// </summary>
		private int[][] segLink;
		/// <summary>
		/// [i] : �Z�O�����gi�̃T�C�Y
		/// </summary>
		private int[] segSize;
		/// <summary>
		/// [i] : ID:0�̃Z�O�����g����ID:i�̃Z�O�����g�ɓ��B����̂ɕK�v�ȃ����N���D
		/// </summary>
		private int[] segLinkOrder;
		/// <summary>
		/// [i][j] : �Z�O�����gj�̌�����i�̂Ƃ��̖����̓s�s
		/// </summary>
		private int[][] segTailCity;
		/// <summary>
		/// [i] : �s�si��������Z�O�����g
		/// </summary>
		private int[] cityToSegment;
		/// <summary>
		/// [i][j] : �Z�O�����g�̕�����i�̂Ƃ��C�Z�O�����g��œs�sj�̎��ɗ���s�s(�Ȃ��Ȃ�-1)
		/// </summary>
		private int[][] cityLink;
		/// <summary>
		/// [i] : �Z�O�����g�̌�����NEXT�ɂ����Ƃ��́C�s�si�̃Z�O�����g��ł̈ʒu
		/// </summary>
		private int[] cityPosition;


		public TwoLevelTreeTour(int cityNum) {
			this.cityNum = cityNum;
			this.segDefaultNum = (int)(Math.Sqrt(cityNum));
			this.segNum = this.segDefaultNum;

			//===�z��̐���===//
			this.segOrientation = new int[this.segDefaultNum + 2];
			this.segSize = new int[this.segDefaultNum + 2];
			this.segLinkOrder = new int[this.segDefaultNum + 2];
			this.segLink = new int[2][];
			this.segTailCity = new int[2][];
			for (int i = 0; i < 2; i++) {
				this.segLink[i] = new int[this.segDefaultNum + 2];
				this.segTailCity[i] = new int[this.segDefaultNum + 2];
			}
			this.cityToSegment = new int[cityNum];
			this.cityPosition = new int[cityNum];
			this.cityLink = new int[2][];
			for (int i = 0; i < 2; i++) {
				this.cityLink[i] = new int[cityNum];
			}

			//===�����o�H�̐���===//
			int[] cityArray = new int[cityNum];
			for (int i = 0; i < cityNum; i++) { cityArray[i] = i; }
			// �t�B�b�V���[�@��p���Ĕz����V���b�t��
 			for (int i = cityArray.Length - 1; i > 0; i--) {
				int j = SRandom.Instance.NextInt(i + 1);
				int swap = cityArray[i];
				cityArray[i] = cityArray[j];
				cityArray[j] = swap;
			}

			//===Two-Level-Tree�̏������F�s�s===//
			double sep = cityNum / (double)this.segDefaultNum;
			int segCityCount = 0;
			int segCount = 0;
			for (int i = 0; i < cityNum; i++) {
				this.cityToSegment[cityArray[i]] = segCount;
				this.cityPosition[cityArray[i]] = segCityCount;

				// �O�ɗ���s�s
				if (segCityCount == 0) {
					this.cityLink[PREV][cityArray[i]] = -1;
					this.segTailCity[PREV][segCount] = cityArray[i];
				} else {
					this.cityLink[PREV][cityArray[i]] = cityArray[i-1];
				}

				segCityCount++;
				// sep��؂�ɓ��B���C�z��̍Ō���̂Ƃ��ɃZ�O�����g��������
				if (i + 1 >= (sep * (segCount + 1)) || i == cityNum - 1) {
					// ���̃Z�O�����g�ɍs���O�Ɍ��݂̃Z�O�����g�̏����X�V
					this.cityLink[NEXT][cityArray[i]] = -1;
					this.segTailCity[NEXT][segCount] = cityArray[i];
					this.segSize[segCount] = segCityCount;
					segCityCount = 0;
					segCount++;
				} else {
					this.cityLink[NEXT][cityArray[i]] = cityArray[i+1];
				}
			}

			//===Two-Level-Tree�̏������F�Z�O�����g===//
			this.segLink[PREV][0] = this.segDefaultNum - 1;
			this.segLink[NEXT][0] = 1;
			this.segLinkOrder[0] = 0;
			for (int i = 1; i < this.segDefaultNum - 1; i++) {
				this.segLink[PREV][i] = i - 1;
				this.segLink[NEXT][i] = i + 1;
				this.segLinkOrder[i] = i;
			}
			this.segLink[PREV][this.segDefaultNum - 1] = this.segDefaultNum - 2;
			this.segLink[NEXT][this.segDefaultNum - 1] = 0;
			this.segLinkOrder[this.segDefaultNum - 1] = this.segDefaultNum - 1;
		}

		/// <summary>
		/// v1��v2�̊ԂŃZ�O�����g��؂藣���D
		/// ������� [v1] -> [v2]�ƂȂ�
		/// </summary>
		/// <param name="v1ID">v1��ID</param>
		/// <param name="v1Seg">v1�̃Z�O�����gID</param>
		/// <param name="v1Ort">v1�̕���</param>
		/// <param name="v2ID">v2��ID</param>
		/// <param name="v2Seg">v2�̃Z�O�����gID</param>
		/// <param name="v2Ort">v2�̕���</param>
		/// <param name="separateV2">v1��؂藣�����H</param>
		public void Separate(
			int v1ID, int v1Seg, int v1Ort,
			int v2ID, int v2Seg, int v2Ort,
			bool separateV2
		) {
			int v1_prev_seg = this.segLink[Turn(v1Ort)][v1Seg];
			int v2_next_seg = this.segLink[v2Ort][v2Seg];

			// ����Z�O�����g�ɂ���Ȃ�؂藣��(�ʂ̃Z�O�����g�Ȃ炷��K�v�Ȃ�)
			if (v1Seg == v2Seg) {
				if (separateV2) {
					// �����̏ꍇ��
					// �� �� �� �� �� �� �� �� 
					// [] [] [v1] [v2] [tail] �� [] [] [v1]>{[v2] [tail]} ����:2
					// �� �� �� �� �� �� �� ��  
					// [tail] [] [v2] [v1] [] �� {[tail] [] [v2]}<[v1] [] ����:3
					v2Seg = this.segNum++;
					v2Ort = v1Ort;

					int tailID = this.segTailCity[v1Ort][v1Seg];

					// ������s�s�̃����N���
					this.cityLink[v2Ort][tailID] = -1;
					this.cityLink[v1Ort][v1ID] = -1;
					this.cityLink[Turn(v2Ort)][v2ID] = -1;

					// �����ƃT�C�Y
					this.segOrientation[v2Seg] = v2Ort;
					this.segSize[v2Seg] = Math.Abs(this.cityPosition[v1ID] - this.cityPosition[tailID]);
					this.segSize[v1Seg] -= this.segSize[v2Seg];

					// �Z�O�����g��head��tail
					this.segTailCity[Turn(v2Ort)][v2Seg] = v2ID;
					this.segTailCity[v2Ort][v2Seg] = tailID;

					this.segTailCity[v1Ort][v1Seg] = v1ID;
					
					// �s�s���Z�O�����g�z��̕ύX
					for (int current = v2ID; current != -1; current = this.cityLink[v2Ort][current]) {
						this.cityToSegment[current] = v2Seg;
					}

					// ������NEXT�Ƃ����ꍇ�̈ʒu��������
					int fix_head = (v2Ort == NEXT) ? v2ID : tailID;
					int pos = 0;
					for (int current = fix_head; current != -1; current = this.cityLink[NEXT][current]) {
						this.cityPosition[current] = pos;
						pos++;
					}
				} else {
					// �����̏ꍇ��
					// �� �� �� �� �� �� �� �� 
					// [head] [] [v1] [v2] [] �� {[head] [] [v1]}>[v2] [] ����:3
					// �� �� �� �� �� �� �� ��  
					// [] [] [v2] [v1] [head] �� [] [] [v2]<{[v1] [head]} ����:2
					v1Seg = this.segNum++;
					v1Ort = v2Ort;

					int headID = this.segTailCity[Turn(v2Ort)][v2Seg];

					//===�؂藣�����Z�O�����g�̏����X�V===//
					// ������s�s�̃����N���
					this.cityLink[Turn(v1Ort)][headID] = -1;
					this.cityLink[v1Ort][v1ID] = -1;
					this.cityLink[Turn(v2Ort)][v2ID] = -1;

					// �����ƃT�C�Y
					this.segOrientation[v1Seg] = v1Ort;
					this.segSize[v1Seg] = Math.Abs(this.cityPosition[v2ID] - this.cityPosition[headID]);
					this.segSize[v2Seg] -= this.segSize[v1Seg];

					// �Z�O�����g��head��tail
					this.segTailCity[v1Ort][v1Seg] = v1ID;
					this.segTailCity[Turn(v1Ort)][v1Seg] = headID;

					this.segTailCity[Turn(v2Ort)][v2Seg] = v2ID;

					// �s�s���Z�O�����g�z��̕ύX
					for (int current = headID; current != -1; current = this.cityLink[v1Ort][current]) {
						this.cityToSegment[current] = v1Seg;
					}

					// ������NEXT�Ƃ����ꍇ�̈ʒu��������
					int fix_head = (v1Ort == NEXT) ? headID : v1ID;
					int pos = 0;
					for (int current = fix_head; current != -1; current = this.cityLink[NEXT][current]) {
						this.cityPosition[current] = pos;
						pos++;
					}
				}
			}

			// �����N��t���ւ���
			//               (1)         (2)         (3)
			// [v1_prev_seg] <-> [v1Seg] <-> [v2Seg] <-> [v2_next_seg]
			//=====(1)=====//
			this.segLink[this.segOrientation[v1_prev_seg]][v1_prev_seg] = v1Seg;
			this.segLink[Turn(this.segOrientation[v1Seg])][v1Seg] = v1_prev_seg;
			//=====(2)=====//
			this.segLink[this.segOrientation[v1Seg]][v1Seg] = v2Seg;
			this.segLink[Turn(this.segOrientation[v2Seg])][v2Seg] = v1Seg;
			//=====(3)=====//
			this.segLink[this.segOrientation[v2Seg]][v2Seg] = v2_next_seg;
			this.segLink[Turn(this.segOrientation[v2_next_seg])][v2_next_seg] = v2Seg;

			// �����N�I�[�_�[��ύX
			int seg = 0;
			for (int i = 0; i < this.segNum; i++) {
				this.segLinkOrder[seg] = i;
				seg = this.segLink[this.segOrientation[seg]][seg];
			}
		}

		public override void Flip(int va, int vb, int vc, int vd) {
			// vb,vc�Ԃ�t1 vd,va�Ԃ�t2�Ƃ���
			// va  vb      vc  vd
			// t2e t1s ... t1e t2s
			int t1s = vb;
			int t1e = vc;
			int t2s = vd;
			int t2e = va;

			// �e�s�s�̃Z�O�����g�ƁC���̕������擾����
			int t1s_seg = this.cityToSegment[t1s];
			int t1s_pos_seg = this.segLinkOrder[t1s_seg];
			int t1s_ort = this.segOrientation[t1s_seg];
			int t1e_seg = this.cityToSegment[t1e];
			int t1e_pos_seg = this.segLinkOrder[t1e_seg];
			int t1e_ort = this.segOrientation[t1e_seg];
			int t2s_seg = this.cityToSegment[t2s];
			int t2s_pos_seg = this.segLinkOrder[t2s_seg];
			int t2s_ort = this.segOrientation[t2s_seg];
			int t2e_seg = this.cityToSegment[t2e];
			int t2e_pos_seg = this.segLinkOrder[t2e_seg];
			int t2e_ort = this.segOrientation[t2e_seg];

			// ����ւ����K�v�ȃZ�O�����g�����擾����
			int length_t1 = t1e_pos_seg - t1s_pos_seg + 1;
			if (length_t1 < 0) { length_t1 += this.segNum; }
			int length_t2 = t2e_pos_seg - t2s_pos_seg + 1;
			if (length_t2 < 0) { length_t2 += this.segNum; }

			// ����ւ������Ȃ�����t1�Ƃ���
			if (length_t1 > length_t2) {
				Swap(ref length_t1, ref length_t2);
				Swap(ref t1s, ref t2s);
				Swap(ref t1e, ref t2e);
				Swap(ref t1s_seg, ref t2s_seg);
				Swap(ref t1e_seg, ref t2e_seg);
				Swap(ref t1s_pos_seg, ref t2s_pos_seg);
				Swap(ref t1e_pos_seg, ref t2e_pos_seg);
				Swap(ref t1s_ort, ref t2s_ort);
				Swap(ref t1e_ort, ref t2e_ort);
			}

			Console.WriteLine(this);
			this.DebugLog();

			Console.WriteLine(t1e + "," + t2s);
			this.Separate(t1e, t1e_seg, t1e_ort, t2s, t2s_seg, t2s_ort, false);

			Console.WriteLine(this);
			this.DebugLog();

			Console.WriteLine(t2e + "," + t1s);
			this.Separate(t2e, t2e_seg, t2e_ort, t1s, t1s_seg, t1s_ort, true);

			Console.WriteLine(this);
			this.DebugLog();
		}

		public override int[] GetTour() {
			int[] result = new int[this.cityNum];

			int current = 0;
			result[0] = current;
			for (int i = 1; i < this.cityNum; i++) {
				current = this.NextID(current);
				result[i] = current;
			}

			return result;
		}

		/// <summary>
		/// ����s�s�̎��ɗ���s�s��Ԃ�
		/// </summary>
		public override int NextID(int city) {
			int nextCity, seg, ort;

			seg = this.cityToSegment[city];
			ort = this.segOrientation[seg];

			nextCity = this.cityLink[ort][city];
			if (nextCity == -1) {
				seg = this.segLink[ort][seg];
				ort = this.segOrientation[seg];
				// �擪����������̂Ŗ���
				nextCity = this.segTailCity[Turn(ort)][seg];
			}
			return nextCity;
		}

		/// <summary>
		/// ����s�s�̑O�ɂ���s�s��Ԃ�
		/// </summary>
		public override int PrevID(int city) {
			int prevCity, seg, ort;

			seg = this.cityToSegment[city];
			ort = this.segOrientation[seg];

			prevCity = this.cityLink[Turn(ort)][city];
			if (prevCity == -1) {
				seg = this.segLink[Turn(ort)][seg];
				ort = this.segOrientation[seg];
				// ��������������̂Ő擪
				prevCity = this.segTailCity[ort][seg];
			}
			return prevCity;
		}

		/// <summary>
		/// �^����ꂽ�����Ƌt������Ԃ����\�b�h
		/// </summary>
		/// <param name="ort">����</param>
		/// <returns>ort�̋t����</returns>
		public static int Turn(int ort) {
			return (ort == PREV) ? NEXT : PREV;
		}

		public void DebugLog() {
			Console.WriteLine("===DEBUG LOG===");
			Console.WriteLine("cityLink[NEXT]");
			Console.WriteLine(ArrayToString(this.cityLink[NEXT]));
			Console.WriteLine("cityLink[PREV]");
			Console.WriteLine(ArrayToString(this.cityLink[PREV]));
			Console.WriteLine("cityToSegment");
			Console.WriteLine(ArrayToString(this.cityToSegment));
			Console.WriteLine("cityPosition");
			Console.WriteLine(ArrayToString(this.cityPosition));
			Console.WriteLine("segLink[NEXT]");
			Console.WriteLine(ArrayToString(this.segLink[NEXT]));
			Console.WriteLine("segLink[PREV]");
			Console.WriteLine(ArrayToString(this.segLink[PREV]));
			Console.WriteLine("segTailCity[NEXT]");
			Console.WriteLine(ArrayToString(this.segTailCity[NEXT]));
			Console.WriteLine("segTailCity[PREV]");
			Console.WriteLine(ArrayToString(this.segTailCity[PREV]));
			Console.WriteLine("segLinkOrder");
			Console.WriteLine(ArrayToString(this.segLinkOrder));
			Console.WriteLine("segSize");
			Console.WriteLine(ArrayToString(this.segSize));
			Console.WriteLine();
		}

		public static string ArrayToString(int[] ary) {
			string result = " ";
			for (int i = 0; i < ary.Length; i++) { result += String.Format("{0, 2}", i) + ","; }
			result += "\n[";
			foreach (int n in ary) { result += String.Format("{0, 2}", n) + ","; }
			result += "]";
			return result;
		}
	}
}
