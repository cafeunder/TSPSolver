using System;
using System.Collections.Generic;
using System.Text;

namespace TSPSolver.solver {
	/// <summary>
	/// TSPソルバを表す抽象クラス
	/// </summary>
	public abstract class Solver {
		abstract public int[] Run(TSPInstance instance);
	}
}
