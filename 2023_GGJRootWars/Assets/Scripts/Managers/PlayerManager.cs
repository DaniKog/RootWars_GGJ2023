using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class PlayerManager : MonoBehaviour
{
    public List<Player> players = new List<Player>();
    public GameObject playerPrefab;
    GameObject CanvasObject;
    public static PlayerManager instance;
    private void Awake()
    {
        instance = this;
        CanvasObject = GameObject.FindGameObjectWithTag("Canvas");
        AddPlayer();
        AddPlayer();
    }
    private void Start()
    {

    }
    int AddPlayer()
    {
        GameObject playerobj = Instantiate(playerPrefab);
        playerobj.transform.SetParent(CanvasObject.transform);
        playerobj.transform.position = new Vector3(0, 0, 0);
        RectTransform rectTransform = playerobj.GetComponent<RectTransform>();
        //rectTransform.position = new Vector2(0,-(Screen.height*0.8f/2));
        //rectTransform.position = new Vector2(0, 0);
        rectTransform.anchoredPosition = new Vector2(0, -((Screen.height / 2) * 0.75f));
        Player player = playerobj.GetComponent<Player>();
        int playerId = GameManager.instance.GetNextPlayerID();
        Color playerColor = GameManager.instance.GetPlayerColor();
        player.Id = playerId;
        player.placeMentEvent = GameManager.instance.GetPlayerPlacementEvent();
        player.muiscType = GameManager.instance.GetNextMusicType();
        player.playerColor = playerColor;
        player.modDeck = new List<Card>(CardManager.instance.GetModDeck());
        player.laneDeck = new List<Card>(CardManager.instance.GetLaneDeck());
        players.Add(player);
        GameManager.instance.SetupPlayer(playerId);
        return playerId;
    }
    public Color GetPlayerColor(int playerID)
    {
        foreach (Player player in players)
        {
            if (player.Id == playerID)
            {
                return player.playerColor;
            }
        }
        return Color.black;
    }
    public Player GetPlayerByID(int id)
    {
        foreach (Player player in players)
        {
            if (player.Id == id)
            {
                return player;
            }
        }
        return null;
    }
    public int GetOtherPlayerId(int id)
    {
        foreach (Player player in players)
        {
            if (player.Id != id)
            {
                return player.Id;
            }
        }
        return -1;
    }
    public void RedrawPlayerDecks()
    {
        foreach (Player player in players)
        {
            player.modDeck = new List<Card>(CardManager.instance.GetModDeck());
            player.laneDeck = new List<Card>(CardManager.instance.GetLaneDeck());
        }
    }
    public MusicType GetPlayerMusic(int playerID)
    {
        foreach (Player player in players)
        {
            if (player.Id == playerID)
            {
                return player.muiscType;
            }
        }
        return MusicType.Roots;
    }
}
