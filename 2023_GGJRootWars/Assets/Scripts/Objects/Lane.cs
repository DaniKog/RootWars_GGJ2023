using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Player;

public class Lane : MonoBehaviour
{
    public enum LaneState
    {
        None,
        Playing,
        Transitioning,
    }
    public enum LaneType
    {
        Kick,
        Bass,
        Perc,
        Melody,
        Pad,
        None
    }
    public enum Side
    {
        Left,
        Right,
    }
    public LaneType laneType;
    public LaneState currentLaneState;
    public LaneSide RSide;
    public LaneSide LSide;
    public string stateGroup;
    public LaneSide currentPlayingLane;
    public bool visuzliationGrow;
    float growthSpeed = 0.25f;
    public void Awake()
    {
        switch (laneType)
        {
            case LaneType.Kick:
                stateGroup = "Music_KickSnare";
                break;
            case LaneType.Bass:
                stateGroup = "Music_Bass";
                break;
            case LaneType.Perc:
                stateGroup = "Music_Perc";
                break;
            case LaneType.Melody:
                stateGroup = "Music_Melody";
                break;
            case LaneType.Pad:
                stateGroup = "Music_Pad";
                break;
            case LaneType.None:
                break;
            default:
                break;
        }
    }
    public void SetLayer(LaneSide laneSide, int value)
    {
        Player.MusicType musicType = PlayerManager.instance.GetPlayerMusic(laneSide.ownerId);
        string stateName = "";
        if (value > 0)
        {
            switch (musicType)
            {
                case MusicType.Roots:
                    stateName = "Roots";
                    break;
                case MusicType.Steppa:
                    stateName = "Steppa";
                    break;

                default:
                    break;
            }
            stateName = stateName + value.ToString();
            //Rest previous lane
            if (currentPlayingLane)
            {
                currentPlayingLane.playerToken.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                currentPlayingLane.playerToken.color = new Color(
                    currentPlayingLane.playerToken.color.r,
                    currentPlayingLane.playerToken.color.g,
                    currentPlayingLane.playerToken.color.b, 1);
                currentPlayingLane.LoadingSlotRelease();
            }
            currentPlayingLane = laneSide;
            currentPlayingLane.playerToken.color = new Color(
            currentPlayingLane.playerToken.color.r,
            currentPlayingLane.playerToken.color.g,
            currentPlayingLane.playerToken.color.b, 0.5f);
            SetState(LaneState.Transitioning);
            //currentPlayingLane.EnableColor();

        }
        else
        {
            stateName = "None";
            if (currentPlayingLane)
            {
                currentPlayingLane.playerToken.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                currentPlayingLane.playerToken.color = new Color(
                    currentPlayingLane.playerToken.color.r,
                    currentPlayingLane.playerToken.color.g,
                    currentPlayingLane.playerToken.color.b, 1);
                currentPlayingLane.LoadingSlotRelease();
            }
            currentPlayingLane = laneSide;

            currentPlayingLane.playerToken.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            currentPlayingLane.playerToken.color = new Color(
                currentPlayingLane.playerToken.color.r,
                currentPlayingLane.playerToken.color.g,
                currentPlayingLane.playerToken.color.b, 1);
            currentPlayingLane.LoadingSlotRelease();
            SetState(LaneState.None);
        }

        AudioMaster.instance.SetState(stateGroup, stateName);

    }
    public void Update()
    {
        if (currentPlayingLane)
        {
            if (currentLaneState == LaneState.Playing)
            {
                Transform tokenTransform = currentPlayingLane.playerToken.transform;
                float growth = Time.deltaTime * growthSpeed;
                if (visuzliationGrow)
                {
                    tokenTransform.localScale = new(tokenTransform.localScale.x + growth, tokenTransform.localScale.y + growth);
                }
                else
                {
                    tokenTransform.localScale = new(tokenTransform.localScale.x - growth, tokenTransform.localScale.y - growth);
                }
            }

            if (currentLaneState == LaneState.Transitioning)
            {
                currentPlayingLane.VisualizeLoadingSlot();
                /*
                Color tokenColor = currentPlayingLane.playerToken.color;
                if (visuzliationGrow)
                {
                    currentPlayingLane.playerToken.color = new Color(tokenColor.r, tokenColor.g, tokenColor.b, tokenColor.a + Time.deltaTime);
                }
                else
                {
                    currentPlayingLane.playerToken.color = new Color(tokenColor.r, tokenColor.g, tokenColor.b, tokenColor.a - Time.deltaTime);
                }
                */
            }
        }


    }
    public void SetState(LaneState newState)
    {
        if (currentLaneState == LaneState.Transitioning && newState == LaneState.Playing)
        {
            currentPlayingLane.playerToken.transform.localScale = Vector3.one;
            currentPlayingLane.playerToken.color = new Color(
                currentPlayingLane.playerToken.color.r,
                currentPlayingLane.playerToken.color.g,
                currentPlayingLane.playerToken.color.b, 1);
        }
        currentLaneState = newState;

        if(newState== LaneState.Playing)
        {
            currentPlayingLane.LoadingSlotRelease();
        }

        if (newState == LaneState.Transitioning)
        {
            currentPlayingLane.UpdateTransitioningImg();
        }
    }

    public void FlushColors()
    {
        RSide.FlushColor();
        LSide.FlushColor();
    }
}
