using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MusicManager : MonoBehaviour
{

    static MusicManager _instance = null;

    public static MusicManager instance
    {
        get { return _instance; }
        set { _instance = value; }
    }
    private void OnLevelWasLoaded(int level)
    {


        switch (level)
        {

            case 0:

                if (aud.clip != MainTheme)
                {
                    aud.clip = MainTheme;
                    aud.pitch = MainThemePitch;
                    aud.Play();
                } 
                break;
            case 1:
                break;
            case 2:
                aud.clip = VillageTheme;
                aud.pitch = VillageThemePitch;
                aud.Play();
                break;
            case 3:
                aud.clip = VillageTheme;
                aud.pitch = VillageThemePitch;
                aud.Play();
                break;
            case 4:
                aud.clip = VillageTheme;
                aud.pitch = VillageThemePitch;
                aud.Play();
                break;
            case 5:
                //player lose
                aud.clip = MainTheme;
                aud.pitch = losethemepitch;
                aud.Play();
                break;
            case 6:
                //player win
                aud.clip = MainTheme;
                aud.pitch = winthemepitch;
                aud.Play();
                break;
            case 7:
              //credits
                break;

            default:
                aud.clip = MainTheme;
                aud.pitch = MainThemePitch;
                aud.Play();
                break;
        }
    }

    public AudioSource aud;

    public AudioClip MainTheme;
    public float MainThemePitch;
    public AudioClip VillageTheme;
    public float VillageThemePitch;

    public float winthemepitch;
    public float losethemepitch;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (instance)
            DestroyImmediate(gameObject);
        else
        {
            DontDestroyOnLoad(this);
            instance = this;
        }

        
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
