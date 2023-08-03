using ChessChallenge.API;
using System;
public class MyBot : IChessBot
{
    public Move Think(Board board, Timer timer)
    {

        Random rng = new();
        Move[] moves = board.GetLegalMoves();
        Move moveToPlay = moves[rng.Next(moves.Length)];
        return moveToPlay;
    }
}