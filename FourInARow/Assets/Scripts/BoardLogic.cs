using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardLogic : MonoBehaviour
{
    private int[,] board;
    private int columns, rows;

    // Start is called before the first frame update
    void Start()
    {
        rows = 6; columns = 7;
        board = new int[6, 7];
        ClearBoard();
    }

    // Since we'll have a column value, all we need to return is the lowest unoccupied slot
    // If the column is occupied, a -1 will be sent as an error value
    private int FindLowestOpenSlot(int c)
    { //arrays start at the top element, so the row index # increases as you go down a 2D array
        for (int r = rows-1; r >= 0; r--) //iterate through the rows of this column, starting at the bottom
        {
            if (board[r, c] == -1) //if it's empty, then put the piece there and we're done
            {
                return r;
            }
        }

        //this last return can't be reached unless the forloop exits out without placing a piece, eg: full
        return -1;
    }


    /********************************************** PLACEHOLDER FUNCTION ************************************************
     * A slot has been clicked, we're now ready to try and drop a piece into that column and see if there are any       *
     * 4-in-a-row victories for this player. It is a good design philosophy to make all slots "clickable" and just      *
     * place the piece in the selected slot's column, since players only actually choose their column and the row       *
     * is inferred. Gameplay is less restrictive this way.                                                              *
     * We start by taking the column and finding the lowest open slot. If none are available, the column is full        *
     * and we can ignore the action and keep waiting for the player to make a legitimate move. If there is a slot       *
     * available then we assign it to the player that claimed it and update its colour.                                 *
     * If successful, we have to assess if there is a win - the bulk of the work. Our algorithm will be a spot-check    *
     * of the 3, 5, or 8 positions available around the piece we placed: we only evaluate if the latest piece placed    *
     * is capable of scoring a win, and only based on the pieces around it. No need to evaluate the whole board.        *
     * The check will iterate through all positions around the placed piece to see if any are owned by the same player; *
     * if so, then each match will be assessed by its relative position to the placed piece                             *
     * (vertical, horizontal, diagonal). We then simply walk our check along the grid positions in both directions      *
     * of that axis, iterating our consecutive pieces owned by that player and stopping at the first empty or           *
     * opponent-owned slot. If we count up to four (first piece included) then that is a win and we can tell the        *
     * GameManager, otherwise we can go ahead and check if that piece filled up the board by checking the occupancy of  *
     * the top row, in which case it's a draw.                                                                          *
     * If the game isn't over after all that, then we're good to tell the GM to iterate the turn and wait for new input.*
     *                                                                                                                  *
     * 2     1     3    4 = WIN                                                                                         *
     * o <- |o| -> o -> o                                                                                               *
     *                                                                                                                  *
     * 2     1     F    _ = FAIL (only 2 in a row)                                                                      *
     * o <- |o| -> x    o    o                                                                                          *
     *                                                                                                                  *
     ********************************************************************************************************************/
    private void SlotSelected(int r, int c)
    {
        int SlotResult = FindLowestOpenSlot(c);
        if (FindLowestOpenSlot(c) != -1)
        {
            board[r, SlotResult] = GameManager.Instance.iTurn();
            //ADD COLOUR TO SLOT BELOW

            //EVALUATE 4 IN A ROW BELOW



            //CLEANUP AND POSTAMBLE BELOW


        }
    }

    public void AssignColorToSlot()
    {
        GameManager.Instance.getCurrentPlayerColor(); //this will have to be assigned to the slot piece
    }


    // Both sets and resets the game
    private void ClearBoard()
    {
        for (int r = 0; r <= rows - 1; r++)
        {
            for (int c = 0; c <= columns - 1; c++)
            {
                board[r, c] = -1;
            }
        }
    }
}
