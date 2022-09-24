using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rune
{
	public static class Piece
	{
		public static int[] Values = new int[] 
		{
			1,
			3,
			3,
			5,
			9,
			10000,
			0
		};

		public static bool[] Slides = new bool[]
		{
			false, /* 0 - Pawn - does not slide */
			false, /* 1 - Knight - does not slide */
			true, /* 2 - Bishop - does not matter */
			true, /* 3 - Rook - does not matter */
			true, /* 4 - Queen - does not matter */
			false, /* 5 - King - does not matter */
		};

		public static int[][] Offsets = new int[][]
		{
			new int[] { 0, 0, 0, 0, 0, 0, 0, 0 }, /* 0 - Pawn - handle moves separately */
			new int[] { -21, -19,-12, -8, 8, 12, 19, 21 }, /* 1 - Knight */
			new int[] { -11,  -9,  9, 11, 0,  0,  0,  0 }, /* 2 - Bishop */
			new int[] { -10,  -1,  1, 10, 0,  0,  0,  0 }, /* 3 - Rook */
			new int[] { -11, -10, -9, -1, 1,  9, 10, 11 }, /* 4 - Queen */
			new int[] { -11, -10, -9, -1, 1,  9, 10, 11 }, /* 5 - King */
		};

		public static int White = 1;
		public static int Black = 2;
		
		public static int Pawn = 0;
		public static int Knight = 1;
		public static int Bishop = 2;
		public static int Rook = 3;
		public static int Queen = 4;
		public static int King = 5;
		public static int Empty = 6;

		public static int ColorFromChar(char c)
		{
			if (c == char.ToLower(c))
			{
				return Piece.Black;
			}
			else if (c == char.ToUpper(c))
			{
				return Piece.White;
			}
			else
			{
				throw new Exception();
			}
		}

		public static int TypeFromChar(char c)
		{
			switch (char.ToLower(c))
			{
				case 'p':
					return Piece.Pawn;
				case 'n':
					return Piece.Knight;
				case 'b':
					return Piece.Bishop;
				case 'r':
					return Piece.Rook;
				case 'q':
					return Piece.Queen;
				case 'k':
					return Piece.King;
				default:
					throw new Exception();
			}
		}
	}
}
