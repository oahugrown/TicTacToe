// YYeung
// ChooseASymbol.cs

using UnityEngine.UI;
using UnityEngine;

public class ChooseASymbol : MonoBehaviour
{
    #region DATA
    private enum SelectedPlayer
    {
        playerOne = 0,
        playerTwo,
        length,
    }

    // This needs to be set in the same order as symbolButtons to get the correct index
    public enum PlayerSymbol
    {
        chip = 0,
        club,
        diamond,
        heart,
        unassigned
    }

    private SelectedPlayer selectedPlayer = SelectedPlayer.playerOne;
    private PlayerSymbol[] playerSymbols = new PlayerSymbol[(int)SelectedPlayer.length];

    public GameObject[] playerSelectObjects;
    public Button[] symbolButtons;
    public Button continueButton;
    #endregion // DATA


    #region GETTERS_AND_SETTERS
    public PlayerSymbol GetPlayerSymbol(int playerIndex) { return playerSymbols[playerIndex]; }
    public Sprite GetSymbolImage(int symbolIndex)
    {
        return symbolButtons[symbolIndex].GetComponent<Image>().sprite;
    }
    public int GetPlayerLength() { return (int)SelectedPlayer.length; }
    #endregion // GETTERS


    #region MONO
    private void Awake()
    {
        GameOver.onReset += SetNewGame;
        SetNewGame();
    }
    #endregion // MONO


    #region UICONTROLLER
    public void AssignSymbolToPlayer(int symbol)
    {
        // Remove the selected outline from the player
        playerSelectObjects[(int)selectedPlayer].GetComponent<Outline>().enabled = false;

        // Assign the symbol
        playerSymbols[(int)selectedPlayer] = (PlayerSymbol)symbol;
        Sprite symbolSprite = symbolButtons[symbol].GetComponent<Image>().sprite;
        Image symbolImage = playerSelectObjects[(int)selectedPlayer].transform.GetChild(1).GetComponent<Image>();
        symbolImage.enabled = true;
        symbolImage.sprite = symbolSprite;

        // Deactivate symbol button so other player doesn't select it
        symbolButtons[symbol].interactable = false;

        // If players have assigned a symbol, enable the continue button
        int numberOfPlayers = (int)SelectedPlayer.length - 1;
        if (selectedPlayer == (SelectedPlayer)numberOfPlayers)
            Continue();
        else
            ChangeSelectedPlayer((int)selectedPlayer + 1);

    }
    #endregion // UICONTROLLER


    #region PRIVATE
    private void ChangeSelectedPlayer(int playerIndex)
    {
        // if the player was already selected than bail
        if (playerIndex == (int)selectedPlayer)
            return;

        // Show which player is currently selected.
        for (int i = 0; i < (int)SelectedPlayer.length; ++i)
        {
            playerSelectObjects[i].GetComponent<Outline>().enabled = false;
        }
        playerSelectObjects[playerIndex].GetComponent<Outline>().enabled = true;
        selectedPlayer = (SelectedPlayer)playerIndex;
    }

    private void Continue()
    {
        GetComponent<Canvas>().enabled = false;
        GameObject.FindWithTag("GridCanvas").GetComponent<Canvas>().enabled = true;
    }

    private void SetNewGame()
    {
        for (int i = 0; i < playerSelectObjects.Length; ++i)
        {
            // Assigning a default value
            playerSymbols[i] = PlayerSymbol.unassigned;
        }
        GetComponent<Canvas>().enabled = true;
        selectedPlayer = SelectedPlayer.playerOne;
    }
    private void OnDestroy()
    {
        GameOver.onReset -= SetNewGame;
    }
    #endregion // PRIVATE
}
