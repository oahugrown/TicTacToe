// YYeung
// WinningTestTool.cs

using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

class EditorTestTool : EditorWindow
{
    #region UNITYMENU
    [MenuItem("Tool/Open Test Window")]

    public static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindow(typeof(EditorTestTool), false, "TTT Test");
        window.minSize = new Vector2(200,400);
    }
    #endregion // UNITYMENU


    #region DATA
    private TestData testData = new TestData();
    #endregion // DATA


    #region WINDOW
    void OnGUI()
    {
        if (!EditorApplication.isPlaying)
        {
            EditorGUILayout.HelpBox("Game needs to be in PlayMode for Testing Tool to activate.", MessageType.Warning);
            return;
        }
        // Title
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUIStyle style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        EditorGUILayout.LabelField("TicTacToe Tester", style, GUILayout.ExpandWidth(true));
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("Test Speed ");
        testData.speed = EditorGUILayout.Slider(testData.speed, 1, 10);
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
        testData.playerToWin = (TestData.PlayerToWin)EditorGUILayout.EnumPopup("Winning player", testData.playerToWin);
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
                break;
            case TestData.WinStyle.VERTICAL:
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Winning Column: ");
                testData.winIndex = EditorGUILayout.IntSlider(testData.winIndex, 0, sliderMax);
                EditorGUILayout.EndHorizontal();
                break;
            case TestData.WinStyle.DIAGONAL:
                    testData.diagonalOptions = (TestData.DiagonalOptions)EditorGUILayout.EnumPopup("Diagonal slash", testData.diagonalOptions);
                break;
            case TestData.WinStyle.DRAW:
                break;
        }

        // Results
        GuiSeparator("Results: ");
        if (testData.testInSession)
        {
            string results = ("Grid: " + testData.gridOptions.ToString() + "\nWinning player: " + testData.playerToWin + "\nWin style: " + testData.winStyle + GetWinOption());
            EditorGUILayout.HelpBox(results, MessageType.Info);
        }

        // Start the test
        GuiSeparator("Test: ");
        if (GUILayout.Button("Run Test"))
            RunTest();



    }
    #endregion

    #region PRIVATE
    void RunTest()
    {
        UnityEngine.Object testObject = Resources.Load("Prefabs/Testing/Test");
        GameObject test = (GameObject)Instantiate(testObject);

        testData.testInSession = true;
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

