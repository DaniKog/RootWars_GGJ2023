using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int Id;
    public bool myTurn = false;
    public int currentScore = 0;
    public int turnCount = 0;
    public Color playerColor = Color.black;
    public List<Card> hand = new List<Card>();
    public List<Card> laneDeck = new List<Card>();
    public List<Card> modDeck = new List<Card>();

    public void HideHand()
    {
        gameObject.SetActive(false);
    }
    public void ShowHand()
    {
        gameObject.SetActive(true);
    }
}
