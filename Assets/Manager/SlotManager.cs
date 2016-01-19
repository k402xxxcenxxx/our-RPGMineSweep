using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SlotManager : MonoBehaviour {

    public int id;
    PlayerContainer container = new PlayerContainer();
    PlayerInfo Player = new PlayerInfo();
    void OnEnable()
    {
        Player = container.Load();
        GetComponentInChildren<Text>().text = Player.Status[id].ToString();
    }
    public void setKey()
    {
        PlayerPrefs.SetInt("Key", id);
    }
}
