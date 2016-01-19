using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MonsterInfo
{
    

    int id;
    int level;
    int textureID;
    private int attack;
    private Protocol.Type monsterType;
    private int exp;

    public MonsterInfo(int myID,int myLevel,int myTextureID, Protocol.Type myType,int myAttack,int myExp) {
        setID(myID);
        setLevel(myLevel);
        setTextureID(myTextureID);
        setMosterType(myType);
        setAttack(myAttack);
        setExp(myExp);
    }
    public MonsterInfo(int myID,int myTextureID)
    {
        setID(myID);
        setLevel(1);
        setTextureID(myTextureID);
        setMosterType(Protocol.Type.WATER);
        setAttack(1);
        setExp(1);
    }

    public int getID() { return id; }
    public int getLevel() { return level; }
    public int getTextureID() { return textureID; }
    public Protocol.Type getMonsterType() { return monsterType; }
    public int getAttack() { return attack; }
    public int getExp() { return exp; }

    public void setID(int newID) { id = newID; }
    public void setLevel(int newLevel) { level = newLevel; }
    public void setTextureID(int newTextureID) { textureID = newTextureID; }
    public void setMosterType(Protocol.Type newMosterType) { monsterType = newMosterType; }
    public void setAttack(int newAttack) { attack = newAttack; }
    public void setExp(int newExp) { exp = newExp; }

    public string toString() {
        return "Monster No." + getID() + " Lv." + getLevel() + " type:" + getMonsterType() +" Exp:"+getExp()+ " .";
    }
}