using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Rune
{
	public class Board
	{
		private int[] mailbox120 = new int[]
		{
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1,  0,  1,  2,  3,  4,  5,  6,  7, -1,
			-1,  8,  9, 10, 11, 12, 13, 14, 15, -1,
			-1, 16, 17, 18, 19, 20, 21, 22, 23, -1,
			-1, 24, 25, 26, 27, 28, 29, 30, 31, -1,
			-1, 32, 33, 34, 35, 36, 37, 38, 39, -1,
			-1, 40, 41, 42, 43, 44, 45, 46, 47, -1,
			-1, 48, 49, 50, 51, 52, 53, 54, 55, -1,
			-1, 56, 57, 58, 59, 60, 61, 62, 63, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
			-1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
		};

		private int[] mailbox64 = new int[]
		{
			21, 22, 23, 24, 25, 26, 27, 28,
			31, 32, 33, 34, 35, 36, 37, 38,
			41, 42, 43, 44, 45, 46, 47, 48,
			51, 52, 53, 54, 55, 56, 57, 58,
			61, 62, 63, 64, 65, 66, 67, 68,
			71, 72, 73, 74, 75, 76, 77, 78,
			81, 82, 83, 84, 85, 86, 87, 88,
			91, 92, 93, 94, 95, 96, 97, 98
		};

		private int[] piece = new int[64];

		private int[] color = new int[64];

		private int[] movesMade = new int[64];

		private Stack<int> capturedPieces = new Stack<int>(32);

		private Stack<int> capturedColors = new Stack<int>(32);

		public static Board FromFen(string fen)
		{
			Board newBoard = new Board();
			string[] ranks = fen.Split('/');
			for (int rank = 0; rank < ranks.Length; rank++)
			{
				string rankString = ranks[rank];
				int file = 0;
				for (int rankItem = 0; rankItem < rankString.Length; rankItem++)
				{
					if (int.TryParse(rankString[rankItem].ToString(), out int numEmpty))
					{
						for (int i = 0; i < numEmpty; i++)
						{
							int square = Board.TranslateRankAndFileToSquare(rank, file);
							newBoard.piece[square] = Piece.Empty;
							newBoard.color[square] = Piece.Empty;
							file++;
						}
					}
					else
					{
						int square = Board.TranslateRankAndFileToSquare(rank, file);
						newBoard.piece[square] = Piece.TypeFromChar(rankString[rankItem]);
						newBoard.color[square] = Piece.ColorFromChar(rankString[rankItem]);
						file++;
					}
				}
			}
			return newBoard;
		}

		// ========================================
		// Evaluation
		// ========================================
		public float Evaluate()
		{
			float whiteScore = 0;
			float blackScore = 0;
			for (int i = 0; i < this.piece.Length; i++)
			{
				if (this.color[i] == Piece.White)
				{
					whiteScore += Piece.Values[this.piece[i]];
				}
				else if (this.color[i] == Piece.Black)
				{
					blackScore += Piece.Values[this.piece[i]];
				}
			}
			return whiteScore - blackScore;
		}

		// ========================================
		// Move Making
		// ========================================
		public void MakeMove(Move move)
		{
			if (move.IsCapture)
			{
				this.capturedPieces.Push(this.piece[move.TargetPosition]);
				this.capturedColors.Push(this.color[move.TargetPosition]);
			}
			this.piece[move.TargetPosition] = this.piece[move.StartingPosition];
			this.color[move.TargetPosition] = this.color[move.StartingPosition];
			this.piece[move.StartingPosition] = Piece.Empty;
			this.color[move.StartingPosition] = Piece.Empty;
			this.movesMade[move.TargetPosition]++;
		}

		public void UnmakeMove(Move move)
		{
			this.piece[move.StartingPosition] = this.piece[move.TargetPosition];
			this.color[move.StartingPosition] = this.color[move.TargetPosition];
			if (move.IsCapture)
			{
				this.piece[move.TargetPosition] = this.capturedPieces.Pop();
				this.color[move.TargetPosition] = this.capturedColors.Pop();
			}
			else
			{
				this.piece[move.TargetPosition] = Piece.Empty;
				this.color[move.TargetPosition] = Piece.Empty;
			}
			this.movesMade[move.TargetPosition]--;
		}

		public bool IsMoveLegal(Move move)
		{
			int side = this.color[move.StartingPosition];
			int xside = side == Piece.White ? Piece.Black : Piece.White;
			this.MakeMove(move);
			MoveList opposingMoves = this.GetAllMovesForColor(xside);
			for (int i = 0; i < opposingMoves.Length; i++)
			{
				if (this.piece[opposingMoves[i].TargetPosition] == Piece.King)
				{
					this.UnmakeMove(move);
					return false;
				}
			}
			this.UnmakeMove(move);
			return true;
		}

		// ======================
		// Helpers
		// ======================
		public static int TranslateRankAndFileToSquare(int rank, int file)
		{
			return file + (rank * 8);
		}

		public void Print()
		{
			Console.WriteLine("===============");
			string rankString = string.Empty;
			for (int rank = 0; rank < 8; rank++)
			{
				for (int file = 0; file < 8; file++)
				{
					int square = Board.TranslateRankAndFileToSquare(rank, file);
					Console.Write(this.color[square]);
					Console.Write(this.piece[square]);
					Console.Write(' ');
				}
				Console.WriteLine();
			}
			Console.WriteLine("===============");
		}

		// ===============================
		// Move Gen
		// ===============================
		public MoveList GetAllLegalMovesForColor(int side)
		{
			MoveList pseudoLegalMoves = this.GetAllMovesForColor(side);
			MoveList legalMoves = new MoveList();
			for (int i = 0; i < pseudoLegalMoves.Length; i++)
			{
				Move move = pseudoLegalMoves[i];
				if (this.IsMoveLegal(move))
				{
					legalMoves.Insert(move);
				}
			}
			return legalMoves;
		}

		public MoveList GetAllMovesForColor(int side)
		{
			int xside = side == Piece.White ? Piece.Black : Piece.White;
			int pawnYMultiplier = side - xside;
			MoveList allMoves = new MoveList();
			for (int startSquare = 0; startSquare < 64; startSquare++) // For each square in the board
			{
				if (this.color[startSquare] == side) // If the current square is the moving color
				{
					int piece = this.piece[startSquare]; // Get piece at square
					if (piece != Piece.Pawn) // Pawns have a weird moveset, so we do them separately
					{
						for (int offsetIndex = 0; offsetIndex < Piece.Offsets[piece].Length; offsetIndex++)
						{
							if (Piece.Offsets[piece][offsetIndex] == 0) { break; } // There are no more offsets for this piece to traverse
							for (int targetSquare = startSquare;;)
							{
								// mailbox64 gives us the the target square as a mailbox120 index. We add the offset to that index
								// which is already in mailbox120 format. If the number at that mailbox120 index is a -1, then we know
								// that we are out of bounds, so we simply break because we cannot move to that offset / in that direction.
								targetSquare = this.mailbox120[this.mailbox64[targetSquare] + Piece.Offsets[piece][offsetIndex]];
								if (targetSquare == -1) { break; }
								if (this.piece[targetSquare] != Piece.Empty)
								{
									if (this.color[targetSquare] == xside)
									{
										Move captureMove = new Move();
										captureMove.StartingPosition = startSquare;
										captureMove.TargetPosition = targetSquare;
										captureMove.Priority = Piece.Values[this.piece[targetSquare]];
										captureMove.IsCapture = true;
										allMoves.Insert(captureMove);
									}
									break;
								}
								Move move = new Move();
								move.StartingPosition = startSquare;
								move.TargetPosition = targetSquare;
								move.Priority = 0;
								allMoves.Insert(move);
								// If the piece is non-sliding, then we break here, because we no longer need to look in that direction.
								// Otherwise, we continue with this loop, and keep incrememnting target square by the same offset
								// until we can slide no longer.
								if (!Piece.Slides[piece]) { break; }
							}
						}
					}
					else // Pawn Moves
					{
						// check pushes first
						int targetSquare = this.mailbox120[this.mailbox64[startSquare] + (10 * pawnYMultiplier)];
						if (targetSquare != -1 && this.piece[targetSquare] == Piece.Empty)
						{
							Move move = new Move();
							move.StartingPosition = startSquare;
							move.TargetPosition = targetSquare;
							move.Priority = 0;
							allMoves.Insert(move);
							targetSquare = this.mailbox120[this.mailbox64[targetSquare] + (10 * pawnYMultiplier)];
							if (this.movesMade[startSquare] == 0 && targetSquare != -1 && this.piece[targetSquare] == Piece.Empty)
							{
								move = new Move();
								move.StartingPosition = startSquare;
								move.TargetPosition = targetSquare;
								move.Priority = 0;
								allMoves.Insert(move);
							}
						}
						// Now check pawn attacks
						targetSquare = this.mailbox120[this.mailbox64[startSquare] + (11 * pawnYMultiplier)];
						if (targetSquare != -1 && this.color[targetSquare] == xside)
						{
							Move move = new Move();
							move.StartingPosition = startSquare;
							move.TargetPosition = targetSquare;
							move.Priority = Piece.Values[this.piece[targetSquare]];
							move.IsCapture = true;
						}

						targetSquare = this.mailbox120[this.mailbox64[startSquare] + (9 * pawnYMultiplier)];
						if (targetSquare != -1 && this.color[targetSquare] == xside)
						{
							Move move = new Move();
							move.StartingPosition = startSquare;
							move.TargetPosition = targetSquare;
							move.Priority = Piece.Values[this.piece[targetSquare]];
							move.IsCapture = true;
						}
					}
				}
			}
			return allMoves;
		}
	}
}
