
using System;
using System.IO;

namespace TSPSolver.structure {
	/// <summary>
	/// TSPのツアーを表すクラス
	/// </summary>
	public abstract class Tour {
		/// <summary>
		/// ある都市の次に来る都市を返す
		/// </summary>
		abstract public int NextID(int city);

		/// <summary>
		/// ある都市の前にある都市を返す
		/// </summary>
		abstract public int PrevID(int city);
		
		/// <summary>
		/// エッジの入れ替えに相当する配列の反転を行う
		/// </summary>
		abstract public void Flip(int va, int vb, int vc, int vd);

		/// <summary>
		/// 都市を訪問順に並べた配列を返す
		/// </summary>
		abstract public int[] GetTour();

		/// <summary>
		/// オーバーライドしたToStringメソッド
		/// </summary>
		override public string ToString() {
			int[] tour = this.GetTour();
			string result = "";
			foreach (int n in tour) {
				result += n + ",";
			}
			return result;
		}

		/// <summary>
		/// ツアーファイルに出力する
		/// </summary>
		/// <param name="filename"></param>
		public static void WriteTo(string filename, int[] tour) {
			using (StreamWriter bw = new StreamWriter(File.OpenWrite(filename))) {
				bw.WriteLine("TOUR_SECTION");
				foreach (int n in tour) {
					bw.WriteLine(n + 1);
				}
				bw.WriteLine(-1);
			}
		}
	}
}
