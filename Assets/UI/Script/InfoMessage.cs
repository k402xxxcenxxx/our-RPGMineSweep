using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoMessage : MonoBehaviour {
    public GameObject InfoMessageObject;
    public Text Textmessage;
    bool isStart = false;
    int counter = 0;
    // Use this for initialization
    void Update()
    {
        if (isStart)
        {
            counter++;
            Textmessage.color = new Color(Textmessage.color.r, Textmessage.color.g, Textmessage.color.b, (180 - (float)counter) / 180);
            InfoMessageObject.transform.Translate(new Vector3(0, 0.2f, 0));
        }
    }

    public void show()
    {
        isStart = true;
        Destroy(InfoMessageObject, 3);
        isStart = true;
    }
}
