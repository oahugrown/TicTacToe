// YYeung
// TestData.cs


#if UNITY_EDITOR
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
    public float minSpeed = 0;
    public float maxSpeed = 1;
    public int testLength = 0;
}

#endif // UNITY_EDITOR