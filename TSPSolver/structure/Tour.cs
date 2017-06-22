

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

			// フィッシャー法を用いて配列をシャッフル
 			for (int i = this.NodeArray.Length - 1; i > 0; i--) {
				int j = SRandom.Instance.NextInt(i + 1);
				int swap = this.NodeArray[i];
				this.NodeArray[i] = this.NodeArray[j];
				this.NodeArray[j] = swap;

				// this.nodeArray[i]のインデックスは確定したのでインデックス配列に追加
				this.indexArray[this.NodeArray[i]] = i; 
			}
		}

		/// <summary>
		/// あるノードの次に来るノードを返す
		/// </summary>
		public int nextID(int node) {
			// nodeの次のインデックスのノードを返す
			// ただし、インデックスが配列の長さを超えた場合は0に戻す
			return this.NodeArray[(this.indexArray[node] + 1) % this.NodeArray.Length];
		}

		/// <summary>
		/// あるノードの前にあるノードを返す
		/// </summary>
		public int prevID(int node) {
			// nodeの前のインデックスのノードを返す
			// だだし、インデックスが0を割った場合は配列の長さ-1とする
			return this.NodeArray[(this.indexArray[node] + this.NodeArray.Length - 1) % this.NodeArray.Length];
		}

		/// <summary>
		/// エッジの入れ替えに相当する配列の反転を行う
		/// </summary>
		/// <param name="v1">ノード1</param>
		/// <param name="v2">ノード2</param>
		/// <param name="forward">順方向か？</param>
		public void flip(int v1, int v2, bool forward) {
			// va - vb      va   vb
			//          ->     X     と考える
			// vc - vd      vc   vd
			int ia, ib, ic, id;
			if (forward) {
				ia = this.indexArray[v1];
				ib = this.indexArray[this.nextID(v1)];
				ic = this.indexArray[v2];
				id = this.indexArray[this.nextID(v2)];
			} else {
				ia = this.indexArray[this.prevID(v1)];
				ib = this.indexArray[v1];
				ic = this.indexArray[this.prevID(v2)];
				id = this.indexArray[v2];
			}

			// va,vd間と、vb,vc間のインデックスの距離を計算
			int length_ad = (ia - id);
			if (length_ad < 0) { length_ad += this.NodeArray.Length; }
			int length_cb = (ic - ib);
			if (length_cb < 0) { length_cb += this.NodeArray.Length; }

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
				int temp = this.NodeArray[head];
				this.NodeArray[head] = this.NodeArray[tail];
				this.NodeArray[tail] = temp;
				this.indexArray[this.NodeArray[head]] = head;
				this.indexArray[this.NodeArray[tail]] = tail;

				head = (head + 1) % this.NodeArray.Length;
				tail = (tail  + this.NodeArray.Length - 1) % this.NodeArray.Length;
			}
		}

		/// <summary>
		/// オーバーライドしたToStringメソッド
		/// </summary>
		override public string ToString() {
			string result = "";
			foreach (int n in this.NodeArray) {
				result += n + ",";
			}
			return result;
		}

		/// <summary>
		/// デバッグ用ログ
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
