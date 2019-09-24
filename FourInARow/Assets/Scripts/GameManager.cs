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
    private Player CurrentPlayer;


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
        AssignCurrentPlayer();
        Play();
    }

    private void Play()
    {
        //GAME GOES HERE
    }

    private int iTurn()
    {
        return Convert.ToInt32(turn);
    }

    private void AssignCurrentPlayer()
    {
        CurrentPlayer = Players[iTurn()];
    }

}
