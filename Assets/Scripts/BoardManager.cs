
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
        if (instance == null)      //Ensures there is only one Boardmanager in the game, destroys any others  
            instance = this;        
        else if (instance != this)        
            Destroy(this);    
    }  

    private TileData[,] board = new TileData[BOARDSIZE, BOARDSIZE]; //2d array that holds the information of each tile in the board

    public void SetupBoard() //Sets up the board, each tile is looped through and assigned into the array
    {
        for (int y = 0; y < BOARDSIZE; y++)        
            for (int x = 0; x < BOARDSIZE; x++)            
                board[x, y] = new TileData(x, y);                    
    }

    public TileData GetTileFromBoard(Vector2 tile) //Returns the tile at the specific position
    {
        return board[(int)tile.x, (int)tile.y];
    }
}
