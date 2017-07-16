using System;
using TSPSolver.common;

namespace TSPSolver.structure {
	public class TwoLevelTreeTour : Tour {
		private const int NEXT = 0;
		private const int PREV = 1;

		/// <summary>
		/// 都市数
		/// </summary>
		private int cityNum;
		/// <summary>
		/// デフォルトのセグメント数
		/// </summary>
		private int segDefaultNum;
		/// <summary>
		/// セグメントの個数
		/// </summary>
		private int segNum;
		/// <summary>
		/// [i] : セグメントiの向き
		/// </summary>
		private int[] segOrientation;
		/// <summary>
		/// [i][j] : セグメントjがiの向きのときに，先頭の都市から次にたどるセグメント
		/// </summary>
		private int[][] segLink;
		/// <summary>
		/// [i] : セグメントiのサイズ
		/// </summary>
		private int[] segSize;
		/// <summary>
		/// [i] : ID:0のセグメントからID:iのセグメントに到達するのに必要なリンク数．
		/// ただし，一時セグメントは個数に含めず，一時セグメントの情報も保持しない．
		/// </summary>
		private int[] segLinkOrder;
		/// <summary>
		/// [i][j] : セグメントjの向きがiのときの末尾の都市
		/// </summary>
		private int[][] segTailCity;
		/// <summary>
		/// [i] : 都市iが属するセグメント
		/// </summary>
		private int[] cityToSegment;
		/// <summary>
		/// [i][j] : セグメントの方向がiのとき，セグメント上で都市jの次に来る都市(ないなら-1)
		/// </summary>
		private int[][] cityLink;
		/// <summary>
		/// [i] : セグメントの向きをNEXTにしたときの，都市iのセグメント上での位置
		/// </summary>
		private int[] cityPosition;


		public TwoLevelTreeTour(int cityNum) {
			this.cityNum = cityNum;
			this.segDefaultNum = (int)(Math.Sqrt(cityNum));
			this.segNum = this.segDefaultNum;

			//===配列の生成===//
			this.segOrientation = new int[this.segDefaultNum + 2];
			this.segSize = new int[this.segDefaultNum + 2];
			// segLinkOrderは一時セグメントの情報を保持しない
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

			//===初期経路の生成===//
			int[] cityArray = new int[cityNum];
			for (int i = 0; i < cityNum; i++) { cityArray[i] = i; }
			// フィッシャー法を用いて配列をシャッフル
 			for (int i = cityArray.Length - 1; i > 0; i--) {
				int j = SRandom.Instance.NextInt(i + 1);
				int swap = cityArray[i];
				cityArray[i] = cityArray[j];
				cityArray[j] = swap;
			}

			//===Two-Level-Treeの初期化：都市===//
			double sep = cityNum / (double)this.segDefaultNum;
			int segCityCount = 0;
			int segCount = 0;
			for (int i = 0; i < cityNum; i++) {
				this.cityToSegment[cityArray[i]] = segCount;
				this.cityPosition[cityArray[i]] = segCityCount;

				// 前に来る都市
				if (segCityCount == 0) {
					this.cityLink[PREV][cityArray[i]] = -1;
					this.segTailCity[PREV][segCount] = cityArray[i];
				} else {
					this.cityLink[PREV][cityArray[i]] = cityArray[i-1];
				}

				segCityCount++;
				// sep区切りに到達か，配列の最後尾のときにセグメントを初期化
				if (i + 1 >= (sep * (segCount + 1)) || i == cityNum - 1) {
					// 次のセグメントに行く前に現在のセグメントの情報を更新
					this.cityLink[NEXT][cityArray[i]] = -1;
					this.segTailCity[NEXT][segCount] = cityArray[i];
					this.segSize[segCount] = segCityCount;
					segCityCount = 0;
					segCount++;
				} else {
					this.cityLink[NEXT][cityArray[i]] = cityArray[i+1];
				}
			}

			//===Two-Level-Treeの初期化：セグメント===//
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
		/// ある都市の次に来る都市を返す
		/// </summary>
		public override int NextID(int city) {
			int nextCity, seg, ort;

			seg = this.cityToSegment[city];
			ort = this.segOrientation[seg];

			nextCity = this.cityLink[ort][city];
			if (nextCity == -1) {
				seg = this.segLink[ort][seg];
				ort = this.segOrientation[seg];
				// 先頭から入ったので末尾
				nextCity = this.segTailCity[Turn(ort)][seg];
			}
			return nextCity;
		}

		/// <summary>
		/// ある都市の前にある都市を返す
		/// </summary>
		public override int PrevID(int city) {
			int prevCity, seg, ort;

			seg = this.cityToSegment[city];
			ort = this.segOrientation[seg];

			prevCity = this.cityLink[Turn(ort)][city];
			if (prevCity == -1) {
				seg = this.segLink[Turn(ort)][seg];
				ort = this.segOrientation[seg];
				// 末尾から入ったので先頭
				prevCity = this.segTailCity[ort][seg];
			}
			return prevCity;
		}

		/// <summary>
		/// 与えられた方向と逆方向を返すメソッド
		/// </summary>
		/// <param name="ort">方向</param>
		/// <returns>ortの逆方向</returns>
		public static int Turn(int ort) {
			return (ort == PREV) ? NEXT : PREV;
		}
	}
}
