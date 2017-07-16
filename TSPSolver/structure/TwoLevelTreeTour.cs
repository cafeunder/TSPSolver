using System;
using TSPSolver.common;

namespace TSPSolver.structure {
	public class TwoLevelTreeTour : Tour {
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
		/// �������C�ꎞ�Z�O�����g�͌��Ɋ܂߂��C�ꎞ�Z�O�����g�̏����ێ����Ȃ��D
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
			// segLinkOrder�͈ꎞ�Z�O�����g�̏���ێ����Ȃ�
			this.segLinkOrder = new int[this.segDefaultNum];
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

		public override void Flip(int va, int vb, int vc, int vd) {
			throw new NotImplementedException();
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
	}
}
