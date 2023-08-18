using ChessChallenge.API;
using System;


namespace MinMaxClasses
{
    /// <summary>
    /// Implementation of the Minimax algorithm with alpha-beta pruning.
    /// </summary>
    public class MinMaxWithAlphaPruning
    {

        /// <summary>
        /// Returns the best move for the current player. 
        /// 
        /// </summary>
        /// <param name="board">The current board</param>
        /// <param name="depth">The depth of the search (Must be greater than 0)</param>
        /// <returns>The best move </returns>
        public Move FindBestMove(Board board, int depth)
        {

            int bestEvaluation = int.MinValue;
            Move bestMove = Move.NullMove;
            int alpha = int.MinValue;
            int beta = int.MaxValue;

            // use GetLegalMovesNonAlloc(ref moves, bool capturesOnly) to avoid allocating a new array every time.
            // see https://seblague.github.io/chess-coding-challenge/documentation/ 
            Span<Move> legalMoves = stackalloc Move[256];
            board.GetLegalMovesNonAlloc(ref legalMoves, capturesOnly: false);

            // Loop through all legal moves.
            foreach (Move move in legalMoves)
            {
                // Make the move.
                board.MakeMove(move);

                // Evaluate the board with the Minimax algorithm.
                int evaluation = AlphaBeta(board, depth - 1, alpha, beta, board.IsWhiteToMove);

                // TODO:(discussion) What to do with equal evals?
                if (evaluation > bestEvaluation)
                {
                    bestEvaluation = evaluation;
                    bestMove = move;
                }

                // Undo the move.
                board.UndoMove(move);
            }

            return bestMove;
        }




        /// <summary>
        /// The Minimax algorithm with alpha-beta pruning
        /// The algorithm is based on the pseudocode from https://en.wikipedia.org/wiki/Alpha%E2%80%93beta_pruning
        /// 
        /// </summary>
        /// <param name="board">The board</param>
        /// <param name="depth">The depth</param>
        /// <param name="alpha">The alpha value</param>
        /// <param name="beta">The beta value</param>
        /// <param name="maximizingPlayer">Is the maximizing player</param>
        /// <returns>The evaluation of the board at the depth</returns>
        private int AlphaBeta(Board board, int depth, int alpha, int beta, bool maximizingPlayer)
        {

            Span<Move> legalMoves = stackalloc Move[256];
            board.GetLegalMovesNonAlloc(ref legalMoves, capturesOnly: false);

            // If the depth is 0, return the evaluation of the board.
            if (depth == 0) return new Evaluation().Evaluate(board);

            if (maximizingPlayer)
            {
                int value = int.MinValue;

                // If the game is over, return the minimum evaluation since we don't want to lose/draw.
                if (board.IsInCheckmate() || board.IsDraw()) return value;


                foreach (Move move in legalMoves)
                {
                    board.MakeMove(move);

                    value = Math.Max(value, AlphaBeta(board, depth - 1, alpha, beta, false));

                    alpha = Math.Max(alpha, value);

                    board.UndoMove(move);

                    if (value >= beta)
                        break;
                }

                return value;
            }
            else
            {
                int value = int.MaxValue;

                // If the game is over, return the maximum value beause the opponent doesn't want to lose/draw.
                if (board.IsInCheckmate() || board.IsDraw()) return value;

                foreach (Move move in legalMoves)
                {
                    board.MakeMove(move);

                    value = Math.Min(value, AlphaBeta(board, depth - 1, alpha, beta, true));
                    beta = Math.Min(beta, value);
                    board.UndoMove(move);

                    if (value <= alpha)
                        break;
                }

                return value;
            }
        }

    }

    /// <summary>
    /// Temporary evaluation function.
    /// </summary>
    public class Evaluation
    {
        /// <summary>
        /// Evaluates the board and returns a score.
        /// Wack implementation for now.
        /// </summary>
        /// <param name="board"></param>
        /// <returns></returns>
        public int Evaluate(Board board)
        {
            PieceList[] pieceList = board.GetAllPieceLists();

            // Piece values: null, pawn, knight, bishop, rook, queen, king
            int[] pieceValues = { 0, 100, 300, 300, 500, 900, 0 };

            int pawns = pieceList[0].Count * pieceValues[1];
            int knights = pieceList[1].Count * pieceValues[2];
            int bishops = pieceList[2].Count * pieceValues[3];
            int rooks = pieceList[3].Count * pieceValues[4];
            int queens = pieceList[4].Count * pieceValues[5];
            int kings = pieceList[5].Count * pieceValues[6];
            int whiteScore = pawns + knights + bishops + rooks + queens + kings;


            int bpawns = pieceList[6].Count * pieceValues[1];
            int bknights = pieceList[7].Count * pieceValues[2];
            int bbishops = pieceList[8].Count * pieceValues[3];
            int brooks = pieceList[9].Count * pieceValues[4];
            int bqueens = pieceList[10].Count * pieceValues[5];
            int bkings = pieceList[11].Count * pieceValues[6];
            int blackScore = bpawns + bknights + bbishops + brooks + bqueens + bkings;

            int score;
            score = whiteScore - blackScore;

            return score;
        }
    }
}
