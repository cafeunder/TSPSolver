using System;
using System.Collections.Generic;
using System.Text;

namespace TSPSolver.solver {
	/// <summary>
	/// TSP�\���o��\�����ۃN���X
	/// </summary>
	public abstract class Solver {
		abstract public int[] Run(TSPInstance instance);
	}
}
