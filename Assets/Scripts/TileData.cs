﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//This script handles the Tile Data and determines each piece that is currently on each tile of the board.
public class TileData
{
    private Vector2 position = Vector2.zero;
    public Vector2 Position
    {
        get{ return position; }
    }

    private ChessPiece currentPiece = null;
    public ChessPiece CurrentPiece
    {
        get{ return currentPiece; }
        set{ currentPiece = value; }
    }

    public TileData(int x, int y) //Constructor of the tiledata class
    {
        position.x = x;
        position.y = y;

        if (y == 0 || y == 1 || y == 6 || y == 7)      
            currentPiece = GameObject.Find("[" + x.ToString() + "," + y.ToString() + "]").GetComponent<ChessPiece>(); //sets the current peice on the tile if there is any    
    }

    public void SwapFakePieces(ChessPiece newPiece)
    {
        currentPiece = newPiece; //Sets the current piece of the tile to the specified one, this is used to perform a fake Move
    }
}
