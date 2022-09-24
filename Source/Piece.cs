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
			10000
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

		// ========================================================================
		// Piece representation. Pieces are represented as integers consisting of 
		// 8 significant bits. The format for a piece's bits is as follows:
		// 
		// Bit 1:		Exists
		// Bits 2-4:	Piece Type
		// Bit 5:		White / Black
		// Bit 6:		Has Moved
		// ========================================================================
		public static int Empty = 0;

		public static int Exists = 1;
		private static int existsBitIndex = 0;
		
		public static int Pawn = 0;
		public static int Knight = 1;
		public static int Bishop = 2;
		public static int Rook = 3;
		public static int Queen = 4;
		public static int King = 5;
		private static int typeMask = 7;
		private static int typeBitIndex = 1;

		public static int White = 0;
		public static int Black = 1;
		private static int colorMask = 1;
		private static int colorBitIndex = 4;

		public static int HasMoved = 1;
		private static int hasMovedBitIndex = 5;

		public static bool GetExists(int piece)
		{
			return ((piece >> Piece.existsBitIndex) & Piece.Exists) == 1;
		}

		public static int SetExists(int piece, bool value)
		{
			int x = value ? 1 : 0;
			piece &= ~(1 << Piece.existsBitIndex);
			piece |= (x << Piece.existsBitIndex);
			return piece;
		}

		public static int GetType(int piece)
		{
			return (piece >> Piece.typeBitIndex) & Piece.typeMask;
		}

		public static int SetType(int piece, int value)
		{
			piece &= ~(Piece.typeMask << Piece.typeBitIndex);
			piece |= (value << Piece.typeBitIndex);
			return piece;
		}

		public static int GetColor(int piece)
		{
			return (piece >> Piece.colorBitIndex) & Piece.colorMask;
		}

		public static int SetColor(int piece, int value)
		{
			piece &= ~(1 << Piece.colorBitIndex);
			piece |= (value << Piece.colorBitIndex);
			return piece;
		}

		public static int GetOppositeColor(int piece)
		{
			return Piece.GetColor(piece) == Piece.White ? Piece.Black : Piece.White;
		}

		public static bool GetHasMoved(int piece)
		{
			return ((piece >> Piece.hasMovedBitIndex) & Piece.HasMoved) == 1;
		}

		public static int SetHasMoved(int piece, bool value)
		{
			int x = value ? 1 : 0;
			piece &= ~(1 << Piece.hasMovedBitIndex);
			piece |= (x << Piece.hasMovedBitIndex);
			return piece;
		}

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
