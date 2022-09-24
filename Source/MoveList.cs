using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rune
{
	public class MoveList
	{
		private Move[] _moves = new Move[200];
		public int Length = 0;

		public void Insert(Move move)
		{
			int low = 0;
			int high = this.Length;

			int mid = 0;
			while (low < high)
			{
				mid = (low + high) / 2;

				if (this._moves[mid].Priority > move.Priority)
				{
					low = mid + 1;
				}
				else
				{
					high = mid;
				}

			}

			for (int i = this.Length; i > low; i--)
			{
				this._moves[i] = this._moves[i - 1];
			}
			this._moves[low] = move;
			this.Length++;
		}

		public Move this[int i]
		{
			get
			{
				return this._moves[i];
			}
		}
	}
}
