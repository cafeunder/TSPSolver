
using System;
using System.IO;

namespace TSPSolver.structure {
	/// <summary>
	/// TSP�̃c�A�[��\���N���X
	/// </summary>
	public abstract class Tour {
		/// <summary>
		/// ����s�s�̎��ɗ���s�s��Ԃ�
		/// </summary>
		abstract public int NextID(int city);

		/// <summary>
		/// ����s�s�̑O�ɂ���s�s��Ԃ�
		/// </summary>
		abstract public int PrevID(int city);
		
		/// <summary>
		/// �G�b�W�̓���ւ��ɑ�������z��̔��]���s��
		/// </summary>
		abstract public void Flip(int va, int vb, int vc, int vd);

		/// <summary>
		/// �s�s��K�⏇�ɕ��ׂ��z���Ԃ�
		/// </summary>
		abstract public int[] GetTour();

		/// <summary>
		/// �I�[�o�[���C�h����ToString���\�b�h
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
		/// �c�A�[�t�@�C���ɏo�͂���
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
