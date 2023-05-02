using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This script handles the move data for the chess pieces, 
public class MoveData
{
    public TileData firstPosition = null;
    public TileData secondPosition = null;
    public ChessPiece pieceMoved = null;
    public ChessPiece pieceKilled = null;
    public int score = int.MinValue;
}
