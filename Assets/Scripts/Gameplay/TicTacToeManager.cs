// YYeung
// TicTacToeManager.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TicTacToeManager : MonoBehaviour
{
    #region DATA
    public delegate void BoardIsSet();
    public static event BoardIsSet onBoardIsSet;

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
    private List<int> winningTiles = new List<int>();
    private Sprite[] playerSymbols = new Sprite[2];
    private Image[] playerActive = new Image[2];
    private GameObject playerFX;
    private Transform[] playerFXPositions = new Transform[2];
    public float speedOfBoard = 0.2f;

    private int sizeOfBoard;
    #endregion // DATA

    #region GETTERS
    public int GetPlayerTurn() { return (int)playerTurn; }
    #endregion // GETTERS


    #region MONO
    private void Awake()
    {
        GameOver.onReset += ResetGame;
        StartCoroutine(SetUpGame());
    }
    #endregion // MONO


    #region UICONTROLS
    public void PlayTile(int tileIndex)
    {
        if (tiles[tileIndex] != Tile.open)
            return;

        // Assign the player to the tile
		GameObject tileButton = this.transform.GetChild(0).GetChild(tileIndex).gameObject;
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
		Vector3 newPosition = playerFXPositions [(int)playerTurn].position;
		newPosition.z = -4;
		playerFX.transform.position = newPosition;
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
        winningTiles.Clear();

        // Check from left to right for a win
        for (int i = 0; i < sizeOfBoard; ++i)
        {
            if ((int)tiles[tileIndex] != (int)playerTurn)
                return false;
            else
            {
                winningTiles.Add(tileIndex);
                tileIndex++;
            }
        }

        return true;
    }

    private bool IsVerticalWin()
    {
        // Determining the top tile in the column of the last played tile
        int tileIndex = moves[moves.Count - 1].tileIndex;
        int topTile = tileIndex;
        winningTiles.Clear();
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
            {
                winningTiles.Add(topTile);
                topTile += sizeOfBoard;
            }
        }

        return true;
    }

    private bool IsBackSlashWin()
    {
        int tileIndex = 0;
        winningTiles.Clear();
        for (int i = 0; i < sizeOfBoard; ++i)
        {
            if ((int)tiles[tileIndex] != (int)playerTurn)
                return false;
            else
            {
                winningTiles.Add(tileIndex);
                tileIndex += (sizeOfBoard + 1);
                if (winningTiles.Count == sizeOfBoard)
                    return true;
            }
        }
        return false;
    }

    private bool IsForwardSlashWin()
    {
        int tileIndex = sizeOfBoard * (sizeOfBoard - 1);
        winningTiles.Clear();
        for (int i = 0; i < sizeOfBoard; ++i)
        {
            if ((int)tiles[tileIndex] != (int)playerTurn)
                return false;
            else
            {
                
                winningTiles.Add(tileIndex);
                tileIndex -= (sizeOfBoard - 1);
                if (winningTiles.Count == sizeOfBoard)
                    return true;
            }
        }

        return false;
    }

    private IEnumerator SetUpGame()
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
            playerFXPositions[i] = playerUI.GetChild(3).transform;
            // Setting player symbols
            playerUI.transform.GetChild(1).GetComponent<Image>().sprite = playerSymbols[i];
        }

        // Spawning player turn fx
        UnityEngine.Object fx = Resources.Load("Prefabs/Gameplay/StarOutlineFX");
        playerFX = (GameObject)Instantiate(fx);

        // Setting up the board
        for (int i = 0; i < (sizeOfBoard * sizeOfBoard); ++i)
        {
            Tile newTile = new Tile();
            newTile = Tile.open;
            tiles.Add(newTile);
        }

        // Store button locations and then teleport them off screen
        GameObject[] tileButtons = GameObject.FindGameObjectsWithTag("TileButton");
        List<Vector2> buttonLocations = new List<Vector2>();
        foreach(GameObject tileButton in tileButtons)
        {
			buttonLocations.Add(tileButton.GetComponent<RectTransform>().anchoredPosition);
			tileButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,500);
        }
		yield return new WaitForSeconds (0.5f);
        for (int i = 0; i < tileButtons.Length; ++i)
        {
			StartCoroutine(AnimateTile(buttonLocations[i], tileButtons[i].GetComponent<RectTransform>().anchoredPosition, tileButtons[i], Time.deltaTime + speedOfBoard));
            yield return new WaitForSeconds(speedOfBoard);
        }
        if (onBoardIsSet != null)
            onBoardIsSet();
    }

    IEnumerator AnimateTile(Vector3 endPosition, Vector3 startPosition, GameObject tileToAnimate, float time)
    {
        float elapsedTime = 0;
		float percentage = elapsedTime / time;
		while (percentage < 1)
        {
            elapsedTime += Time.deltaTime;
			percentage = elapsedTime / time;
			tileToAnimate.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(startPosition, endPosition, percentage);
            yield return null;
        }
    }

    private void EndGame(bool draw = false)
    {
        GameOver gameOver = GameObject.FindWithTag("GameOverCanvas").GetComponent<GameOver>();

        if (draw)
            gameOver.ActivateGameOver(-1);
        else
        {
            playerFX.SetActive(true);
            gameOver.ActivateGameOver((int)playerTurn);
            GameObject game = GameObject.FindWithTag("GameCanvas").transform.GetChild(2).gameObject;
            for (int i = 0; i < winningTiles.Count; ++i)
            {
                GameObject tileObject = game.transform.GetChild(0).GetChild(winningTiles[i]).gameObject;
                tileObject.GetComponent<Animation>().Play("TileWiggle");
                float size = tileObject.GetComponent<RectTransform>().localScale.x;
                float newSize = size += size / 4;
                tileObject.GetComponent<RectTransform>().localScale = new Vector3(newSize, newSize);
            }
        }
    }

    private void ResetGame()
    {
        Destroy(playerFX);
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        GameOver.onReset -= ResetGame;
    }
    #endregion  // PRIVATE
}
