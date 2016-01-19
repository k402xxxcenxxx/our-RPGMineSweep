using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

public class PlayerInfo : MonoBehaviour {
    public string Name;
    public int id;
    public List<int> Characters = new List<int>();  //Owned Character
    public List<int> Equipments = new List<int>();  //Owned Equipment
    public List<int> Status = new List<int>();      //Character and equipment chosen
    public int diamonds;
    public int helmet;
    public int armor;
    public int shoes;
    public int pants;
}
