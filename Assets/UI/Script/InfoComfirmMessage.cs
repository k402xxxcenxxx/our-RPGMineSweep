using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoComfirmMessage : MonoBehaviour {
    public GameObject InfoComfirmMessageObject;
    public Text Textmessage;
    public Text TextComfirmButton;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void removeThisMessage() {
        Destroy(InfoComfirmMessageObject);
    }
}
