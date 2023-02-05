using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMaster : MonoBehaviour
{
    // Start is called before the first frame update
    public static AudioMaster instance;
    //public AK.Wwise.Event MusicEvent;
    // Use this for initialization.
    public bool skipbeat = false;
    public bool endGame = false;
    public float lastSyncTookTime = 12.5f;
    public float currentSyncTime = 12.5f;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //AkSoundEngine.SetState("Music_Bass", "Roots1");
        //AkSoundEngine.SetState("Music_KickSnare", "Roots1");
        //AkSoundEngine.SetState("Music_Melody", "Roots1");
        //AkSoundEngine.SetState("Music_Pad", "Roots1");
        //AkSoundEngine.SetState("Music_Perc", "Roots1");
        //MusicEvent.Post(gameObject, (uint)AkCallbackType.AK_MusicSyncBar, CallbackFunction);
    }

    // Update is called once per frame
    void Update()
    {
        currentSyncTime += Time.deltaTime;
    }
    public void CallbackFunction(object in_cookie, AkCallbackType in_type, object in_info)
    {
        if(in_type == AkCallbackType.AK_MusicSyncBeat) 
        {
            Debug.Log("Beat!");
        }

        if (in_type == AkCallbackType.AK_MusicSyncEntry)
        {
            Debug.Log("Entry!");
        }
    }
    public void SetState(string stateGroup, string state)
    {
        AkSoundEngine.SetState(stateGroup, state);
    }
    public void OnMusicBeat()
    {
        if(endGame || !skipbeat)
        {
            bool anyMusicplaying = false;
            int numberOfLanes = 0;
            foreach (Lane lane in GameManager.instance.board)
            {
                lane.visuzliationGrow = !lane.visuzliationGrow;
                if (lane.currentLaneState == Lane.LaneState.Playing)
                {
                    lane.FlushColors();
                    anyMusicplaying = true;
                    numberOfLanes++;
                }
            }

            if (anyMusicplaying)
            {
                ScreenShakerManager.instance.TriggerShake(0.1f* numberOfLanes, 0.8f * numberOfLanes);
            }
            skipbeat = !skipbeat;
        }
        else
        {
            skipbeat = !skipbeat;
        }

    }

    public void OnMusicSyncEntry()
    {
        //Update Sync time
        lastSyncTookTime = currentSyncTime;
        currentSyncTime = 0;

        float shakeDuraction = 0 ;
        float shakeMagnitute = 0 ;
        foreach (Lane lane in GameManager.instance.board)
        {
            if(lane.currentLaneState == Lane.LaneState.Transitioning)
            {
                
                lane.SetState(Lane.LaneState.Playing);
                shakeDuraction += 0.2f;
                shakeMagnitute += 5.1f;
            }
        }

        if (shakeDuraction > 0) 
        {
            ScreenShakerManager.instance.TriggerShake(shakeDuraction, shakeMagnitute);
        }
        
    }
}
