using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaneSide : MonoBehaviour
{
    public GameObject[] slots;
    public LaneMod mod;
    public int ownerId;
    public Image highLightImg;
    public bool Highlighted;
    public Image playerToken;

    int currentSlot = 0;
    float highlightVlaue = 0.3f;

    private void Awake()
    {
        highLightImg.color = new Color(highLightImg.color.r, highLightImg.color.g, highLightImg.color.b, 0);
        /*Image myImage = this.GetComponent<Image>();
        if (myImage)
        {
            float leftXPos = slots[0].GetComponent<RectTransform>().anchoredPosition.x * 1.5f;
            float RightXPos = slots[2].GetComponent<RectTransform>().anchoredPosition.x * 1.5f;

            myImage.raycastPadding = new Vector4(leftXPos, 0, RightXPos, 0);
        }
        */
    }
    private void Start()
    {
        playerToken.color = PlayerManager.instance.GetPlayerColor(ownerId);
        SetTokenSlot(0);
    }
    public void Hilight()
    {
        highLightImg.color = new Color(highLightImg.color.r, highLightImg.color.g, highLightImg.color.b, highlightVlaue);
        Highlighted = true;
    }
    public void Hide()
    {
        highLightImg.color = new Color(highLightImg.color.r, highLightImg.color.g, highLightImg.color.b, 0);
        Highlighted = false;
    }
    void SetTokenSlot(int slot)
    {
        if(slot <= 0)
        {
            float xPos =  slots[slot].GetComponent<RectTransform>().anchoredPosition.x;
            xPos = xPos * 1.4f; //Move the Token to the Center based on the position of the 1st slot
            RectTransform rectTransform = playerToken.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(xPos, rectTransform.anchoredPosition.y);
        }
        else
        {
            // Array is 0 but current location is 1 based
            float xPos = slots[slot-1].GetComponent<RectTransform>().anchoredPosition.x;
            RectTransform rectTransform = playerToken.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(xPos, rectTransform.anchoredPosition.y);
        }
    }

    public void IncreaceCurrentSlot()
    {
        //Todo handle out of bounds feedback
        currentSlot ++;
        if (currentSlot > 3)
        {
            currentSlot = 3;
        }
        SetTokenSlot(currentSlot);
    }
    public void ResetSlot()
    {
        currentSlot = 0;
        SetTokenSlot(currentSlot);
    }
    public void AddScore(Player player)
    {
        player.currentScore += currentSlot;
    }
}
