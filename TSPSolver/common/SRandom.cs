
using System;

namespace TSPSolver.common {
	/// <summary>
	/// �V���O���g�������Ŏg���܂킷System.Random
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
		/// 0����max-1�܂ł̐����������_���ɕԂ�
		/// </summary>
		/// <param name="max">�����œ���l�̍ő�l+1</param>
		public int NextInt(int max) {
			return this.rand.Next(max);
		}


		//=== �V���O���g������ ===//
		private static SRandom instance = new SRandom();
		public static SRandom Instance {
			get {
				return SRandom.instance;
			}
		}

		/// <summary>
		/// �V�[�h���w�肵�ė���������𐶐����Ȃ���
		/// </summary>
		/// <param name="seed">�V�[�h�l</param>
		public static void Intialize(int seed) {
			SRandom.instance = new SRandom(seed);
		}
	}
}
