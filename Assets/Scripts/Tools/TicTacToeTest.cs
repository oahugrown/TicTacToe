// YYeung
// TicTacToeTest.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TicTacToeTest : MonoBehaviour
{
    #region DATA
    [SerializeField]
    private TestData data;
    private GameObject symbolCanvas;
    private GameObject gridCanvas;
    private GameObject gameCanvas;
    private TicTacToeManager gameManager;

    private List<int> playerOneTiles = new List<int>();
    private List<int> playerTwoTiles = new List<int>();
    private int sizeOfBoard;

    int firstRowIndex = 0;
    List<bool> firstRow = new List<bool>();
    private bool waitingForTheBoard = true;
    #endregion // DATA


    public void StartTest(TestData newData)
    {
        data = newData;
        TicTacToeManager.onBoardIsSet += BoardIsReady;
        InitializeTestData();

        // Find UI buttons
        if (FindAllCanvasObjects())
            StartCoroutine(Run());
        else
            Debug.LogError("Failed to set up necessary references TicTacToeTest.StartTest()");

    }


    #region PRIVATE
    private void BoardIsReady()
    {
        waitingForTheBoard = false;
    }

    private void OnDestroy()
    {
        TicTacToeManager.onBoardIsSet -= BoardIsReady;
    }

    private bool FindAllCanvasObjects()
    {
        symbolCanvas = GameObject.FindWithTag("SymbolCanvas");
        if (symbolCanvas == null)
            return false;

        gridCanvas = GameObject.FindWithTag("GridCanvas");
        if (gridCanvas == null)
            return false;

        gameCanvas = GameObject.FindWithTag("GameCanvas");
        if (gameCanvas == null)
            return false;

        return true;
    }

    private IEnumerator Run()
    {
        // Select player 1 symbol
        SelectPlayerSymbol(0);
        yield return new WaitForSeconds(GetSpeed());
        // Select player 2 symbol
        SelectPlayerSymbol(1);
        yield return new WaitForSeconds(GetSpeed());

        // Select grid size
        Transform buttons = gridCanvas.transform.GetChild(0);
        buttons.GetChild((int)data.gridOptions).GetComponent<Button>().onClick.Invoke();
        yield return new WaitForSeconds(GetSpeed());

        // Continue
        buttons.GetChild(2).GetComponent<Button>().onClick.Invoke();

        // waiting for the board to be set
        while (waitingForTheBoard)
        {
            yield return null;
        }

        // Alternate player turns until player wins
        gameManager = gameCanvas.transform.GetChild(2).GetComponent<TicTacToeManager>();
        buttons = gameCanvas.transform.GetChild(2).GetChild(0);
        int runTime = sizeOfBoard * sizeOfBoard;
        for (int j = 0; j < runTime; ++j)
        {
            if (GameOver.gameIsOver)
                break;

            // player 1's turn
            if (gameManager.GetPlayerTurn() == 0)
            {
                int rng = Random.Range(0, playerOneTiles.Count);
                buttons.GetChild(playerOneTiles[rng]).GetComponent<Button>().onClick.Invoke();
                playerOneTiles.RemoveAt(rng);
            }
            else // player 2's turn
            {
                int rng = Random.Range(0, playerTwoTiles.Count);
                buttons.GetChild(playerTwoTiles[rng]).GetComponent<Button>().onClick.Invoke();
                playerTwoTiles.RemoveAt(rng);
            }
            yield return new WaitForSeconds(GetSpeed());
        }
    }

    private float GetSpeed()
    {
        float speed = data.speed;
        speed /= 3;
        return speed;
    }

    private void SelectPlayerSymbol(int player)
    {
        Transform buttons = symbolCanvas.transform.GetChild(0);
        switch (data.playerSymbols[player])
        {
            case TestData.SymbolOptions.CHIP:
                buttons.GetChild(2).GetComponent<Button>().onClick.Invoke();
                break;
            case TestData.SymbolOptions.CLUB:
                buttons.GetChild(3).GetComponent<Button>().onClick.Invoke();
                break;
            case TestData.SymbolOptions.DIAMOND:
                buttons.GetChild(4).GetComponent<Button>().onClick.Invoke();
                break;
            case TestData.SymbolOptions.HEART:
                buttons.GetChild(5).GetComponent<Button>().onClick.Invoke();
                break;
        }
    }

    private void InitializeTestData()
    {
        // Getting the grid size
        if (data.gridOptions == TestData.GridOptions.THREE_BY_THREE)
            sizeOfBoard = 3;
        else
            sizeOfBoard = 4;


        // Making sure our data is empty
        playerTwoTiles.Clear();
        playerOneTiles.Clear();

        // Setting the index and tile increment data
        int tileIndex = 0;
        int tileIncrement = 1;
        if (data.winStyle == TestData.WinStyle.HORIZONTAL)
        {
            tileIncrement = 1;
            tileIndex = sizeOfBoard * data.winIndex;
        }
        else if (data.winStyle == TestData.WinStyle.VERTICAL)
        {
            tileIncrement = sizeOfBoard;
            tileIndex = data.winIndex;
        }
        else if (data.winStyle == TestData.WinStyle.DIAGONAL)
        {
            if (data.diagonalOptions == TestData.DiagonalOptions.FORWARD_SLASH)
            {
                tileIndex = sizeOfBoard * (sizeOfBoard - 1);
                tileIncrement = -(sizeOfBoard - 1);
            }
            else
            {
                tileIndex = 0;
                tileIncrement = sizeOfBoard + 1;
            }
        }
        if (data.winStyle != TestData.WinStyle.DRAW)
        {
            List<int> winningTiles = new List<int>();
            List<int> losingTiles = new List<int>();
            // Setting the winning tiles
            for (int i = 0; i < sizeOfBoard; ++i)
            {
                winningTiles.Add(tileIndex);
                tileIndex += tileIncrement;
            }

            // Filling remaining tiles with tiles that the losing player can play.
            int middleOfBoard = (sizeOfBoard * sizeOfBoard) / 2;
            for (int i = 0; i < middleOfBoard; ++i)
            {
                if (!winningTiles.Contains(i))
                {
                    losingTiles.Add(i);
                    i += Random.Range(1, sizeOfBoard);
                    if (i >= sizeOfBoard * sizeOfBoard)
                        i = 0;
                }
                if (losingTiles.Count > sizeOfBoard)
                    break;
            }
            for (int i = 0; i < sizeOfBoard * sizeOfBoard; ++i)
            {
                if (!winningTiles.Contains(i))
                {
                    if (losingTiles.Contains(i - 1) || losingTiles.Contains(i - sizeOfBoard) || losingTiles.Contains(i))
                        continue;

                    losingTiles.Add(i);
                    i += Random.Range(1, sizeOfBoard);
                    if (i >= sizeOfBoard * sizeOfBoard)
                        i = 0;
                }
                if (losingTiles.Count > sizeOfBoard)
                    break;
            }

            // Set tiles to the players
            if ((int)data.playerToWin == 0)
            {
                playerOneTiles = winningTiles;
                playerTwoTiles = losingTiles;
            }
            else
            {
                playerOneTiles = losingTiles;
                playerTwoTiles = winningTiles;
            }
        }
        // ..Draw
        else
        {
            int playerTwoTilesCount = 0;
            int middleRow = 0;

            if (sizeOfBoard % 2 == 1)
            {
                middleRow = 1;
                playerTwoTilesCount = 1;
            }

            for (int i = 0; i < sizeOfBoard; ++i)
            {
                int endOfRow = i * sizeOfBoard - 1 + sizeOfBoard;
                int rowIndex = 0;
                bool previousTileIsPlayerOne = false;
                for (int j = i * sizeOfBoard; j <= endOfRow; ++j)
                {
                    #region FIRST_ROW
                    // If this is the first row, save whatever pattern was generated
                    if (i == 0)
                    {
                        int rng = Random.Range(0, 2);
                        // Make sure the last tile in the row is the same as the first one
                        if (firstRow.Count == sizeOfBoard - 1)
                        {
                            if (firstRow[0] == false)
                            {
                                playerTwoTiles.Add(j);
                                firstRow.Add(false);
                            }
                            else
                            {
                                playerOneTiles.Add(j);
                                firstRow.Add(true);
                            }

                            // Verify first row didn't accidentally win
                            if (firstRow[0] == firstRow[1])
                            {
                                if (firstRow[1])
                                {
                                    playerTwoTiles.Add(playerOneTiles[1]);
                                    playerOneTiles.RemoveAt(1);
                                }
                                else
                                {
                                    playerOneTiles.Add(playerTwoTiles[1]);
                                    playerTwoTiles.RemoveAt(1);
                                }
                                firstRow[1] = !firstRow[1];
                            }
                        }
                        // ..else.. do random placement
                        else if (rng == 0)
                        {
                            firstRow.Add(true);
                            playerOneTiles.Add(j);
                        }
                        else if (rng == 1)
                        {
                            firstRow.Add(false);
                            playerTwoTiles.Add(j);
                        }
                    }
                    #endregion // FIRST_ROW
                    #region LAST_ROW
                    // If this is the last row, generate the same pattern of the first row, but swap the players
                    else if (i == sizeOfBoard - 1)
                    {
                        if (firstRow[firstRowIndex])
                            playerTwoTiles.Add(j);
                        else
                            playerOneTiles.Add(j);
                        ++firstRowIndex;
                    }
                    #endregion// LAST_ROW

                    #region MIDDLE_ROWS
                    // If this is the middle rows make sure we have an even amount of tiles assigned per player
                    else
                    {
                        // create anything if this is the first middle row
                        if (middleRow == 0)
                        {
                            int rng = Random.Range(0, 2);
                            if (j == endOfRow)
                            {
                                // separate for 4x4
                                ++middleRow;
                                if (!previousTileIsPlayerOne)
                                    playerOneTiles.Add(j);
                                else
                                {
                                    playerTwoTiles.Add(j);
                                    ++playerTwoTilesCount;
                                }
                                playerTwoTilesCount = Mathf.Abs(playerTwoTilesCount - sizeOfBoard);
                            }
                            else if (rng == 0)
                            {
                                playerOneTiles.Add(j);
                                previousTileIsPlayerOne = true;
                            }
                            else if (rng == 1)
                            {
                                playerTwoTiles.Add(j);
                                ++playerTwoTilesCount;
                                previousTileIsPlayerOne = false;
                            }
                        }

                        else
                        {
                            // randomize if we can
                            if (playerTwoTilesCount > 0)
                            {
                                if (sizeOfBoard - rowIndex > playerTwoTilesCount && playerTwoTilesCount > 0)
                                {
                                    int rng = Random.Range(0, 2);
                                    if (rng == 0)
                                        playerOneTiles.Add(j);
                                    else
                                    {
                                        playerTwoTiles.Add(j);
                                        --playerTwoTilesCount;
                                    }
                                }
                                else
                                {
                                    playerTwoTiles.Add(j);
                                    --playerTwoTilesCount;
                                }
                            }
                            else
                            {
                                playerOneTiles.Add(j);
                            }
                        }
                    }
                    #endregion // MIDDLE_ROWS
                    ++rowIndex;
                }
            }
        }
    }
    #endregion // PRIVATE
}
