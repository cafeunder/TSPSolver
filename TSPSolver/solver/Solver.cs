using TSPSolver.common;

namespace TSPSolver.solver {
	/// <summary>
	/// TSP�\���o��\�����ۃN���X
	/// </summary>
	public abstract class Solver {
		abstract public (int, int[]) Run(TSPInstance instance);
	}
}
