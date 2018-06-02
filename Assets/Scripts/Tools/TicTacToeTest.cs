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
    private List<int> losingTiles = new List<int>();
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
        int runTime = sizeOfBoard * sizeOfBoard;
        for (int i = 0; i < runTime; ++i)
        {
            // Making sure the winning player plays the right tiles
            if ((int)data.playerToWin == gameManager.GetPlayerTurn())
            {
                Transform tile = buttons.GetChild(winningTiles[winningTileIndex]);
                tile.GetComponent<Button>().onClick.Invoke();
                ++winningTileIndex;
                if (winningTileIndex > winningTiles.Count)
                    break;
            }
            else
            {
                int rng = Random.Range(0, losingTiles.Count);
                buttons.GetChild(losingTiles[rng]).GetComponent<Button>().onClick.Invoke();
                // Safeguard against the rng so that it doesn't choose an already chosen tile.
                losingTiles.RemoveAt(rng);
            }
            yield return new WaitForSeconds(GetSpeed());
        }
        // success

    }

    float GetSpeed()
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
        // Making sure our data is empty
        losingTiles.Clear();
        winningTiles.Clear();

        // Getting the winning tiles
        if (data.gridOptions == TestData.GridOptions.THREE_BY_THREE)
            sizeOfBoard = 3;
        else
            sizeOfBoard = 4;

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
        }
        // ..Draw
        else
        {
            bool tileForPlayer1 = true;
            for (int i = 0; i < sizeOfBoard * sizeOfBoard; ++i)
            {
                if (tileForPlayer1)
                    winningTiles.Add(i);
                else
                    losingTiles.Add(i);
                int rng = Random.Range(0, 2);
                if (rng == 0 || winningTiles.Contains(i - 1) || 
                    winningTiles.Contains(i - sizeOfBoard))
                    tileForPlayer1 = !tileForPlayer1;
            }

            if (winningTiles.Count + 1 > losingTiles.Count)
            {
                losingTiles.Add(0);
                winningTiles.RemoveAt(0);
            }
        }
        // Safeguard to make sure player 1 has more moves that player 2 since 1 goes first
        while (winningTiles.Count < losingTiles.Count)
        {
            int lastIndex = losingTiles.Count - 1;
            winningTiles.Add(losingTiles[lastIndex]);
            losingTiles.RemoveAt(lastIndex);
        }
    }
}
