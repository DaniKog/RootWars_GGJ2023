using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int ID;
    public bool myTurn = false;
    int currentScore = 0;
    public int turnCount = 0;
    public List<Card> hand = new List<Card>();
    public List<Card> laneDeck = new List<Card>();
    public List<Card> modDeck = new List<Card>();
}
