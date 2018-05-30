// YYeung
// GameOver.cs

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour 
{
    public string[] playerStrings;
    public Text winnerText;
    private void Start()
    {
        GetComponent<Canvas>().enabled = false;
    }

    public void ActivateGameOver(int playerThatWon)
    {
        GetComponent<Canvas>().enabled = true;
        winnerText.text = playerStrings[playerThatWon];     
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
	
}
