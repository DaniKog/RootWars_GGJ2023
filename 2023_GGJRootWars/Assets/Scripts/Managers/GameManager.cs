using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int currentRound = 1;
    int currentPlayerTurnID;
    void Awake()
    {
        instance = this;
    }

    public void StartTurn(int currentPlayerTurn)
    {
        foreach (Player player in PlayerManager.instance.players)
        {
            if (player.ID == currentPlayerTurn)
            {
                player.myTurn = true;
                if (player.turnCount == 0)
                {
                    CardManager.instance.DrawHand(player);
                }
            }
            else //TODO check if Currentplayer might be invalid
            {
                player.myTurn = false;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
