using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    /**********************************************************
     * GameManager is going to manage the starting,           *
     * rigging, turns, scoring, and ending of the game. It    *
     * is a general all-purpose director. As a Singleton, it  *
     * will destroy itself if not unique and can be accessed  *
     * through GameManager.Instance from any class.           *
     *********************************************************/

    public static GameManager Instance { get; private set; }

    private bool turn; //this game is for 2 players and 2 players only
    private class Player
    {
        public int ID, Score;
        public Color MyColor;

        public Player (int id, int score, Color color)
        {
            ID = id;
            Score = score;
            MyColor = color;
        }
    }
    private Player[] Players;


    //Check if instance is occupied:
    //If vacant, we're the first - occupy it and become invincible
    //If occupied, we're a copy - destroy self
    private void Awake()
    {
        if (Instance = null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        turn = false;
        Players = new Player[] { new Player(0, 0, new Color(255, 255, 255)), new Player(1, 0, new Color(255, 0, 0)) };
        Play();
    }

    private void Play()
    {
        //GAME GOES HERE
    }

    // Returns Turn as an int
    public int iTurn()
    {
        return Convert.ToInt32(turn);
    }

    public void ThisMoveWon()
    {
        Players[iTurn()].Score++;
        TurnCompleted();
    }

    public void TurnCompleted()
    {
        turn = !turn;
    }

    public bool getTurn()
    {
        return turn;
    }

    public Color getCurrentPlayerColor()
    {
        return Players[iTurn()].MyColor;
    }

    public Color getPlayerColor(bool player)
    {
        return Players[Convert.ToInt32(player)].MyColor;
    }

    public void ResetGame()
    {
        //???
    }
}
