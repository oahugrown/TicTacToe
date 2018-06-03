// YYeung
// GameOver.cs

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour 
{
    public delegate void GamePlayOver();
    public static event GamePlayOver onGameOver;
    public delegate void ReloadGameScene();
    public static event ReloadGameScene onReload;
    public delegate void ResetGamePlay();
    public static event ResetGamePlay onReset;
    public static bool gameIsOver = false;

    public string[] playerStrings;
    public Text winnerText;
    public static int winningPlayer;
    private void Awake()
    {
        NewGame();
    }

    private void NewGame()
    {
        GetComponent<Canvas>().enabled = false;
        gameIsOver = false;
    }

    public void ActivateGameOver(int playerThatWon)
    {
        gameIsOver = true;
        GetComponent<Canvas>().enabled = true;
        winningPlayer = playerThatWon;

        // draw
        if (playerThatWon == -1)
        {
            winnerText.text = "DRAW";
            transform.GetChild(3).GetComponent<Text>().text = "";
        }
        else
            winnerText.text = playerStrings[playerThatWon];

        if (onGameOver != null)
            onGameOver();
    }

    public void ReloadScene()
    {
        if (onReload != null)
            onReload();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResetGame()
    {
        NewGame();
        if(onReset != null)
            onReset();
    }
}
