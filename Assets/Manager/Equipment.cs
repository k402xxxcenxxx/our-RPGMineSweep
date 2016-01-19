using UnityEngine;
using System.Collections;

public class Equipment : MonoBehaviour {

    private int id;
    private string Name;
    private int textureID;
    private string effect;


    public Equipment(int myID, string myName, int myTextureID, string myEffect)
    {
        setID(myID);
        setName(myName);
        setTextureID(myTextureID);
        setEffect(myEffect);
    }

    public int getID() { return id; }
    public string getName() { return Name; }
    public int getTextureID() { return textureID; }
    public string getEffect() { return effect;}

    public void setID(int newID) { id = newID; }
    public void setName(string newName) { Name = newName; }
    public void setTextureID(int newTextureID) { textureID = newTextureID; }
    public void setEffect(string newEffect) { effect = newEffect; }

    public string toString()
    {
        return "Equipment No." + getID() + " Name." + getName() + " type:" + getEffect() + " .";
    }
}
