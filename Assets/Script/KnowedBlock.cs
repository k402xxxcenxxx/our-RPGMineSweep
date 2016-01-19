using UnityEngine;
using System.Collections;


/// <summary>
/// 已知格子的資訊物件
/// </summary>
public class KnowedBlock : MonoBehaviour{
    int x;
    int y;
    BlockContent content;
    public KnowedBlock(int myX, int myY, BlockContent myContent)
    {
        x = myX;
        y = myY;
        content = myContent;
    }
    public int getX() { return x; }
    public int getY() { return y; }
    public BlockContent getContent() { return content; }
}
