using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int currentRound = 1;
    int currentPlayerTurnID;
    void Awake()
    {
        instance = this;
    }

    void StartTurn(int playerID)
    {
        currentPlayerTurnID = playerID;
        //PlayerManager.instance.AssignTurn(playerID);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
