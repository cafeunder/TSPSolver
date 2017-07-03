
namespace TSPSolver.solver {
	/// <summary>
	/// 選択候補リストを実装するクラス
	/// </summary>
	public class SelectNodeList {
		// 要素を格納する配列
		private int[] elmArray;
		// 要素のインデックスを格納する配列
		private int[] indexArray;
		// 要素が配列中に存在するかどうかのフラグを管理する配列
		private bool[] existArray;
		// 配列のサイズ
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
		/// 要素を追加する
		/// </summary>
		public void Add(int elm) {
			if (this.existArray[elm]) { return; }
			this.existArray[elm] = true;
			this.elmArray[this.Size] = elm;
			this.indexArray[elm] = this.Size;
			this.Size++;
		}

		/// <summary>
		/// 要素を削除する
		/// </summary>
		/// <param name="elm"></param>
		public void Remove(int elm) {
			if (!this.existArray[elm]) { return; }
			this.existArray[elm] = false;
			//末尾の位置を、削除する要素の位置で更新
			this.indexArray[this.elmArray[this.Size - 1]] = this.indexArray[elm];
			//削除した要素が存在するインデックスに、末尾を追加
			this.elmArray[this.indexArray[elm]] = this.elmArray[this.Size - 1];
			this.Size--;
		}

		/// <summary>
		/// 配列中の要素をランダムに取得する
		/// </summary>
		public int GetRand() {
			return this.elmArray[SRandom.Instance.NextInt(this.Size)];
		}
	}
}
