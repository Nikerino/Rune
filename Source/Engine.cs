using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rune
{
	public static class Engine
	{
		private static int maxPlayer = Piece.White;

		private static int minPlayer = Piece.Black;

		private static int lookaheadDepth = 4;

		public static Move GetBestMove(Board board, int side)
		{
			int xside = side == Piece.White ? Piece.Black : Piece.White;
			Move bestMove = new Move();
			float bestScore = side == Engine.maxPlayer ? float.MinValue : float.MaxValue;

			MoveList moves = board.GetAllLegalMovesForColor(side);
			float alpha = float.MinValue;
			float beta = float.MaxValue;
			for (int i = 0; i < moves.Length; i++)
			{ 
				Move move = moves[i];
				board.MakeMove(move);
				float score = Engine.Minimax(board, Engine.lookaheadDepth - 1, xside, alpha, beta);
				board.UnmakeMove();
				if (side == Engine.maxPlayer)
				{
					if (score > bestScore)
					{
						bestScore = score;
						bestMove = move;
					}
					if (bestScore > alpha)
					{
						alpha = bestScore;
					}
				}
				else
				{
					if (score < bestScore)
					{
						bestScore = score;
						bestMove = move;
					}
					if (bestScore < beta)
					{
						beta = bestScore;
					}
				}
			}
			return bestMove;
		}

		public static float Minimax(Board board, int lookaheadDepth, int playerToMove, float alpha, float beta)
		{
			float score = board.Evaluate();
			if (lookaheadDepth == 0 || score < -5000 || score > 5000)
			{
				return score;
			}

			if (playerToMove == Engine.maxPlayer)
			{
				float bestEvaluation = float.MinValue;
				MoveList legalMoves = board.GetAllLegalMovesForColor(playerToMove);
				for (int i = 0; i < legalMoves.Length; i++)
				{ 
					Move move = legalMoves[i];
					board.MakeMove(move);
					float evaluation = Engine.Minimax(board, lookaheadDepth - 1, Engine.minPlayer, alpha, beta);
					board.UnmakeMove();
					if (evaluation > bestEvaluation)
					{ 
						bestEvaluation = evaluation;
					}
					if (bestEvaluation > alpha)
					{
						alpha = bestEvaluation;
					}
					if (beta <= alpha)
					{
						break;
					}
				}
				return bestEvaluation;
			}
			else
			{
				float bestEvaluation = float.MaxValue;
				MoveList legalMoves = board.GetAllLegalMovesForColor(playerToMove);
				for (int i = 0; i < legalMoves.Length; i++)
				{
					Move move = legalMoves[i];
					board.MakeMove(move);
					float evaluation = Engine.Minimax(board, lookaheadDepth - 1, Engine.maxPlayer, alpha, beta);
					board.UnmakeMove();
					if (evaluation < bestEvaluation)
					{
						bestEvaluation = evaluation;
					}
					if (bestEvaluation < beta)
					{
						beta = bestEvaluation;
					}
					if (beta <= alpha)
					{
						break;
					}
				}
				return bestEvaluation;
			}
		}
	}
}
