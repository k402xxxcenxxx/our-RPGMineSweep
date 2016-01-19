using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatusManager : MonoBehaviour {

    PlayerContainer container = new PlayerContainer();
    int index;
    int id;
    /// <summary>
    /// If change confirmed, then save the status
    /// </summary>
    public void char_change_confirmed()
    {
        container.Player = container.Load();
        index = PlayerPrefs.GetInt("Key");
        id = GetComponent<CharacterInfomation>().getID();
        container.Player.Status[index] = id;
        Debug.Log(container.Player.Status.Count);
        container.Save();
    }
    public void equip_change_confirmed()
    {
        container.Player = container.Load();
        index = PlayerPrefs.GetInt("Key");
        id = GetComponent<Equipment>().getID();
        container.Player.Status[index] = id;
        Debug.Log(container.Player.Status.Count);
        container.Save();
    }
}
