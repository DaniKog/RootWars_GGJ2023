using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    //Modifiables 
    public int countCard = 3; // the count of each card 
    public int handLaneCount = 3; // the count of each lane card in hand
    public int handModCount = 3; // the count of each mod card in hand
    public float spaceBetweenCard = 0.1f;

    //Internal
    Card[] cards;
    public List<Card> laneDeck = new List<Card>();
    public List<Card> modDeck = new List<Card>();

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        //Create Decks
        cards = Resources.FindObjectsOfTypeAll<Card>();
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
    public void DrawHand(Player player)
    {
        //clear hand just in case 
        player.hand.Clear();
        for (int i = 0; i < CardManager.instance.handLaneCount; i++)
        {
            DrawCard(player, Card.CardType.Lane);
        }

        for (int i = 0; i < CardManager.instance.handModCount; i++)
        {
            DrawCard(player, Card.CardType.Lane);
        }

        UpdateHand(player);
    }

    void DrawCard(Player player, Card.CardType cardtype)
    {
        int randomNumber = UnityEngine.Random.Range(0, player.laneDeck.Count);
        if (cardtype == Card.CardType.Lane)
        {
            Card card = player.laneDeck[randomNumber];
            
            Card gameObject = Instantiate(card);
            gameObject.transform.SetParent(player.transform);
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            player.hand.Add(gameObject);
            player.laneDeck.RemoveAt(randomNumber);
        }

        if (cardtype == Card.CardType.Mod) 
        {
            Card card = player.laneDeck[randomNumber];
            
            Card gameObject = Instantiate(card);
            gameObject.transform.SetParent(player.transform);
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            player.hand.Add(gameObject);
            player.laneDeck.RemoveAt(randomNumber);
        }
        else
        {
           //ASSERT
        }
    }

    void UpdateHand(Player player)
    {
        int handsize = player.hand.Count;
        //TODO Fix this seems scary
        int cardWidth = player.hand[0].picture.texture.width;
        float cardWidthWithSpace = cardWidth + (cardWidth * spaceBetweenCard);
        float cardSpace = (cardWidthWithSpace * handsize);
        float handX = (cardWidth * 0.5f) - (cardSpace * 0.5f);
        for (int i = 0; i < handsize; i++)
        {
            Card card = player.hand[i];
            int imgWidth = card.picture.texture.width;
            RectTransform rectTransform = card.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(handX, 0);
            handX += cardWidthWithSpace;
        }
    }
}
