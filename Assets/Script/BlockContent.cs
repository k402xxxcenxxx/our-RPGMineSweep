using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public abstract class BlockType
{
    public enum Type
    {
        BLOCK,
        NUM,
        MONSTER,
        TERRAIN
    }
    public abstract Type getType();
}

public class BlockContent:BlockType
{
    

    private int x;
    private int y;
    static private Type type = Type.BLOCK;

    public BlockContent() { }

    public BlockContent(int myX, int myY) {
        x = myX;
        y = myY;
    }

    public int getX() { return x; }
    public int getY() { return y; }
    public override Type getType() { return Type.BLOCK; }

    public void setX(int newX) {  x = newX; }
    public void setY(int newY) { y = newY; }
    public string toString() {return "PosX :" + getX() + ",PosY : " + getY() + ",Type : " + getType(); }
}

public class NumBlock : BlockContent{
    static private Type type = Type.NUM;
    private float colorR;
    private float colorG;
    private float colorB;
    private int number;

    public NumBlock(int myX, int myY, float myR, float myG, float myB, int myNum) {
        setX(myX);
        setY(myY);
        colorR = myR;
        colorG = myG;
        colorB = myB;
        number = myNum;
    }

    public float getR() { return colorR; }
    public float getG() { return colorG; }
    public float getB() { return colorB; }
    public int getNumber() { return number; }
    public override Type getType() { return Type.NUM; }

    public void setR(float newR) { colorR = newR; }
    public void setG(float newG) { colorG = newG; }
    public void setB(float newB) { colorB = newB; }
    public void setNumber(int newNum) { number = newNum; }

    public string toString() { 
        return "Num Block , position ("+getX()+","+getY()+") , color ("+getR()+","+getG()+","+getB()+")";
    }
}

public class MonsterBlock : BlockContent {
    static private Type type = Type.MONSTER;
    /// <summary>
    /// 怪物格子上的怪物ID
    /// </summary>
    private int id;
    /// <summary>
    /// 怪物格子物件
    /// </summary>
    /// <param name="myX">所在的x座標</param>
    /// <param name="myY">所在的y座標</param>
    /// <param name="myID">怪物的ID</param>
    public MonsterBlock(int myX, int myY,int myID) { 
        setX(myX);
        setY(myY);
        id = myID;
    }
    
    public int getID() { return id; }
    public int getTextureID() { return Protocol.MonsterInfoList[id].getTextureID(); }
    public override Type getType() { return Type.MONSTER; }
    
    public void setID(int newID) { id = newID; }
}

public class TerrainBlock : BlockContent {
    static private Type type = Type.TERRAIN;

    private int textureID;
    public TerrainBlock(int myX, int myY, int myTextureID)
    {
        setX(myX);
        setY(myY);
        setTextureID(myTextureID);
    }
    public int getTextureID() { return textureID; }
    public override Type getType() { return Type.TERRAIN; }

    public void setTextureID(int newTextureID){textureID = newTextureID;}
    public string toString() { return "TerrainBlock , position (" + getX() + "," + getY() + ") , terrain (" + getTextureID() + ")"; }
}