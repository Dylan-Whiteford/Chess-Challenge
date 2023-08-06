using ChessChallenge.API;
using System;
using System.Diagnostics;

public class MyBot : IChessBot
{
    public Move Think(Board board, Timer timer)
    {

        MinMax minmax = new();
        Random rng = new();
        Move[] moves = board.GetLegalMoves();
        Move moveToPlay = moves[0];
        int bestEval=0;
        foreach(Move move in moves) {
            board.MakeMove(move);
            int evaluation = minmax.alphabeta(board, 5, true);

            //Console.Write(evaluation.ToString());
            
            if(evaluation > bestEval) moveToPlay = move;
            board.UndoMove(move);
            
        }
        return moveToPlay;
    }
}