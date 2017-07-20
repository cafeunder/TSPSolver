using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TSPSolver.solver;

namespace TSPSolver.solver.util {
	/// <summary>
	/// 近傍リストを表すクラス
	/// </summary>
	public class InverseNeighborList {
		// インスタンスの次元数
		private int dimension;
		// 近傍数
		public int NeighborNum { get; private set; }
		/// <summary>
		/// NeighborNodes[i] (int[]) : インデックスiに対応する都市をk近傍以内に持つ都市の配列
		/// </summary>
		public int[][] InvNeighborNodes { get; private set; }

		// デフォルトコンストラクタ
		public InverseNeighborList() { }
		
		/// <summary>
		/// Initializeを呼び出すコンストラクタ
		/// </summary>
		public InverseNeighborList(NeighborList neighborList) {
			this.Initialize(neighborList);
		}

		/// <summary>
		/// 逆近傍リストを作る
		/// </summary>
		public void Initialize(NeighborList neighborList) {
			this.dimension = neighborList.NeighborNodes.Length;
			this.NeighborNum = neighborList.NeighborNum;

			// 配列の初期化
			int[] elm_num = new int[this.dimension];
			this.InvNeighborNodes = new int[this.dimension][];
			for (int i = 0; i < this.dimension; i++) {
				this.InvNeighborNodes[i] = new int[this.NeighborNum * this.NeighborNum];
			}

			// 近傍リストを見て逆近傍リストを埋める
			for (int i = 0; i < dimension; i++) {
				for (int j = 0; j < this.NeighborNum; j++) {
					int v = neighborList.NeighborNodes[i][j];
					// iのj近傍がv -> vをj近傍に持つ都市i
					this.InvNeighborNodes[v][elm_num[v]] = i;
					elm_num[v]++;
					if (elm_num[v] < this.NeighborNum * this.NeighborNum) {
						this.InvNeighborNodes[v][elm_num[v]] = -1;
					}
				}
			}
		}

		/// <summary>
		/// ノードvをk近傍以内に持つノード全てをselectNodeListに追加する
		/// </summary>
		/// <param name="selectNodeList">選択候補リスト</param>
		/// <param name="v">ノード</param>
		public void AddNearestNodes(SelectNodeList selectNodeList, int v) {
			for (int i = 0; i < this.InvNeighborNodes[v].Length && this.InvNeighborNodes[v][i] != -1; i++) {
				selectNodeList.Add(this.InvNeighborNodes[v][i]);
			}
		}
	}
}
