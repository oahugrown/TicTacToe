// YYeung
// TestData.cs

using UnityEngine;

public class TestData 
{
    [System.Serializable]
    public enum PlayerToWin
    {
        PLAYER_ONE,
        PLAYER_TWO
    }

    [System.Serializable]
    public enum SymbolOptions
    {
        CHIP,
        CLUB,
        DIAMOND,
        HEART
    }

    [System.Serializable]
    public enum GridOptions
    {
        THREE_BY_THREE,
        FOUR_BY_FOUR
    }

    [System.Serializable]
    public enum WinStyle
    {
        HORIZONTAL,
        VERTICAL,
        DIAGONAL,
        DRAW
    }

    [System.Serializable]
    public enum DiagonalOptions
    {
        FORWARD_SLASH,
        BACK_SLASH
    }

    [SerializeField]
    public PlayerToWin playerToWin;
    [SerializeField]
    public SymbolOptions[] playerSymbols = new SymbolOptions[2];

    [SerializeField]
    public GridOptions gridOptions;

    [SerializeField]
    public WinStyle winStyle;
    [SerializeField]
    public DiagonalOptions diagonalOptions;
    [SerializeField]
    public int winIndex;

    [SerializeField]
    public float speed;
    [SerializeField]
    public float minSpeed = 1;
    [SerializeField]
    public float maxSpeed = 10;
    [SerializeField]
    public bool testInSession;
}