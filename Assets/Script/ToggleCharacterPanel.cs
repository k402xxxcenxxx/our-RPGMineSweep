using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ToggleCharacterPanel : MonoBehaviour {

    public List<GameObject> togglelist;
    public GameObject SmallPanel;
    public bool isOpen = true;
    public Animation m_aAnimation;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void togglePanel()
    {
        if (isOpen) { 
            for(int i = 0;i < togglelist.Count; i++)
            {
                togglelist[i].SetActive(false);
            }
            m_aAnimation.clip = m_aAnimation.GetClip("CloseCharacterPanel");
            m_aAnimation.Play();
            SmallPanel.SetActive(true);
            isOpen = false;
        }
        else
        {
            
            m_aAnimation.clip = m_aAnimation.GetClip("OpenCharacterPanel");
            m_aAnimation.Play();

            StartCoroutine(waitAndShowPanel());

            
            SmallPanel.SetActive(false);
            isOpen = true;
        }
    }

    IEnumerator waitAndShowPanel() {
        yield return m_aAnimation.isPlaying;
        for (int i = 0; i < togglelist.Count; i++)
        {
            togglelist[i].SetActive(true);
        }
    }
}
