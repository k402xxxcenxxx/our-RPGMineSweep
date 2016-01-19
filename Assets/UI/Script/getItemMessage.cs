using UnityEngine;
using UnityEngine.UI;

public class getItemMessage : MonoBehaviour {
    public GameObject getItemMessageObject;
    public Text Textmessage;
    public Image ImageItem;
    bool isStart = false;
    int counter = 0;
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (isStart) { 
            counter++;
            Textmessage.color = new Color(Textmessage.color.r, Textmessage.color.g, Textmessage.color.b, (180 - (float)counter) / 180);
            ImageItem.color =  new Color(ImageItem.color.r, ImageItem.color.g, ImageItem.color.b, (180-(float)counter) / 180);
            getItemMessageObject.transform.Translate(new Vector3(0,0.2f,0));
        }
    }

    public void show() {
        isStart = true;
        Destroy(getItemMessageObject, 3);
        isStart = true;
    }
}
