using System;
using System.Text;
using TSPSolver.common;

namespace TSPSolver.structure {
	public class CityArrayTour : Tour {
		// 都市配列
		private int[] cityArray;
		// ある都市iがcityArray上のどの位置にあるかを表す配列
		private int[] indexArray;

		public CityArrayTour(int cityNum) {
			this.cityArray = new int[cityNum];
			this.indexArray = new int[cityNum];
			for (int i = 0; i < this.cityArray.Length; i++) {
				this.cityArray[i] = i;
				this.indexArray[i] = i;
			}

			// フィッシャー法を用いて配列をシャッフル
 			for (int i = this.cityArray.Length - 1; i > 0; i--) {
				int j = SRandom.Instance.NextInt(i + 1);
				Common.Swap(ref indexArray[cityArray[i]], ref indexArray[cityArray[j]]);
				Common.Swap(ref cityArray[i], ref cityArray[j]);
			}
		}

		/// <summary>
		/// ある都市の次に来る都市を返す
		/// </summary>
		override public int NextID(int city) {
			// cityの次のインデックスの都市を返す
			// ただし、インデックスが配列の長さを超えた場合は0に戻す
			if (this.indexArray[city] < this.cityArray.Length - 1) {
				return this.cityArray[this.indexArray[city] + 1];
			}
			return this.cityArray[0];
		}

		/// <summary>
		/// ある都市の前にある都市を返す
		/// </summary>
		override public int PrevID(int city) {
			// cityの前のインデックスの都市を返す
			// だだし、インデックスが0を割った場合は配列の長さ-1とする
			if (this.indexArray[city] > 0) {
				return this.cityArray[this.indexArray[city] - 1];
			}
			return this.cityArray[this.cityArray.Length - 1];
		}

		/// <summary>
		/// エッジの入れ替えに相当する配列の反転を行う
		/// </summary>
		override public void Flip(int va, int vb, int vc, int vd) {
			// va - vb      va   vb
			//          ->     X     と考える
			// vc - vd      vc   vd
			int ia = this.indexArray[va], ib = this.indexArray[vb],
				ic = this.indexArray[vc], id = this.indexArray[vd];

			// va,vd間と、vb,vc間のインデックスの距離を計算
			int length_ad = (ia - id);
			if (length_ad < 0) { length_ad += this.cityArray.Length; }
			int length_cb = (ic - ib);
			if (length_cb < 0) { length_cb += this.cityArray.Length; }

			// インデックスが近い組を反転位置とする
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

			// 決められた節点間で反転を行う
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
		/// 都市を訪問順に並べた配列を返す
		/// </summary>
		override public int[] GetTour() {
			return this.cityArray;
		}

		/// <summary>
		/// デバッグ用ログ
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
