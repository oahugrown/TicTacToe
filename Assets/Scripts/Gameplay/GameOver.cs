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

    public string[] playerStrings;
    public Text winnerText;
    public static int winningPlayer;
    private void Awake()
    {
        GetComponent<Canvas>().enabled = false;
    }

    public void ActivateGameOver(int playerThatWon)
    {
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
        GetComponent<Canvas>().enabled = false;
        if(onReset != null)
            onReset();
    }
}
