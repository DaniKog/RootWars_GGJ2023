using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    List<Player> players = new List<Player>();
    public GameObject playerPrefab;
    public static PlayerManager instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        //AddPlayer(0);
        
        //AssignTurn(0);

    }
    void AddPlayer(int playerID)
    {
        GameObject playerobj = Instantiate(playerPrefab);
        playerobj.transform.localPosition = new Vector3(Screen.width / 2, 0, 0);
        Player player = playerobj.GetComponent<Player>();
        player.ID = playerID;
        player.modDeck = CardManager.instance.modDeck;
        player.laneDeck = CardManager.instance.laneDeck;
        //TODO REMOVE From here
        Card card = DrawCard(player, Card.CardType.Lane);
        Card gameObject = Instantiate(card);
        player.hand.Add(gameObject);
    }
    void AssignTurn(int currentPlayerTurn)
    {
        foreach (Player player in players)
        {
            if(player.ID == currentPlayerTurn)
            {
                player.myTurn = true;
                if(player.turnCount == 0)
                {
                    DrawHand(player);
                }
            }
            else //TODO check if Currentplayer might be invalid
            {
                player.myTurn = false;
            }
        }
    }
    void DrawHand(Player player)
    {
        //clear hand just in case 
        player.hand.Clear();
        for (int i = 0; i < CardManager.instance.handLaneCount; i++)
        {
            Card card = DrawCard(player, Card.CardType.Lane);
            Card gameObject = Instantiate(card);
            player.hand.Add(gameObject);
        }

        for (int i = 0; i < CardManager.instance.handModCount; i++)
        {
            Card card = DrawCard(player, Card.CardType.Lane);
        }
    }
    
    Card DrawCard(Player player,Card.CardType cardtype)
    {
        int randomNumber = UnityEngine.Random.Range(0, player.laneDeck.Count);
        if (cardtype == Card.CardType.Lane)
        {
            Card card = player.laneDeck[randomNumber];
            player.laneDeck.RemoveAt(randomNumber);
            return card;
        }

        if (cardtype == Card.CardType.Mod)
        {
            Card card = player.laneDeck[randomNumber];
            player.laneDeck.RemoveAt(randomNumber);
            return card;
        }
        else
        {
            return null;
        }
       
    }
}
