using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class PlayerContainer : MonoBehaviour {
    public PlayerInfo Player;
    void Start()
    {
        //Player = Load();
    }
    public void Save()
    {
        PlayerPrefs.SetString("name", Player.Name);
        PlayerPrefs.SetInt("id", Player.id);
        PlayerPrefs.SetInt("diamonds", Player.diamonds);
        PlayerPrefs.SetInt("helmet", Player.helmet);
        PlayerPrefs.SetInt("armor", Player.armor);
        PlayerPrefs.SetInt("shoes", Player.shoes);
        PlayerPrefs.SetInt("pants", Player.pants);
        for (int i = 1; i <= Player.Characters.Count; i++)
        {
            PlayerPrefs.SetInt("c" + i.ToString(), Player.Characters[i-1]);
        }
        PlayerPrefs.SetInt("c_index", Player.Characters.Count);
        for (int i = 1; i <= Player.Equipments.Count; i++)
        {
            PlayerPrefs.SetInt("e" + i.ToString(), Player.Equipments[i-1]);
        }
        PlayerPrefs.SetInt("e_index", Player.Equipments.Count);
        for (int i = 1; i <= 6; i++) //Equipment status only has five block 
        {
            PlayerPrefs.SetInt("s" + i.ToString(), Player.Status[i - 1]);
        }
    }
    public PlayerInfo Load()
    {
        PlayerInfo loadedPlayer = new PlayerInfo();
        loadedPlayer.Name = PlayerPrefs.GetString("name");
        loadedPlayer.id = PlayerPrefs.GetInt("id");
        loadedPlayer.diamonds = PlayerPrefs.GetInt("diamonds");
        loadedPlayer.helmet = PlayerPrefs.GetInt("helmet");
        loadedPlayer.armor = PlayerPrefs.GetInt("armor");
        loadedPlayer.pants = PlayerPrefs.GetInt("pants");
        loadedPlayer.shoes = PlayerPrefs.GetInt("shoes");
        for (int i = 1; i <= PlayerPrefs.GetInt("c_index"); i++)
        {
            loadedPlayer.Characters.Add(PlayerPrefs.GetInt("c" + i.ToString()));
        }
        for (int i = 1; i <= PlayerPrefs.GetInt("e_index"); i++)
        {
            loadedPlayer.Equipments.Add(PlayerPrefs.GetInt("e" + i.ToString()));
        }
        for (int i = 1; i <= 6; i++)
        {
            loadedPlayer.Status.Add(PlayerPrefs.GetInt("s" + i.ToString()));
        }
        
        return loadedPlayer;
    }
}
