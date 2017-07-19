
using System;

namespace TSPSolver.common {
	/// <summary>
	/// シングルトン実装で使いまわすSystem.Random
	/// </summary>
	public class SRandom {
		private Random rand;

		private SRandom() {
			this.rand = new Random();
		}
		private SRandom(int seed) {
			this.rand = new Random(seed);
		}

		/// <summary>
		/// 0からmax-1までの整数をランダムに返す
		/// </summary>
		/// <param name="max">乱数で得る値の最大値+1</param>
		public int NextInt(int max) {
			return this.rand.Next(max);
		}


		//=== シングルトン実装 ===//
		private static SRandom instance = new SRandom();
		public static SRandom Instance {
			get {
				return SRandom.instance;
			}
		}

		/// <summary>
		/// シードを指定して乱数生成器を生成しなおす
		/// </summary>
		/// <param name="seed">シード値</param>
		public static void Intialize(int seed) {
			SRandom.instance = new SRandom(seed);
		}
	}
}
