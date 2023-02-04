using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    //Modifiables 
    public int countCard = 3; // the count of each card 
    public int handLaneCount = 3; // the count of each lane card in hand
    public int handModCount = 3; // the count of each mod card in hand
    //Internal
    Card[] cards;
    public List<Card> laneDeck = new List<Card>();
    public List<Card> modDeck = new List<Card>();

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        //Create Decks
        cards = Resources.FindObjectsOfTypeAll(typeof(Card)) as Card[];
        foreach (Card card in cards)
        {
            if(card.cardType == Card.CardType.Lane)
            {
                for (int i = 0; i < countCard; i++)
                {
                    laneDeck.Add(card);
                }

            }
            else if (card.cardType == Card.CardType.Mod)
            {
                for (int i = 0; i < countCard; i++)
                {
                    modDeck.Add(card);
                }
            }
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
