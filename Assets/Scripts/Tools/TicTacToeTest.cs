// YYeung
// TicTacToeTest.cs

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TicTacToeTest : MonoBehaviour
{
    [SerializeField]
    private TestData data;
    private GameObject symbolCanvas;
    private GameObject gridCanvas;
    private GameObject gameCanvas;
    private TicTacToeManager gameManager;

    private List<int> winningTiles = new List<int>();
    private List<int> tilesRemaining = new List<int>();
    private int winningTileIndex;
    private int sizeOfBoard;

    public void StartTest(TestData newData)
    {
        data = newData;
        InitializeTestData();

        // Find UI buttons
        if (FindAllCanvasObjects())
            StartCoroutine(Run());
        else
            Debug.LogError("Failed to set up necessary references TicTacToeTest.StartTest()");

    }
    bool FindAllCanvasObjects()
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
    IEnumerator Run()
    {
        // Do a test on a fresh game by disabling all the canvas's and set a new game
        gameManager = gameCanvas.transform.GetChild(2).GetComponent<TicTacToeManager>();
        symbolCanvas.GetComponent<ChooseASymbol>().SetNewGame();
        
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

        // Alternate player turns until player wins
        gameManager = gameCanvas.transform.GetChild(2).GetComponent<TicTacToeManager>();
        buttons = gameCanvas.transform.GetChild(2);
        int runTime = (sizeOfBoard * 2) - 1;
        for (int i = 0; i < runTime; ++i)
        {
            // Making sure the winning player plays the right
            if ((int)data.playerToWin == gameManager.GetPlayerTurn())
            {
                Transform tile = buttons.GetChild(winningTiles[winningTileIndex]);
                tile.GetComponent<Button>().onClick.Invoke();
                ++winningTileIndex;
            }
            else
            {
                bool searching = true;
                int rng = 0;
                while(searching)
                {
                    // randomly choose a tile that hasn't been played or isn't on the winning tile list.
                    rng = Random.Range(0, tilesRemaining.Count);
                    // Make sure that player doesn't accidently win
                    searching = false;
                }
                buttons.GetChild(tilesRemaining[rng]).GetComponent<Button>().onClick.Invoke();
            }
            yield return new WaitForSeconds(GetSpeed());
        }

        // Success!
    }
    float GetSpeed()
    {
        float speed = data.speed;
        speed /= 3;
        print(speed);
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
        // Getting the winning tiles

        if (data.gridOptions == TestData.GridOptions.THREE_BY_THREE)
            sizeOfBoard = 3;
        else
            sizeOfBoard = 4;

        // Setting the index and tile increment data
        int tileIndex = 0;
        int tileIncrement = 0;
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
            // Setting the winning tiles
            for (int i = 0; i < sizeOfBoard; ++i)
            {
                winningTiles.Add(tileIndex);
                tileIndex += tileIncrement;
            }
        }
        // Filling remaining tiles with tiles that the losing player can play.
        for (int i = 0; i < sizeOfBoard * sizeOfBoard; ++i)
        {
            tilesRemaining.Add(i);
        }
        for (int i = 0; i < winningTiles.Count; ++i)
        {
            tilesRemaining.Remove(winningTiles[i]);
        }
    }
}
