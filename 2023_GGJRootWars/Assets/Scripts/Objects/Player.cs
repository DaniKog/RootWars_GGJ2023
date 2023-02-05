using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public AK.Wwise.Event placeMentEvent;
    public enum MusicType
    {
        Roots,
        Steppa
    }
    public MusicType muiscType;
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

    internal void PlayPlacementEvent()
    {
        placeMentEvent.Post(gameObject);
    }
}
