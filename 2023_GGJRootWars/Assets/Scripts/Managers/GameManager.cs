using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        PreGame,
        DrawCards,
        PickCard,
        CardInPlayZone,
        CardPlayed,
        EndTurn,
        EndRound,
        GameEnd
    }

    public static GameManager instance;
    public Lane[] board;
    public int currentRound = 1;
    public Image currentPlayerTurn;
    public TextMeshProUGUI gameOverText;
    public GameObject gameOvergameObejct;
    public PlayerScoreUI[] playerScores;
    public Color[] playerColors;
    public AK.Wwise.Event[] playerPlacementEvents;
    int currentPlayerTurnID;
    int playerCount = 0;
    int turnCount = 1;
    int cardsPlayedThisTurn = 0;
    float scoreHightRaise = 8;
    GameState currentGameState = GameState.PreGame;
    void Awake()
    {
        instance = this;
        board = GameObject.FindObjectsOfType<Lane>();
        playerScores = GameObject.FindObjectsOfType<PlayerScoreUI>();
        SetGameState(GameState.PreGame);
    }
    void Start()
    {
        SetGameState(GameState.DrawCards);
    }
    public int GetNextPlayerID()
    {
        playerCount++;
        if (playerCount > 2)
        {
            //ASSERT uns`ported Player count!
        }
        return playerCount;

    }
    public Color GetPlayerColor()
    {
        switch (playerCount)
        {
            case 1:
                return playerColors[0];
            case 2:
                return playerColors[1];
            default:
                return Color.black;
        }
    }
    public AK.Wwise.Event GetPlayerPlacementEvent()
    {
        switch (playerCount)
        {
            case 1:
                return playerPlacementEvents[0];
            case 2:
                return playerPlacementEvents[1];
            default:
                return null;
        }
    }
    public void StartTurn(int playerID)
    {
        foreach (PlayerScoreUI scoreUI in playerScores)
        {
            scoreUI.textMeshPro.color = Color.white;
        }

        foreach (Player player in PlayerManager.instance.players)
        {
            if (player.Id == playerID)
            {
                player.myTurn = true;
                player.ShowHand();
                currentPlayerTurn.color = player.playerColor;
                foreach (PlayerScoreUI scoreUI in playerScores)
                {
                    if (scoreUI.PlayerId == player.Id)
                    {
                        scoreUI.textMeshPro.color = player.playerColor;
                    }
                }

            }
            else //TODO check if Currentplayer might be invalid
            {
                player.myTurn = false;
                player.HideHand();
            }
        }
        //TODO Handle Game State better
        SetGameState(GameState.PickCard);
        currentPlayerTurnID = playerID;
    }

    public void SetupPlayer(int PlayerID)
    {
        switch (playerCount)
        {
            case 1:
                foreach (Lane lane in board)
                {
                    lane.RSide.ownerId = PlayerID;
                    if(lane.laneType == Lane.LaneType.Kick)
                    {
                        playerScores[0].PlayerId = PlayerID;
                        RectTransform rectTransform = playerScores[0].gameObject.GetComponent<RectTransform>();
                        RectTransform kickSideTrnansofom = lane.RSide.gameObject.GetComponent<RectTransform>();
                        
                        rectTransform.anchoredPosition= new UnityEngine.Vector2(kickSideTrnansofom.anchoredPosition.x, (Screen.height * 0.5f) * 0.85f);    
                    }
                }
                break;
            case 2:
                foreach (Lane lane in board)
                {
                    lane.LSide.ownerId = PlayerID;
                    if (lane.laneType == Lane.LaneType.Kick)
                    {
                        playerScores[1].PlayerId = PlayerID;
                        RectTransform rectTransform = playerScores[1].gameObject.GetComponent<RectTransform>();
                        RectTransform kickSideTrnansofom = lane.LSide.gameObject.GetComponent<RectTransform>();
                        
                        rectTransform.anchoredPosition = new UnityEngine.Vector2(kickSideTrnansofom.anchoredPosition.x, (Screen.height * 0.5f) * 0.85f);
                    }
                }
                break;
            default:
                //Assert upsupported Player Count
                break;
        }
    }
    public void SetGameState(GameState gamestate)
    {
        currentGameState = gamestate;
        if (gamestate == GameState.DrawCards)
        {

            foreach (Player player in PlayerManager.instance.players)
            {
                CardManager.instance.DiscardCardHand(player.Id);
                if (player.laneDeck.Count > CardManager.instance.handLaneCount &&
                    player.modDeck.Count > CardManager.instance.handModCount)
                {
                    CardManager.instance.DrawHand(player);
                }
                else
                {
                    SetGameState(GameState.GameEnd);
                    return;
                }
            }
            int playerToStart = UnityEngine.Random.Range(0, PlayerManager.instance.players.Count - 1); // 0 to 1 
            //Determine the 1st turn
            if (currentRound != 1)
            {
                int higestScore = -1;
                int index = 0;
                foreach (Player player in PlayerManager.instance.players)
                {
                    if (player.currentScore > higestScore)
                    {
                        playerToStart = index;
                        higestScore = player.currentScore;
                    }
                    else if (player.currentScore == higestScore)
                    {
                        playerToStart = UnityEngine.Random.Range(0, PlayerManager.instance.players.Count);
                    }
                    index++;
                }
            }
            if (currentRound >= 2)
            {
                AudioMaster.instance.endGame = true;
            }

            StartTurn(PlayerManager.instance.players[playerToStart].Id);
        }

        if (gamestate == GameState.PickCard)
        {
            HideAllPossiblePlays();
        }

        if (gamestate == GameState.CardPlayed)
        {
            CalculateScore();
            cardsPlayedThisTurn++;
            if (cardsPlayedThisTurn >= 2)
            {
                turnCount++;
                if (turnCount > 4)
                {
                    SetGameState(GameState.EndRound);
                }
                else
                {
                    SetGameState(GameState.EndTurn);
                }
            }
            else
            {
                SetGameState(GameState.PickCard);
            }
        }

        if (gamestate == GameState.EndTurn)
        {
            CalculateScore();
            int otherPlayerId = PlayerManager.instance.GetOtherPlayerId(currentPlayerTurnID);
            StartTurn(otherPlayerId);
            cardsPlayedThisTurn = 0;
        }

        if (gamestate == GameState.EndRound)
        {
            currentRound++;
            CalculateScore();
            turnCount = 1;
            cardsPlayedThisTurn = 0;
            SetGameState(GameState.DrawCards);
        }

        if (gamestate == GameState.GameEnd)
        {
            CalculateScore();
            int higestScore = -1;
            int winnerPlayerindex = -1;
            int index = 0;
            foreach (Player player in PlayerManager.instance.players)
            {
                if (player.currentScore > higestScore)
                {
                    winnerPlayerindex = index;
                    higestScore = player.currentScore;
                }
                else if (player.currentScore == higestScore)
                {
                    winnerPlayerindex = 99;
                }
                index++;
            }
            /*
            if (winnerPlayerindex > PlayerManager.instance.players.Count)
            {
                gameOverText.text = "IT IS A TIE!";
            }
            else
            {
                gameOverText.text = "Winner is :" + winnerPlayerindex;
            }
            */
            gameOvergameObejct.SetActive(true);
        }
    }
    public void CalculateScore()
    {
        foreach (Player player in PlayerManager.instance.players)
        {
            player.currentScore = 0;
            foreach (Lane lane in board)
            {
                if (lane.RSide.ownerId == player.Id)
                {
                    lane.RSide.AddScore(player);
                }
                else if (lane.LSide.ownerId == player.Id)
                {
                    lane.LSide.AddScore(player);
                }
            }

            foreach (PlayerScoreUI scoreUI in playerScores)
            {
                if(scoreUI.PlayerId == player.Id)
                {
                    scoreUI.textMeshPro.text = "Score: " + player.currentScore;
                }
            }
        }


    }
    public GameState GetGameState()
    {
        return currentGameState;
    }
    public void ShowPossiblePlays(Card card)
    {
        //TODO handle all mods and special cards    
        if (card.cardType == Card.CardType.Lane)
        {
            foreach (Lane lane in board)
            {
                if (card.effectedLane == lane.laneType)
                {
                    if (lane.RSide.ownerId == card.ownerID)
                    {
                        lane.RSide.Hilight();
                    }
                    else if (lane.LSide.ownerId == card.ownerID)
                    {
                        lane.LSide.Hilight();
                    }
                }
            }
        }
    }
    public void HideAllPossiblePlays()
    {
        //TODO handle all mods and special cards    
        foreach (Lane lane in board)
        {
            if (lane.RSide.Highlighted)
            {
                lane.RSide.Hide();
            }
            if (lane.LSide.Highlighted)
            {
                lane.LSide.Hide();
            }
        }
    }
    public void RestartGame()
    {
        foreach (Lane lane in board)
        {
            lane.RSide.Hide();
            lane.RSide.ResetSlot();
            lane.LSide.ResetSlot();
            lane.LSide.Hide();
        }
        PlayerManager.instance.RedrawPlayerDecks();
        CalculateScore();
        gameOvergameObejct.SetActive(false);
        SetGameState(GameState.DrawCards);
        AudioMaster.instance.endGame = false;
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(GetGameState() == GameState.GameEnd)
            {
                RestartGame();
            }
        }
    }

    public Player.MusicType GetNextMusicType()
    {
        switch (playerCount)
        {
            case 1:
                return Player.MusicType.Roots;
            case 2:
                return Player.MusicType.Steppa;
            default:
                return Player.MusicType.Roots;
        }
    }
}
