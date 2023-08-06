
using System;
using ChessChallenge.API;
public class MinMax
{

    const int infinity = 69;
    const int alpha = -infinity;
    const int beta = infinity;


    public int alphabeta(Board position, int depth, bool maximizingPlayer) {


        return _alphabeta(position, depth, alpha, beta, maximizingPlayer);
    }

    public int _alphabeta(Board position, int depth, int a, int b, bool maximizingPlayer) {
        
        Span<Move> legalMoves = stackalloc Move[256];
        position.GetLegalMovesNonAlloc(ref legalMoves);

        if(depth == 0 ) {
            // TODO: HEURISTIC FUNCTION
            Random rng = new();
            return rng.Next(138)-69;
        }
            
        if(maximizingPlayer) {
            int value = -infinity;
            foreach(Move move in legalMoves) {
                position.MakeMove(move);
                value = Math.Max(value, _alphabeta(position, depth - 1, a, b, false));
                if (value > b) {
                    position.UndoMove(move);
                    break;
                }
                a = Math.Max(a, value);
                position.UndoMove(move);
            }
            return value;
        }
        else{
            int value = infinity;

            foreach(Move move in legalMoves) {
                position.MakeMove(move);
                value = Math.Min(value, _alphabeta(position, depth - 1, a, b, true));
                if (value < a) {
                    position.UndoMove(move);
                    break;
                }
                b = Math.Min(b, value);
                position.UndoMove(move);
            }
            return value;
        }
    }
}
// Psuedo Code
//
// function alphabeta(node, depth, α, β, maximizingPlayer) is
//     if depth == 0 or node is terminal then
//         return the heuristic value of node
//     if maximizingPlayer then
//         value := −∞
//         for each child of node do
//             value := max(value, alphabeta(child, depth − 1, α, β, FALSE))
//             if value > β then
//                 break (* β cutoff *)
//             α := max(α, value)
//         return value
//     else
//         value := +∞
//         for each child of node do
//             value := min(value, alphabeta(child, depth − 1, α, β, TRUE))
//             if value < α then
//                 break (* α cutoff *)
//             β := min(β, value)
//         return value