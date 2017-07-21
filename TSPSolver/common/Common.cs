using System;
using System.Collections.Generic;
using System.Text;

namespace TSPSolver.common {
	public class Common {
		/// <summary>
		/// 参照渡しで受け取ったa,bの値を入れ替える
		/// </summary>
		public static void Swap(ref int a, ref int b) {
			int temp = a;
			a = b;
			b = temp;
		}
	}
}
