using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoardLogic : MonoBehaviour
{
    private int[,] board;
    private int columns, rows;
    private int BackDiagTally, ForeDiagTally, HorTally, VerTally;

    // Start is called before the first frame update
    void Start()
    {
        ResetGame();
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
    private void SlotSelected(int c)
    {
        int SlotResult = FindLowestOpenSlot(c);
        if (SlotResult != -1)
        {
            board[SlotResult, c] = GameManager.Instance.iTurn();
            //ADD COLOUR TO SLOT AND UPDATE VISUALS BELOW


            //EVALUATE 4 IN A ROW BELOW
            /* >>>>[dv, dh]<<<<
         -> [-1, -1] [-1, 0] [-1, 1] -> F
         -> [0, -1]  [0, 0]  [0, 1] ->
          S [1, -1]  [1, 0]  [1, 1] ->
            */

            for (int v = SlotResult - 1; v <= SlotResult + 1; v++)
            { //double-nested forloop to iterate through the possibilities
                for (int h = c - 1; h <= c + 1; h++)
                {
                    if (IsInBounds(v, h) && !(v == SlotResult && h == c))
                    {
                        int tv = v;
                        int th = h;
                        int dv = v - SlotResult;
                        int dh = h - c;

                        //we're going to iterate through the slots in this direction until we either hit a slot without this player's piece in it, or we reach the end of the board
                        while (IsInBounds(tv, th) && board[tv, th] == GameManager.Instance.iTurn())
                        { //NEEDS CLEANUP: IsInBounds will be assessed first and cancel out on fail, so an out of bounds array check shouldn't happen, but it's still dodgy
                            SolveTallyDirection(dv, dh);
                            tv += dv;
                            th += dh;
                        }
                    }
                }
            }

            if (BackDiagTally >= 4 || ForeDiagTally >= 4 || HorTally >= 4 || VerTally >= 4)
            { //that's a bingo
                GameManager.Instance.ThisMoveWon();
            }
            else
            {
                ResetTallies();
                GameManager.Instance.TurnCompleted();
            }
        }
    }

    private bool IsInBounds(int r, int c)
    {
        return (!(r < 0 || c < 0 || r > rows - 1 || c > columns - 1));
    }

    private void SolveTallyDirection(int r, int c)
    {
        int hash = r * 10 + c;
        switch (hash)
        {
            case 11:
            case -11:
                BackDiagTally++;
                break;
            case 10:
            case -10:
                VerTally++;
                break;
            case 9:
            case -9:
                ForeDiagTally++;
                break;
            case 1:
            case -1:
                HorTally++;
                break;
            default:
                break;
        }
    }

    public void AssignColorToSlot()
    {
        GameManager.Instance.getCurrentPlayerColor(); //this will have to be assigned to the slot piece
    }

    private void ResetTallies()
    {
        BackDiagTally = 1; ForeDiagTally = 1; HorTally = 1; VerTally = 1;
    }

    private void ResetGame()
    {
        rows = 6; columns = 7;
        board = new int[6, 7];
        ResetTallies();
        ClearBoard();
    }

    // Both sets and resets the game board
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
