using UnityEngine;
using System.Collections;

public class SoundManagement : MonoBehaviour {
    private static SoundManagement m_Instance = new SoundManagement();
    public static SoundManagement Instance
    {
        get
        {
            if (m_Instance == null)
            {
                if (GameObject.Find("SoundManagement"))
                    m_Instance = GameObject.Find("SoundManagement").GetComponent<SoundManagement>();
                else
                    m_Instance = new SoundManagement();
            }
            return m_Instance;
        }
    }
    AudioSource mySource;

    public AudioClip ClickBlock;
    public AudioClip ClickButton;
    public AudioClip Fight;
    public AudioClip Win;
    public AudioClip Lose;
    // Use this for initialization
    void Start () {
        mySource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void playClickButton() {
        mySource.PlayOneShot(ClickButton);
    } 

    public void playFight()
    {
        mySource.PlayOneShot(Fight);
    }

    public void playWin()
    {
        mySource.PlayOneShot(Win);
    }

    public void playLose()
    {
        mySource.PlayOneShot(Lose);
    }
}
