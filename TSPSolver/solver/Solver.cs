using TSPSolver.common;

namespace TSPSolver.solver {
	/// <summary>
	/// TSPソルバを表す抽象クラス
	/// </summary>
	public abstract class Solver {
		abstract public (int, int[]) Run(TSPInstance instance);
	}
}
