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
    public Sprite picture;

    Vector3 originalPosition;
    int ownerID;
    //public Sprite cardOutline;

    void Awake()
    {
        TextMeshProUGUI myTextMesh = this.GetComponentInChildren<TextMeshProUGUI>();
        if (myTextMesh)
        {
            myTextMesh.SetText(cardName);
        }
        
        Image myImage = this.GetComponentInChildren<Image>();
        if(myImage)
        {
            myImage.sprite = picture;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("OnMouseExit");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnMouseDrag");
        originalPosition = transform.position;
        // TODO Reparent for sorting ?
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnMouseUp");
        transform.position = originalPosition;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        transform.position = eventData.position;
    }
}
