﻿using System;
using ChessChallenge.API;
using MinMaxClasses;

public class MyBot : IChessBot
{

    public Move Think(Board board, Timer timer)
    {

        // AlphaBetaPruning example
        MinMaxWithAlphaPruning minimax = new();
        Move moveToPlay = minimax.FindBestMove(board, 4);

        return moveToPlay;
    }


}


