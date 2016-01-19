using UnityEngine;
using System.Collections;

public class CharacterInfomation : MonoBehaviour {

    private int id;
    private string Name;
    private int textureID;
    private Protocol.Type Type;


    public CharacterInfomation(int myID, string myName, int myTextureID, Protocol.Type myType)
    {
        setID(myID);
        setName(myName);
        setTextureID(myTextureID);
        setType(myType);
    }

    public int getID() { return id; }
    public string getName() { return Name; }
    public int getTextureID() { return textureID; }
    public Protocol.Type getType() { return Type; } 

    public void setID(int newID) { id = newID; }
    public void setName(string newName) { Name = newName; }
    public void setTextureID(int newTextureID) { textureID = newTextureID; }
    public void setType(Protocol.Type newType) { Type = newType; }

    public string toString()
    {
        return "Character No." + getID() + " Name." + getName() + " type:" + getType() + " .";
    }
}
