using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Handles each chess piece holds team, piece type and position information
public class ChessPiece : MonoBehaviour
{
    public enum PieceType //Manages all the types of pieces that can be used
    {
        NONE = -1,
        PAWN,
        BISHOP,
        KNIGHT,
        ROOK,
        QUEEN,
        KING,
    };    

    [SerializeField] private PieceType type = PieceType.NONE;
    [SerializeField] private PlayerTeam team = PlayerTeam.NONE;

    public PieceType Type
    {
        get{ return type; }
    }
    public PlayerTeam Team
    {
        get{ return team; }
    }
    public Vector2 chessPosition; //the current position of the piece
    private Vector2 moveTo; //The Target pos of the piece

    private bool hasMoved = false;
    public bool HasMoved
    {
        get{ return hasMoved; }
        set{ hasMoved = value; }
    }
        
    void Start()
    {
        //Initialize the variables
        transform.position = chessPosition; //the starting position is  set to chessposition
        moveTo = transform.position; //MoveTo is set to the starting pos
    }

    void Update()
    {
        transform.position = moveTo; //Moves the piece to the MoveTo Vector
    }

    public void MovePiece(Vector2 position) //Sets the MoveTo Vector of the piece to the selected position
    {
        moveTo = position;
    }
}
