using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public List<Player> players = new List<Player>();
    public GameObject playerPrefab;
    GameObject CanvasObject;
    public static PlayerManager instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        CanvasObject = GameObject.FindGameObjectWithTag("Canvas");
        AddPlayer(0);
        GameManager.instance.StartTurn(0);

    }
    void AddPlayer(int playerID)
    {
        GameObject playerobj = Instantiate(playerPrefab);
        playerobj.transform.SetParent(CanvasObject.transform);
        playerobj.transform.position = new Vector3(0,0,0);
        RectTransform rectTransform = playerobj.GetComponent<RectTransform>();
        //rectTransform.position = new Vector2(0,-(Screen.height*0.8f/2));
        //rectTransform.position = new Vector2(0, 0);
        rectTransform.anchoredPosition = new Vector2(0, -((Screen.height / 2) * 0.75f ));
        Player player = playerobj.GetComponent<Player>();
        player.ID = playerID;
        player.modDeck = CardManager.instance.modDeck;
        player.laneDeck = CardManager.instance.laneDeck;
        players.Add(player);
    }
    
}
