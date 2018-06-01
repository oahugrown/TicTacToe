// YYeung
// TicTacToeManager.cs

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TicTacToeManager : MonoBehaviour
{
    #region DATA
    private const int k_threeByThree = 3;
    private const int k_fourByFour = 4;
    private const int k_minMovesToWin = 5;

    // To determine if the tile is open to play, or if it belongs to player
    public enum Tile
    {
        playerOne,
        playerTwo,
        open
    }

    public enum PlayerTurn
    {
        playerOneTurn,
        playerTwoTurn
    }

    public struct Move
    {
        public int tileIndex;
        public int playerIndex;
    }

    private ChooseAGrid chooseAGrid;
    private ChooseASymbol chooseASymbol;
    private PlayerTurn playerTurn;

    private List<Tile> tiles = new List<Tile>();
    private List<Move> moves = new List<Move>();
    //private List<Move> winningMoves = new List<Move>();
    private Sprite[] playerSymbols = new Sprite[2];
    private Image[] playerActive = new Image[2];

    private int sizeOfBoard;
    #endregion // DATA

    #region GETTERS
    public int GetPlayerTurn() { return (int)playerTurn; }
    #endregion // GETTERS


    #region MONO
    private void Awake()
    {
        GameOver.onReset += ResetGame;
        SetUpGame();
    }
    #endregion // MONO


    #region UICONTROLS
    public void PlayTile(int tileIndex)
    {
        if (tiles[tileIndex] != Tile.open)
            return;

        // Assign the player to the tile
        GameObject tileButton = this.transform.GetChild(tileIndex).gameObject;
        tileButton.GetComponent<Image>().sprite = playerSymbols[(int)playerTurn];
        tiles[tileIndex] = (Tile)playerTurn;

        // Record move history
        Move newMove;
        newMove.tileIndex = tileIndex;
        newMove.playerIndex = (int)playerTurn;
        moves.Add(newMove);

        // Check for a win when necessary
        if (moves.Count >= k_minMovesToWin)
        {
            if (HasPlayerWon())
            {
                EndGame();
                return;
            }
        }

        // Stalemate
        if (moves.Count == sizeOfBoard * sizeOfBoard)
        {
            EndGame(true);
            return;
        }

        // Switch players
        playerActive[(int)playerTurn].enabled = false;
        int nextPlayer = (int)playerTurn + 1;

        if (nextPlayer >= chooseASymbol.GetPlayerLength())
            nextPlayer = 0;
        playerTurn = (PlayerTurn)nextPlayer;
        playerActive[(int)playerTurn].enabled = true;
    }
    #endregion // UICONTROLS


    #region PRIVATE
    private bool HasPlayerWon()
    {
        if (IsHorizontalWin())
            return true;
        else if (IsVerticalWin())
            return true;
        else if (IsBackSlashWin())
            return true;
        else if (IsForwardSlashWin())
            return true;
        
        return false;
    }

    private bool IsHorizontalWin()
    {
        // Determining the furthest left tile in the row of the last played tile
        int tileIndex = moves[moves.Count - 1].tileIndex;
        int remainder = tileIndex % sizeOfBoard;
        tileIndex -= remainder;

        // Check from left to right for a win
        for (int i = 0; i < sizeOfBoard; ++i)
        {
            if ((int)tiles[tileIndex] != (int)playerTurn)
                return false;
            else
                tileIndex++;
        }

        return true;
    }

    private bool IsVerticalWin()
    {
        // Determining the top tile in the column of the last played tile
        int tileIndex = moves[moves.Count - 1].tileIndex;
        int topTile = tileIndex;
        for (int i = 0; i < sizeOfBoard; ++i)
        {
            if ((topTile - sizeOfBoard) >= 0)
                topTile -= sizeOfBoard;
            else
                break;
        }

        // Check from top to bottom for a win
        for (int i = 0; i < sizeOfBoard; ++i)
        {
            if ((int)tiles[topTile] != (int)playerTurn)
                return false;
            else
                topTile += sizeOfBoard;
        }

        return true;
    }

    private bool IsBackSlashWin()
    {
        int tileIndex = 0;
        for (int i = 0; i < sizeOfBoard; ++i)
        {
            if ((int)tiles[tileIndex] != (int)playerTurn)
                return false;
            else
            {
                if (tileIndex == tiles.Count - 1)
                    return true;
                else
                    tileIndex += (sizeOfBoard + 1);
            }
        }
        return false;
    }

    private bool IsForwardSlashWin()
    {
        int tileIndex = sizeOfBoard * (sizeOfBoard - 1);
        for (int i = 0; i < sizeOfBoard; ++i)
        {
            if ((int)tiles[tileIndex] != (int)playerTurn)
                return false;
            else
            {
                if (tileIndex == sizeOfBoard - 1)
                    return true;
                else
                    tileIndex -= (sizeOfBoard - 1);
            }
        }

        return false;
    }

    private void SetUpGame()
    {
        // Caching data
        chooseAGrid = GameObject.FindWithTag("GridCanvas").GetComponent<ChooseAGrid>();
        chooseASymbol = GameObject.FindWithTag("SymbolCanvas").GetComponent<ChooseASymbol>();

        // Setting size of board
        if (chooseAGrid.GetGridType() == ChooseAGrid.GridType.threeByThree)
            sizeOfBoard = k_threeByThree;
        else
            sizeOfBoard = k_fourByFour;

        playerTurn = PlayerTurn.playerOneTurn;

        // Getting the player symbols
        for (int i = 0; i < chooseASymbol.GetPlayerLength(); ++i)
        {
            int symbolIndex = (int)chooseASymbol.GetPlayerSymbol(i);
            playerSymbols[i] = chooseASymbol.GetSymbolImage(symbolIndex);
        }

        // Getting and setting game UI
        for (int i = 0; i < chooseASymbol.GetPlayerLength(); ++i)
        {
            // Getting ctive images
            Transform playerUI = this.transform.parent.GetChild(i);
            Image playerActiveImage = playerUI.GetChild(0).GetComponent<Image>();
            playerActive[i] = playerActiveImage;
            // Setting player symbols
            playerUI.transform.GetChild(1).GetComponent<Image>().sprite = playerSymbols[i];
        }

        // Setting up the board
        for (int i = 0; i < (sizeOfBoard * sizeOfBoard); ++i)
        {
            Tile newTile = new Tile();
            newTile = Tile.open;
            tiles.Add(newTile);
        }
    }

    private void EndGame(bool draw = false)
    {   
        GameOver gameOver = GameObject.FindWithTag("GameOverCanvas").GetComponent<GameOver>();

        if (draw)
            gameOver.ActivateGameOver(-1);
        else
            gameOver.ActivateGameOver((int)playerTurn);
    }

    private void ResetGame()
    {
        transform.parent.GetComponent<Canvas>().enabled = false;
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        GameOver.onReset -= ResetGame;
    }
    #endregion  // PRIVATE
}
