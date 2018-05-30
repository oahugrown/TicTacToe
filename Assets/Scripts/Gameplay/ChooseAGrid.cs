using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ChooseAGrid : MonoBehaviour
{
    #region DATA
    public enum GridType
    {
        threeByThree = 0,
        fourByFour,
        length
    }
    private GridType gridType = GridType.threeByThree;
    public GameObject[] gridPrefabs;
    public Button[] gridButtons;

    #endregion // DATA

    public GridType GetGridType() { return gridType; }

    private void Awake()
    {
        GetComponent<Canvas>().enabled = false;
    }

    #region UICONTROLS
    public void ChangeGridType(int grid)
    {
        // If the selection is the same, then bail.
        if (grid == (int)gridType)
            return;

        // Disable selection
        for (int i = 0; i < (int)GridType.length; ++i)
        {
            gridButtons[i].GetComponent<Outline>().enabled = false;
        }

        // Assign the new type
        gridButtons[grid].GetComponent<Outline>().enabled = true;
        gridType = (GridType)grid;
    }

    public void ContinueButton()
    {
        GetComponent<Canvas>().enabled = false;
        GameObject gameCanvas = GameObject.FindWithTag("GameCanvas");
        gameCanvas.GetComponent<Canvas>().enabled = true;
        Instantiate(gridPrefabs[(int)gridType], gameCanvas.transform);
    }

    #endregion // UICONTROLS
}
