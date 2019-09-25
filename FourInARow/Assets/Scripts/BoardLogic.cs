using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardLogic : MonoBehaviour
{
    private int[,] board;
    private int columns, rows, vertical, horizontal;

    // Start is called before the first frame update
    void Start()
    {
        vertical = (int)Camera.main.orthographicSize;
        horizontal = vertical * (Screen.width / Screen.height);
        rows = 5; columns = 6;
        board = new int[6, 7];

        for (int r = 0; r >= rows; r++)
        {
            for (int c = 0; c >= columns; c++)
            {
                board[r, c] = -1;
            }
        }


    }

    private void SpawnSlot(int x, int y)
    {
        GameObject g = new GameObject("X: " + x + " Y: " + y);
    }

    void ClearBoard()
    {
        
    }
}
