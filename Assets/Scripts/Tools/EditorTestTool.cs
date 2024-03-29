﻿// YYeung
// WinningTestTool.cs

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
class EditorTestTool : EditorWindow
{
    #region UNITYMENU
    [MenuItem("Tools/Open Test Window")]

    public static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindow(typeof(EditorTestTool), false, "TTT Test");
        window.minSize = new Vector2(200,400);
    }
    #endregion // UNITYMENU


    #region DATA
    private TestData testData = new TestData();
    private bool testActivated = false;
    private string status;
    private GameObject gameOverCanvas;
    GameObject test;
    int testLengthIndex;
    Vector2 scrollPos;
    #endregion // DATA


    #region WINDOW
    void OnGUI()
    {
        if (!EditorApplication.isPlaying)
        {
            EditorGUILayout.HelpBox("Game needs to be in PlayMode for Testing Tool to activate.", MessageType.Warning);
            ReloadTest();
            return;
        }
        if (testActivated)
        {
            // Results
            GuiSeparator("Test in session");
            string playerToWin = "";
            if (testData.winStyle != TestData.WinStyle.DRAW)
                playerToWin = "\nWinning player: " + testData.playerToWin;
            string results = ("Grid: " + testData.gridOptions.ToString() + "\nWin style: " + testData.winStyle + GetWinOption() + playerToWin + status);
            EditorGUILayout.BeginHorizontal();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(position.height * 2 / 3));
            GUILayout.Label(results);
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();
            ShowBigWinTest();
            return;
        }
        // Title
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUIStyle style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        EditorGUILayout.LabelField("TicTacToe Tester", style, GUILayout.ExpandWidth(true));
        // Speed
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Test Speed ");
        testData.speed = EditorGUILayout.Slider(testData.speed, 0, testData.maxSpeed);
        EditorGUILayout.EndHorizontal();
        // Number of runs
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Number of tests to run");
        testData.testLength = EditorGUILayout.IntField(testData.testLength);
        if (testData.testLength < 1)
            testData.testLength = 1;
        EditorGUILayout.EndHorizontal();

        // Player options
        GuiSeparator("Player Symbol Options: ");
        testData.playerSymbols[0] = (TestData.SymbolOptions)EditorGUILayout.EnumPopup("Player 1", testData.playerSymbols[0]);
        testData.playerSymbols[1] = (TestData.SymbolOptions)EditorGUILayout.EnumPopup("Player 2", testData.playerSymbols[1]);
        if (testData.playerSymbols[1] == testData.playerSymbols[0])
            testData.playerSymbols[1] = (TestData.SymbolOptions)FindUnusedSymbol();

        // Grid options
        GuiSeparator("Grid Options: ");
        testData.gridOptions = (TestData.GridOptions)EditorGUILayout.EnumPopup("Grid style", testData.gridOptions);

        // Win options
        GuiSeparator("Win Options: ");
        testData.winStyle = (TestData.WinStyle)EditorGUILayout.EnumPopup("Win style", testData.winStyle);
        int sliderMax;
        if (testData.gridOptions == TestData.GridOptions.THREE_BY_THREE)
            sliderMax = 2;
        else
            sliderMax = 3;
        switch (testData.winStyle)
        {
            case TestData.WinStyle.HORIZONTAL:
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Winning Row: ");
                testData.winIndex = EditorGUILayout.IntSlider(testData.winIndex, 0, sliderMax);
                EditorGUILayout.EndHorizontal();
                testData.playerToWin = (TestData.PlayerToWin)EditorGUILayout.EnumPopup("Winning player", testData.playerToWin);
                break;
            case TestData.WinStyle.VERTICAL:
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Winning Column: ");
                testData.winIndex = EditorGUILayout.IntSlider(testData.winIndex, 0, sliderMax);
                EditorGUILayout.EndHorizontal();
                testData.playerToWin = (TestData.PlayerToWin)EditorGUILayout.EnumPopup("Winning player", testData.playerToWin);
                break;
            case TestData.WinStyle.DIAGONAL:
                testData.diagonalOptions = (TestData.DiagonalOptions)EditorGUILayout.EnumPopup("Diagonal slash", testData.diagonalOptions);
                testData.playerToWin = (TestData.PlayerToWin)EditorGUILayout.EnumPopup("Winning player", testData.playerToWin);
                break;
            case TestData.WinStyle.DRAW:
                break;
        }

        // Start the test
        GuiSeparator("Test: ");
        if (GUILayout.Button("Run Test"))
        {
            testLengthIndex = 1;
            RunTest();
        }

        ShowBigWinTest();

    }
    #endregion

    #region PRIVATE
    private void ShowBigWinTest()
    {
        GuiSeparator("Big Win");
        string buttonText = "";
        if (BigWin.bigWinActive)
            buttonText = "Stop Big Win effects";
        else
            buttonText = "Play Big Win effects";
        if (GUILayout.Button(buttonText))
        {
            BigWin.bigWinActive = !BigWin.bigWinActive;
            BigWin.ActivateBigWin(BigWin.bigWinActive);
        }
        string cannonText = "";
        if (BigWin.coinCannonActive)
            cannonText = "Stop coin cannons";
        else
            cannonText = "Play coin cannons";
        if (GUILayout.Button(cannonText))
        {
            BigWin.coinCannonActive = !BigWin.coinCannonActive;
            BigWin.ActivateCoins(BigWin.coinCannonActive);
        }
    }

    private void Update()
    {
        if (!testActivated)
            return;
        
        if (GameOver.gameIsOver && testLengthIndex < testData.testLength)
        {
            float timeToSleep = Time.deltaTime + testData.speed;
            if (timeToSleep >= Time.deltaTime)
            {
                ++testLengthIndex;
                Destroy(test);
                RunTest();
            }
        }
    }

    void TestComplete()
    {
        GameOver.onReload += ReloadTest;
        // Show test results based on gameover
        if (GameOver.winningPlayer == -1 && testData.winStyle == TestData.WinStyle.DRAW)
            status += "\n" + " SUCCESS";
        else if (GameOver.winningPlayer == 0 && testData.playerToWin == TestData.PlayerToWin.PLAYER_ONE)
            status += "\n" + " SUCCESS";
        else if (GameOver.winningPlayer == 1 && testData.playerToWin == TestData.PlayerToWin.PLAYER_TWO)
            status += "\n" + " SUCCESS";
        else
            status += "\n" + testLengthIndex + " FAILED****************************";
    }

    void ReloadTest()
    {
        testActivated = false;
        GameOver.onGameOver -= TestComplete;
    }

    void RunTest()
    {
        // Do a test on a fresh game by resetting the game
        GameObject.FindWithTag("GameOverCanvas").GetComponent<GameOver>().ResetGame();

        status = "\n\n RESULTS";
        testActivated = true;
        GameOver.onGameOver += TestComplete;
        GameOver.onReload -= ReloadTest;
        UnityEngine.Object testObject = Resources.Load("Prefabs/Testing/Test");
        test = (GameObject)Instantiate(testObject);
        test.GetComponent<TicTacToeTest>().StartTest(testData);
    }

    void GuiSeparator(string label)
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // Grid options
        GUILayout.Label("", GUI.skin.horizontalSlider);
        GUILayout.Label(label);
    }

    private string GetWinOption()
    {
        string results = "";
        switch (testData.winStyle)
        {
            case TestData.WinStyle.HORIZONTAL:
                results = ", Row: " + testData.winIndex;
                break;

            case TestData.WinStyle.VERTICAL:
                results = ", Column: " + testData.winIndex;
                break;
            case TestData.WinStyle.DIAGONAL:
                results = ", " + testData.diagonalOptions.ToString();
                break;
        }

        return results;
    }


    int FindUnusedSymbol()
    {
        int symbolLength = System.Enum.GetNames(typeof(TestData.SymbolOptions)).Length;
        int nextAvailableSymbol;
        for (int i = 0; i < symbolLength; ++i)
        {
            nextAvailableSymbol = i;
            if (nextAvailableSymbol >= System.Enum.GetNames(typeof(TestData.SymbolOptions)).Length)
                nextAvailableSymbol = 0;
            if ((int)testData.playerSymbols[0] != nextAvailableSymbol)
                return nextAvailableSymbol;
        }
        return 0;
    }
    #endregion // PRIVATE

}


#endif // UNITY_EDITOR