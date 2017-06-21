

namespace TSPSolver.structure {
	public class Tour {
		public int[] NodeArray{ get; private set; }
		private int[] orderArray;

		public Tour(int nodeNum) {
			this.NodeArray = new int[nodeNum];
			this.orderArray = new int[nodeNum];
			for (int i = 0; i < this.NodeArray.Length; i++) {
				this.NodeArray[i] = i;
			}

			// フィッシャー法を用いて配列をシャッフル
 			for (int i = this.NodeArray.Length - 1; i > 0; i--) {
				int j = SRandom.Instance.NextInt(i + 1);
				int swap = this.NodeArray[i];
				this.NodeArray[i] = this.NodeArray[j];
				this.NodeArray[j] = swap;

				// this.nodeArray[i]のインデックスは確定したのでオーダー配列に追加
				this.orderArray[this.NodeArray[i]] = i; 
			}
		}

		/// <summary>
		/// あるノードの次に来るノードを返す
		/// </summary>
		public int nextID(int node) {
			// nodeの次のインデックスのノードを返す
			// ただし、インデックスが配列の長さを超えた場合は0に戻す
			return this.NodeArray[(this.orderArray[node] + 1) % this.NodeArray.Length];
		}
	}
}
