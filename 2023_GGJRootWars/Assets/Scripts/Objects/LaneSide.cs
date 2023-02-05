using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using static ak.wwise.core;

public class LaneSide : MonoBehaviour
{
    public GameObject[] slots;
    public List<Color> slotColors = new List<Color>();
    public LaneMod mod;
    public int ownerId;
    public Image highLightImg;
    public bool Highlighted;
    public Image playerToken;
    Lane myLane;
    int currentSlot = 0;
    float highlightVlaue = 0.3f;
    bool flushColor = false;
    float flushUpspeed = 50.0f;
    float flushDownspeed = 5.0f;
    float currentFlsuhVlaue = 0.0f;
    public Image loadingSlotImg;
    private void Awake()
    {
        highLightImg.color = new Color(highLightImg.color.r, highLightImg.color.g, highLightImg.color.b, 0);
        foreach (GameObject slot in slots)
        {
            Image image = slot.GetComponent<Image>();
            slotColors.Add(image.color);
            image.color = Color.gray;
        }

    }
    private void Start()
    {
        playerToken.color = PlayerManager.instance.GetPlayerColor(ownerId);
        myLane = gameObject.GetComponentInParent<Lane>();
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
    public void ResetAllColor()
    {
        foreach (GameObject slot in slots)
        {
            Image image = slot.GetComponent<Image>();
            image.color = Color.gray;
        }
    }
    public void EnableColor()
    {
        GameObject slotGameObj = slots[currentSlot - 1];
        Image image = slotGameObj.GetComponent<Image>();
        image.color = slotColors[currentSlot - 1];
    }
    void SetTokenSlot(int slot)
    {
        if (slot <= 0)
        {
            float xPos = slots[slot].GetComponent<RectTransform>().anchoredPosition.x;
            xPos = xPos * 1.4f; //Move the Token to the Center based on the position of the 1st slot
            RectTransform rectTransform = playerToken.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new UnityEngine.Vector2(xPos, rectTransform.anchoredPosition.y);
        }
        else
        {
            // Array is 0 but current location is 1 based
            float xPos = slots[slot - 1].GetComponent<RectTransform>().anchoredPosition.x;
            RectTransform rectTransform = playerToken.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new UnityEngine.Vector2(xPos, rectTransform.anchoredPosition.y);
        }
        // Set Layer
        myLane.SetLayer(this, slot);
    }

    public void IncreaceCurrentSlot()
    {
        //Todo handle out of bounds feedback
        currentSlot++;
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
    public int GetCurrentSlot()
    {
        return currentSlot;
    }

    public void FlushColor()
    {
        flushColor = true;
    }
    public void Update()
    {
        if (flushColor)
        {
            currentFlsuhVlaue += Time.deltaTime * flushUpspeed;

            for (int i = 0; i < slots.Length; i++)
            {
                Image image = slots[i].GetComponent<Image>();
                Color targetColor = slotColors[i];
                float r = Mathf.Lerp(Color.gray.r, targetColor.r, currentFlsuhVlaue);
                float g = Mathf.Lerp(Color.gray.g, targetColor.g, currentFlsuhVlaue);
                float b = Mathf.Lerp(Color.gray.b, targetColor.b, currentFlsuhVlaue);
                image.color = new Color(r,g,b);
            }
            
            if(currentFlsuhVlaue > 1)
            {
                flushColor = false;
            }

        }
        else if (currentFlsuhVlaue > 0)
        {
            currentFlsuhVlaue -= Time.deltaTime * flushDownspeed;

            for (int i = 0; i < slots.Length; i++)
            {
                Image image = slots[i].GetComponent<Image>();
                Color targetColor = slotColors[i];
                float r = Mathf.Lerp(Color.gray.r, targetColor.r, currentFlsuhVlaue);
                float g = Mathf.Lerp(Color.gray.g, targetColor.g, currentFlsuhVlaue);
                float b = Mathf.Lerp(Color.gray.b, targetColor.b, currentFlsuhVlaue);
                image.color = new Color(r, g, b);
            }
        }
    }

    internal void UpdateTransitioningImg()
    {
        LoadingSlotRelease();
        int slot = GetCurrentSlot();
        GameObject LoadingSlot = slots[slot - 1];
        loadingSlotImg = LoadingSlot.GetComponent<Image>();
        loadingSlotImg.color = PlayerManager.instance.GetPlayerColor(ownerId);
        loadingSlotImg.rectTransform.localScale = new UnityEngine.Vector3(1,0);
  
    }
    internal void LoadingSlotRelease()
    {
        if (loadingSlotImg)
        {
            loadingSlotImg.rectTransform.localScale = new UnityEngine.Vector3(1, 1);
            loadingSlotImg = null;
        }

    }

    public void VisualizeLoadingSlot()
    {
        if (loadingSlotImg )
        {
            float fillamount = AudioMaster.instance.currentSyncTime / AudioMaster.instance.lastSyncTookTime;
            if(fillamount <= 1 )
            {
                loadingSlotImg.rectTransform.localScale = new UnityEngine.Vector3(1, fillamount);
            }
        }
    }
}

