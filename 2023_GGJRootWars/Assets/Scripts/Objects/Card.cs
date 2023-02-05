using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

[System.Serializable]
public class Card : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler
{
    public enum CardType
    {
        Lane,
        Mod
    }
    public string cardName,tooltip;
    public CardType cardType;
    public Lane.LaneType effectedLane;
    public Sprite picture;

    float screenEnterPlayAreaPresentage = 0.35f;
    Vector3 originalPosition;
    public int ownerID;
    Image myImage;
    float screenEnterPlayAreaY = 0;
    float cardEnterdPlayAreaAlphaValue = 0.5f;
    float fadeInMulipler = 4.0f;
    float fadeOutMulipler = 2.0f;
    bool isInPlayArea = false;
    bool gobackIntoHand = false;
    bool updateFade = false;
    float gobackIntoHandSpeed = 2.0f;
    float gobackIntoHandDistanceMultipler = 8.0f;
    float gobackIntoHandSpeedPadding = 200.0f;
    void Awake()
    {
        TextMeshProUGUI myTextMesh = this.GetComponentInChildren<TextMeshProUGUI>();
        if (myTextMesh)
        {
            myTextMesh.SetText(cardName);
        }
        
        myImage = this.GetComponentInChildren<Image>();
        if(myImage)
        {
            myImage.sprite = picture;
            myImage.raycastTarget = true;
        }
        screenEnterPlayAreaY = Screen.height * screenEnterPlayAreaPresentage;
    }
    void Update()
    {
        if(updateFade)
        {
            if (isInPlayArea)
            {
                if (myImage.color.a > cardEnterdPlayAreaAlphaValue)
                {
                    float deltatime = Time.deltaTime;
                    float newAlpha = myImage.color.a - myImage.color.a * deltatime * fadeInMulipler;
                    myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, newAlpha);
                }
                else
                {
                    updateFade = false;
                }
            }
            else
            {
                if (myImage.color.a < 1)
                {
                    float deltatime = Time.deltaTime;
                    float newAlpha = myImage.color.a + myImage.color.a * deltatime * fadeOutMulipler;
                    myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, newAlpha);
                }
                else
                {
                    updateFade = false;
                }
            }
        }


        if(gobackIntoHand)
        {
            if(transform.position != originalPosition)
            {
                float moveThisFrame = gobackIntoHandDistanceMultipler * gobackIntoHandSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, originalPosition, moveThisFrame);
            }
            else
            {
                gobackIntoHand = false;
                transform.position = originalPosition;
            }
        }

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Enter");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("OnMouseExit");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.GameEnd)
            return;
        //Debug.Log("OnMouseDrag");
        originalPosition = transform.position;
        myImage.raycastTarget = false;
        // TODO Reparent for sorting ?
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.GameEnd)
            return;
        myImage.raycastTarget = true;
        float distance = Vector3.Distance(originalPosition, transform.position);
        gobackIntoHandDistanceMultipler = (distance + gobackIntoHandSpeedPadding); // Padd lower values to gain more speed 
        gobackIntoHand = true;
        bool playedCard = false;
        Debug.Log(eventData.pointerEnter);
        if(eventData.pointerEnter)
        {
            Lane lane = eventData.pointerEnter.GetComponentInParent<Lane>();
            if (lane.laneType == effectedLane)
            {
                Debug.Log("PlayedCard!");
                LaneSide laneSide = eventData.pointerEnter.GetComponent<LaneSide>();
                playedCard = true;
                CardManager.instance.PlayedCard(this, laneSide);
                GameManager.instance.SetGameState(GameManager.GameState.CardPlayed);
                //PlayerManager.instance.playedCard();
            }
        }

        if (!playedCard && GameManager.instance.GetGameState() == GameManager.GameState.CardInPlayZone)
        {
            GameManager.instance.SetGameState(GameManager.GameState.PickCard);
            isInPlayArea = false;
            updateFade = true;
        }

    }
    public void OnDrag(PointerEventData eventData)
    {
        if (GameManager.instance.GetGameState() == GameManager.GameState.GameEnd)
            return;
        //Debug.Log("OnDrag");
        Vector3 currentPos = eventData.position;
        transform.position = currentPos;

        if(currentPos.y > screenEnterPlayAreaY)
        {
            if(GameManager.instance.GetGameState() == GameManager.GameState.PickCard)
            {
                GameManager.instance.SetGameState(GameManager.GameState.CardInPlayZone);
                GameManager.instance.ShowPossiblePlays(this);
                isInPlayArea = true;
                updateFade = true;
            }
        }
        else
        {
            if (GameManager.instance.GetGameState() == GameManager.GameState.CardInPlayZone)
            {
                GameManager.instance.SetGameState(GameManager.GameState.PickCard);
                isInPlayArea = false;
                updateFade = true;
                GameManager.instance.HideAllPossiblePlays();
            }
        }    
    }
}
