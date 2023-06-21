using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This Script handles the Minimax Algorithm for calculating the best outcome
public class Minimax : MonoBehaviour
{
    BoardManager board;
    GameManager gameManager;
    MoveData bestMove;
    int myScore = 0;
    int opponentScore = 0;
    int maxDepth;

    List<TileData> myPieces = new List<TileData>();
    List<TileData> opponentPieces = new List<TileData>();
    Stack<MoveData> moveStack = new Stack<MoveData>();
    MoveHeuristic weight = new MoveHeuristic();

    public static Minimax instance;
    public static Minimax Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)        
            instance = this;        
        else if (instance != this)        
            Destroy(this);    
    }


    public List<T> Shuffle<T>(List<T> list)  //SHuffles the list of moves to avoid repetition 
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = Random.Range(0,n);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
        return list;
    }
    
    MoveData CreateMove(TileData from, TileData to) //Hypothetical move based on where it is and where it is moving to, checks if any peice is killed during the move
    {
        MoveData tempMove = new MoveData
        {
            firstPosition = from,
            pieceMoved = from.CurrentPiece,
            secondPosition = to
        };

        if (to.CurrentPiece != null)        
            tempMove.pieceKilled = to.CurrentPiece;        

        return tempMove;
    }


    List<MoveData> GetMoves(PlayerTeam team) //Finds all available moves for the current player
    {
        List<MoveData> turnMove = new List<MoveData>();
        List<TileData> pieces = (team == gameManager.playerTurn) ? myPieces : opponentPieces;      

        foreach (TileData tile in pieces)
        {
            MoveFunction movement = new MoveFunction(board);
            List<MoveData> pieceMoves = movement.GetMoves(tile.CurrentPiece, tile.Position);

            foreach (MoveData move in pieceMoves)
            {
                MoveData newMove = CreateMove(move.firstPosition, move.secondPosition);
                turnMove.Add(newMove);
            }
        }
        return turnMove;
    }


    void DoFakeMove(TileData currentTile, TileData targetTile) //does a fake move for the algorithm, with a current tile and a target tile
    {
        targetTile.SwapFakePieces(currentTile.CurrentPiece);
        currentTile.CurrentPiece = null;
    }


    void UndoFakeMove() //Reverts the fake move
    {
        MoveData tempMove = moveStack.Pop(); //pop the last move off the movestack
        TileData movedTo = tempMove.secondPosition; //reverses the move by setting the target to the original position
        TileData movedFrom = tempMove.firstPosition;
        ChessPiece pieceKilled = tempMove.pieceKilled;
        ChessPiece pieceMoved = tempMove.pieceMoved;

        movedFrom.CurrentPiece = movedTo.CurrentPiece;
        movedTo.CurrentPiece = (pieceKilled != null) ? pieceKilled : null;      //Restores the peice that would have been killed
    }



    int Evaluate() //return the difference between the player scores
    {
        int pieceDifference = myScore - opponentScore;            
        return pieceDifference;
    }

    void GetBoardState() //Iterates through the current pieces of each player and calculates the current score 
    {
        myPieces.Clear();
        opponentPieces.Clear();
        myScore = 0;
        opponentScore = 0;

        for (int y = 0; y < 8; y++)        
            for (int x = 0; x < 8; x++)
            {
                TileData tile = board.GetTileFromBoard(new Vector2(x, y));
                if(tile.CurrentPiece != null && tile.CurrentPiece.Type != ChessPiece.PieceType.NONE)
                {
                    if (tile.CurrentPiece.Team == gameManager.playerTurn) //checks the piece team  and compares to the current player
                    {
                        myScore += weight.GetPieceWeight(tile.CurrentPiece.Type); //adds the weight to the players score
                        myPieces.Add(tile); //adds the piece to the players piece list
                    }
                    else
                    {
                        opponentScore += weight.GetPieceWeight(tile.CurrentPiece.Type); 
                        opponentPieces.Add(tile);
                    }
                }
            }     
    }



    public MoveData GetMove()
    {
        board = BoardManager.Instance;
        gameManager = GameManager.Instance;
        bestMove = CreateMove(board.GetTileFromBoard(new Vector2(0, 0)), board.GetTileFromBoard(new Vector2(0, 0)));

        maxDepth = 3; //sets the max depth that the algorithm will go to in the move tree
        CalculateMinMax(maxDepth,int.MinValue,int.MaxValue, true); 

        return bestMove;
    } 





    int CalculateMinMax(int depth, int alpha, int beta,  bool max) //the recursive minimax algorithm, with the maximum depth , the alpha and beta values for pruning and whether it will be finding the maximum score or the minimum score
    {
        GetBoardState(); //finds the state of the board

        if (depth == 0)        //when the depth reaches 0 it has reached an end node it will return
            return Evaluate();

        if (max)
        {
            List<MoveData> allMoves = GetMoves(gameManager.playerTurn); //find all the player moves
            allMoves = Shuffle(allMoves); //shuffle the list
            foreach (MoveData move in allMoves)
            {
                moveStack.Push(move); //add each move to the movestack so it  can be reversed

                DoFakeMove(move.firstPosition, move.secondPosition); //Complete a fake move
                int score = CalculateMinMax(depth - 1,int.MinValue,int.MaxValue, false); //calls the method with a decreased depth by 1 and the oppisite value for  the boolean to find the next move
                UndoFakeMove();     //Reverts the fake move       

                if (score > alpha)
                {
                    alpha = score;
                    move.score = score;

                    if (score > bestMove.score && depth == maxDepth)                                                                
                        bestMove = move;                                                            
                }

                if (score >= beta)  //Prunes the branch if the branch wont affect the final decision        
                break;
            }
            return alpha;
        }
        else //For calculating the minimum score
        {
            PlayerTeam opponent = gameManager.playerTurn == PlayerTeam.WHITE ? PlayerTeam.BLACK : PlayerTeam.WHITE;
            List<MoveData> allMoves = GetMoves(opponent);
            allMoves = Shuffle(allMoves);
            foreach (MoveData move in allMoves)
            {
                moveStack.Push(move);

                DoFakeMove(move.firstPosition, move.secondPosition);
                int score = CalculateMinMax(depth - 1,int.MinValue,int.MaxValue, true);
                UndoFakeMove();

                if (score < beta)                
                    beta = score; 

                if (score <= alpha)                
                break;                         
            }
            return beta;
        }
    }

}
