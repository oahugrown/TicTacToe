// YYeung
// TestData.cs

using UnityEngine;
[System.Serializable]
public class TestData 
{
    public enum PlayerToWin
    {
        PLAYER_ONE,
        PLAYER_TWO
    }

    public enum SymbolOptions
    {
        CHIP,
        CLUB,
        DIAMOND,
        HEART
    }

    public enum GridOptions
    {
        THREE_BY_THREE,
        FOUR_BY_FOUR
    }

    public enum WinStyle
    {
        HORIZONTAL,
        VERTICAL,
        DIAGONAL,
        DRAW
    }

    public enum DiagonalOptions
    {
        FORWARD_SLASH,
        BACK_SLASH
    }

    public PlayerToWin playerToWin;
    public SymbolOptions[] playerSymbols = new SymbolOptions[2];

    public GridOptions gridOptions;

    public WinStyle winStyle;
    public DiagonalOptions diagonalOptions;
    public int winIndex;

    public float speed;
    public float minSpeed = 1;
    public float maxSpeed = 5;

}