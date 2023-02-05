using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    public static CardManager instance;

    //Modifiables 
    public int countCard = 3; // the count of each card 
    public int handLaneCount = 3; // the count of each lane card in hand
    public int handModCount = 3; // the count of each mod card in hand
    public float spaceBetweenCard = 0.1f;
    public GameObject cardContainder;
    //Internal
    Card[] cards;
    List<Card> laneDeck = new List<Card>();
    List<Card> modDeck = new List<Card>();

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        //Create Decks
        cards = cardContainder.GetComponentsInChildren<Card>();
        //cards = Resources.FindObjectsOfTypeAll<Card>();
        foreach (Card card in cards)
        {
            if (card.cardType == Card.CardType.Lane)
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
            DrawCard(player, Card.CardType.Mod);
        }

        UpdateHand(player);
        player.HideHand();
    }

    void DrawCard(Player player, Card.CardType cardtype)
    {
        int randomNumber = UnityEngine.Random.Range(0, player.laneDeck.Count-1);
        if (cardtype == Card.CardType.Lane)
        {
            Card card = player.laneDeck[randomNumber];
            
            Card playerCard = Instantiate(card);
            Image cardImg = playerCard.GetComponentInChildren<Image>();
            cardImg.color = player.playerColor;
            playerCard.transform.SetParent(player.transform);
            RectTransform rectTransform = playerCard.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            playerCard.ownerID = player.Id;
            player.hand.Add(playerCard);
            player.laneDeck.RemoveAt(randomNumber);
        }

        if (cardtype == Card.CardType.Mod)
        {
            Card card = player.modDeck[randomNumber];

            Card playerCard = Instantiate(card);
            playerCard.transform.SetParent(player.transform);
            RectTransform rectTransform = playerCard.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;
            player.hand.Add(playerCard);
            player.laneDeck.RemoveAt(randomNumber);
        }
        else
        {
            //ASSERT
        }
    }

    public void UpdateHand(Player player)
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
    public void PlayedCard(Card card, LaneSide landSide)
    {
        Player Ownerplayer = PlayerManager.instance.GetPlayerByID(card.ownerID);
        Ownerplayer.PlayPlacementEvent();
        //TODO add mods
        if (card.cardType == Card.CardType.Lane)
        {
            landSide.IncreaceCurrentSlot();
            DiscardCard(card);
        }
        ScreenShakerManager.instance.TriggerShake(0.2f, landSide.GetCurrentSlot());
        //aound

    }

    private void DiscardCard(Card card)
    {
        Player player = PlayerManager.instance.GetPlayerByID(card.ownerID);
        player.hand.Remove(card);
        Destroy(card.gameObject);
        if (player.hand.Count > 0)
        {
            UpdateHand(player);
        }
    }
    public void DiscardCardHand(int playerId)
    {
        Player player = PlayerManager.instance.GetPlayerByID(playerId);
        while (player.hand.Count > 0)
        {
            DiscardCard(player.hand[0]);
        }
    }

    internal List<Card> GetModDeck()
    {
        return modDeck;
    }

    internal List<Card> GetLaneDeck()
    {
        return laneDeck;
    }
}
