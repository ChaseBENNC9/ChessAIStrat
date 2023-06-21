
//This Script Manages the board , Sets up the tile data 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    private static BoardManager instance;
    public static BoardManager Instance

    {
        get{ return instance; }
    }



    private const int BOARDSIZE = 8; //The Size of the Board
    private void Awake()
    {
        if (instance == null)        
            instance = this;        
        else if (instance != this)        
            Destroy(this);    
    }  

    private TileData[,] board = new TileData[BOARDSIZE, BOARDSIZE];

    public void SetupBoard()
    {
        for (int y = 0; y < BOARDSIZE; y++)        
            for (int x = 0; x < BOARDSIZE; x++)            
                board[x, y] = new TileData(x, y);                    
    }

    public TileData GetTileFromBoard(Vector2 tile)
    {
        return board[(int)tile.x, (int)tile.y];
    }
}
