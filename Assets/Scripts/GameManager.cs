using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Handles the Main Game Logic, 
public enum PlayerTeam //Manages the Teams in the Game
{
    NONE = -1,
    WHITE,
    BLACK,
};

public class GameManager : MonoBehaviour
{
    Minimax minimax;
    BoardManager board;
    public PlayerTeam playerTurn;
    bool kingDead = false;
    public GameObject fromHighlight;
    public GameObject toHighlight;

    private static GameManager instance;    
    public static GameManager Instance
    {
        get { return instance; }
    }
    private bool isCoroutineExecuting = false;

    private void Awake()
    {
        if (instance == null)        
            instance = this;        
        else if (instance != this)        
            Destroy(this);    
    }    

    void Start()
    {
        minimax = Minimax.Instance;
        board = BoardManager.Instance;        
        board.SetupBoard();
        
    }

    private void Update()
    {
        StartCoroutine(DoAIMove());
    }

    IEnumerator DoAIMove()
{       
    if(isCoroutineExecuting) //Break out of the coroutine if it is already running
        yield break;

    isCoroutineExecuting = true;

    if (kingDead)              //If the king is dead then the game is over and the winner is declared      
        Debug.Log(playerTurn + " wins!");        
    else if (!kingDead)
    {                     
        
        MoveData move = minimax.GetMove(); //Use minimax to calculate the next move
        RemoveObject("Highlight");
        ShowMove(move); //show the target pos

        yield return new WaitForSeconds(1);

        SwapPieces(move);  
        if(!kingDead)                
            UpdateTurn();     

        isCoroutineExecuting = false;                                                                                                         
    }
}

    public void SwapPieces(MoveData move)
    {
        TileData firstTile = move.firstPosition;
        TileData secondTile = move.secondPosition;        

        firstTile.CurrentPiece.MovePiece(new Vector2(secondTile.Position.x, secondTile.Position.y));

        CheckDeath(secondTile); //Checks if the king dies when the piece has been moved
                        
        secondTile.CurrentPiece = move.pieceMoved;
        firstTile.CurrentPiece = null;
        secondTile.CurrentPiece.chessPosition = secondTile.Position;
        secondTile.CurrentPiece.HasMoved = true;            
    }   

    private void UpdateTurn()
    {     
        playerTurn = playerTurn == PlayerTeam.WHITE ? PlayerTeam.BLACK : PlayerTeam.WHITE;        
    }

    void CheckDeath(TileData _secondTile)
    {
        if (_secondTile.CurrentPiece != null)        
            if (_secondTile.CurrentPiece.Type == ChessPiece.PieceType.KING)           
                kingDead = true;                           
            else
                Destroy(_secondTile.CurrentPiece.gameObject);        
    }

    void ShowMove(MoveData move)
    {
        GameObject GOfrom = Instantiate(fromHighlight); //Highlights the current pos
        GOfrom.transform.position = new Vector2(move.firstPosition.Position.x, move.firstPosition.Position.y);
        GOfrom.transform.parent = transform;

        GameObject GOto = Instantiate(toHighlight); //highlights the target pos
        GOto.transform.position = new Vector2(move.secondPosition.Position.x, move.secondPosition.Position.y);
        GOto.transform.parent = transform;
    }

    public void RemoveObject(string text) //removes all game objects that have the inputed tag
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(text);
        foreach (GameObject GO in objects)
            Destroy(GO);        
    }
}
