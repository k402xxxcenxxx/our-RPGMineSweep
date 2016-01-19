using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class MessageManager : MonoBehaviour {

    private static MessageManager m_Instance;
    public static MessageManager Instance
    {
        get
        {
            if (GameObject.Find("MessageManager"))
            {
                m_Instance = GameObject.Find("MessageManager").GetComponent<MessageManager>();
            }
            else 
            {
                m_Instance = new MessageManager();
            }
            return m_Instance;
        }
    }
    public GameObject InfoComfirmMessageObject;
    public GameObject InfoMessageObject;
    public GameObject GetItemMessageObject;
    public GameObject MaskObject;
    public GameObject containCanvas;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void showInfoComfirmMessage(string message) {
        GameObject newInfoComfirmMessage = (GameObject)GameObject.Instantiate(InfoComfirmMessageObject, new Vector2(0, 0), Quaternion.identity);
        newInfoComfirmMessage.GetComponent<InfoComfirmMessage>().Textmessage.text = message;

        newInfoComfirmMessage.transform.parent = containCanvas.transform;
        newInfoComfirmMessage.transform.localScale = Vector3.one;
    }

    public void showInfoComfirmMessage(string message,int width,int height)
    {
        GameObject newInfoComfirmMessage = (GameObject)GameObject.Instantiate(InfoComfirmMessageObject, new Vector2(0, 0), Quaternion.identity);
        newInfoComfirmMessage.GetComponent<InfoComfirmMessage>().Textmessage.text = message;
        newInfoComfirmMessage.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
        

        newInfoComfirmMessage.transform.parent = containCanvas.transform;
        newInfoComfirmMessage.transform.localScale = Vector3.one;
    }

    public void showGetItemMessage(string message,int item_imageId,float posX,float posY)
    {
        GameObject newGetItemMessage = (GameObject)GameObject.Instantiate(GetItemMessageObject, new Vector2(0, 0), Quaternion.identity);
        newGetItemMessage.GetComponent<getItemMessage>().Textmessage.text = message;
        newGetItemMessage.GetComponent<getItemMessage>().ImageItem.sprite = Resources.Load("Textures/Item/item_" + item_imageId,typeof(Sprite)) as Sprite;

        newGetItemMessage.GetComponent<RectTransform>().localPosition = new Vector2(posX,posY);
        newGetItemMessage.transform.parent = containCanvas.transform;
        newGetItemMessage.transform.localScale = Vector3.one;

        newGetItemMessage.GetComponent<getItemMessage>().show();
    }

    public void showInfoMessage(string message, float posX, float posY, Color fontColor, int newFontSize = 16)
    {
        GameObject newGetInfoMessage = (GameObject)GameObject.Instantiate(InfoMessageObject, new Vector2(0, 0), Quaternion.identity);
        newGetInfoMessage.GetComponent<InfoMessage>().Textmessage.text = message;
        newGetInfoMessage.GetComponent<InfoMessage>().Textmessage.fontSize = newFontSize;
        newGetInfoMessage.GetComponent<InfoMessage>().Textmessage.color = fontColor;
        newGetInfoMessage.GetComponent<RectTransform>().localPosition = new Vector2(posX, posY);
        newGetInfoMessage.transform.parent = containCanvas.transform;
        newGetInfoMessage.transform.localScale = Vector3.one;

        newGetInfoMessage.GetComponent<InfoMessage>().show();
    }

    public void showInfoMessage(string message, Color fontColor,int newFontSize = 16)
    {
        GameObject newGetInfoMessage = (GameObject)GameObject.Instantiate(InfoMessageObject, new Vector2(0, 0), Quaternion.identity);
        newGetInfoMessage.GetComponent<InfoMessage>().Textmessage.text = message;
        newGetInfoMessage.GetComponent<InfoMessage>().Textmessage.fontSize = newFontSize;
        newGetInfoMessage.GetComponent<InfoMessage>().Textmessage.color = fontColor;
        newGetInfoMessage.GetComponent<RectTransform>().localPosition = new Vector2(TouchControl.mybtnClone.m_recX, TouchControl.mybtnClone.m_recY);
        newGetInfoMessage.transform.parent = containCanvas.transform;
        newGetInfoMessage.transform.localScale = Vector3.one;

        newGetInfoMessage.GetComponent<InfoMessage>().show();
    }

    public void showMask(Color color, float seconds) {
        StartCoroutine(DelayShowMask(color, seconds));
    }

    public IEnumerator DelayShowMask(Color color,float seconds) {
        MaskObject.GetComponent<Image>().color = color;
        MaskObject.SetActive(true);
        yield return new WaitForSeconds(seconds);
        MaskObject.SetActive(false);
    }
}
