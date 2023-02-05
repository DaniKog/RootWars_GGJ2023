using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Lane : MonoBehaviour
{
    public enum LaneType
    {
        Kick,
        Bass,
        Perc,
        Melody,
        Pad,
        None
    }
    public LaneType laneType;
    public LaneSide RSide;
    public LaneSide LSide;

}
