using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


/// <summary>
/// 表示地圖隨機怪物的編號及數量
/// </summary>
public class mapContent
{
    MonsterBlock content;
    int num;

    public mapContent(int ID, int myNum)
    {
        content = new MonsterBlock(0,0,ID);
        num = myNum;
    }

    public mapContent(MonsterBlock myMonsterBlock, int myNum)
    {
        content = myMonsterBlock;
        num = myNum;
    }

    public MonsterBlock getMonsterBlock()
   {
       return content;
    }

    public int getNum()
    {
        return num;
    }

    public string toString(){
        return "mapContent No." + content.getID() + " monster has " + num + " .";
    }
}

public class MapManager : MonoBehaviour {
    private static MapManager m_Instance = new MapManager();
    public static MapManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                if(GameObject.Find("MapManager"))
                    m_Instance = GameObject.Find("MapManager").GetComponent <MapManager>();
                else
                    m_Instance = new MapManager();
            }
            return m_Instance;
        }
    }
    /// <summary>
    ///每個格子物件的List   
    /// </summary>
    List<GameObject> m_gCloneBtnList = new List<GameObject>();

    /// <summary>
    ///內容地圖，存放格子內的資訊。正整數為ID、負整數為數字
    /// </summary>
    BlockContent[,] contentMap;

    /// <summary>
    ///狀態地圖，存放格子目前的狀態。
    /// </summary>
    BlockState[,] stateMap;

    /// <summary>
    ///   格子狀態的類別，
    /// </summary>
    public enum BlockState {
        /// <summary>
        ///   格子未知
        /// </summary>
        UNKNOW,
        /// <summary>
        ///  格子已按下
        /// </summary>
        PRESS_CLEAR,
        /// <summary>
        ///  格子放置旗子
        /// </summary>
        FLAGS,
        /// <summary>
        ///  格子放置不確定
        /// </summary>
        NOTSURE,
        PRESS_NOTCLEAR
    }


    /// <summary>
    ///   格子的物件
    /// </summary>
    public GameObject btnCell;

    private int width = Protocol.width;
    private int height = Protocol.height;
    private int monsterNum = 0;
    private bool m_bExist = false;
    private bool m_bFinish = false;

    /// <summary>
    ///  刪除地圖
    /// </summary>
    public void destroyMap() {
        for (int i = 0; i < m_gCloneBtnList.Count; i++)
            Destroy(m_gCloneBtnList[i]);

        m_gCloneBtnList.Clear();

        BlockState[,] stateMap = null;
        BlockContent[,] contentMap = null;
        monsterNum = 0;
        m_bExist = false;
    }

    /// <summary>
    /// 紀錄已知格子在map裡
    /// </summary>
    public void getBtnCellInformation(){

    }

    /// <summary>
    ///  建立地圖的格子物件
    /// </summary>
    public void buildMapObject()
    {
        float btnWidth, btnHeight;
        GameObject CloneBtnContainer = GameObject.Find("CloneBtnContainer");
        Canvas gameCanvas = GameObject.Find("GameCanvas").GetComponent<Canvas>();
        TouchControl.setBtnHold(null);
        width = Protocol.width;
        height = Protocol.height;
        btnWidth = (float)Screen.width / 32f;
        btnHeight = (float)Screen.height / 16f;

        

        CloneBtnContainer.GetComponent<RectTransform>().localPosition = Vector3.zero;
        CloneBtnContainer.GetComponent<RectTransform>().localScale = Vector3.one;
        gameCanvas.scaleFactor = 1;

        Debug.Log("width :" + width);
        Debug.Log("height :" + height);
        if (width > 0 && height > 0)
        {
            destroyMap();
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    GameObject newButton = (GameObject)GameObject.Instantiate(btnCell, new Vector2(0, 0), Quaternion.identity);
                    newButton.GetComponent<RectTransform>().sizeDelta = new Vector2(btnWidth, btnHeight);
                    newButton.GetComponent<RectTransform>().localPosition = new Vector3(((i - (width / 2f)) + 0.5f) * btnWidth + Screen.width / 2f, (j - height / 2f + 0.5f) * btnHeight + Screen.height / 2f, 0);

                    newButton.transform.parent = CloneBtnContainer.transform;
                     m_gCloneBtnList.Add(newButton);

                    newButton.GetComponent<btnClone>().setX(i);
                    newButton.GetComponent<btnClone>().setY(j);

                    if(Protocol.m_gameMode == Protocol.gameMode.STORY_MODE)
                    {
                        newButton.GetComponent<Button>().interactable = false;
                    }
                }
            }
        }

        stateMap = new BlockState[width, height];
        m_bFinish = false;
    }

    public void buildMapContentList(){
    }

    /// <summary>
    /// 建立空的contentMap
    /// </summary>
    /// <returns>建立好的contentMap</returns>
    public BlockContent[,] buildEmptyMapContent()
    {
        BlockContent[,] result = new BlockContent[width, height];

        //建立空格子
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                result[i, j] = new BlockContent(i,j);
            }
        }

        return result;
    }

    /// <summary>
    /// 建立地圖的已知內容
    /// </summary>
    public void buildKnowedMapContent() {
        contentMap = new BlockContent[width, height];
        contentMap = buildEmptyMapContent();

        //放置固定物
        for (int i = 0; i < Protocol.KnowedBlockList.Count; i++)
        {
            //如果是空格
            if (contentMap[Protocol.KnowedBlockList[i].getX(), Protocol.KnowedBlockList[i].getY()].getType() == BlockContent.Type.BLOCK)
            {
                //就換成要放的格子
                contentMap[Protocol.KnowedBlockList[i].getX(), Protocol.KnowedBlockList[i].getY()] = Protocol.KnowedBlockList[i];
                Debug.Log("KnowedBlock[" + i + "](" + Protocol.KnowedBlockList[i].getX() + "," + Protocol.KnowedBlockList[i].getY() + ") = " + Protocol.KnowedBlockList[i]);
            }
            
        }

    }

    /// <summary>
    /// 建立隨機格子的確定資訊
    /// </summary>
    /// <param name="exceptX">第一個按下的x座標</param>
    /// <param name="exceptY">第一個按下的y座標</param>
    public void buildRandomMapContent(int exceptX,int exceptY) {
        int random;

        List<BlockContent> ExceptBlockList = new List<BlockContent>();
        MonsterBlock monsterBlockTemp;
        MonsterInfo monsterInfoTemp;

        /// <summary>
        ///  可以隨機的清單
        /// </summary>
        List<BlockContent> unuseBlockList = new List<BlockContent>();

        //可隨機的格子的除外清單
        ExceptBlockList = getAroundBlockByType(exceptX, exceptY, BlockContent.Type.BLOCK);
        Debug.Log("ExceptList num = " + ExceptBlockList.Count);
        ExceptBlockList.Add(contentMap[exceptX, exceptY]);//包括自己

        //建立可隨機放置的格子清單
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //如果是空格
                if (contentMap[i, j].getType() == BlockContent.Type.BLOCK)
                {   
                    //加入可以隨機的選項
                    unuseBlockList.Add(contentMap[i, j]);
                }
            }
        }

        //把除外的格子去除
        for (int i = 0; i < ExceptBlockList.Count; i++) {
            unuseBlockList.Remove(ExceptBlockList[i]);
        }

        //放置隨機怪物
        for (int i = 0; i < Protocol.mapContentList.Count;i++ ){//i 是第幾種物件
            for (int j = 0; j < Protocol.mapContentList[i].getNum(); j++)//j 是物件的第幾個
            {
                //從可放置的格子中取出一個
                random = UnityEngine.Random.Range(0, unuseBlockList.Count-1);
                //放置在選到的格子
                contentMap[unuseBlockList[random].getX(), unuseBlockList[random].getY()] = Protocol.mapContentList[i].getMonsterBlock();

                contentMap[unuseBlockList[random].getX(), unuseBlockList[random].getY()].setX(unuseBlockList[random].getX());
                contentMap[unuseBlockList[random].getX(), unuseBlockList[random].getY()].setY(unuseBlockList[random].getY()); 

                Debug.Log("add monster (" + unuseBlockList[random].getX() + "," +unuseBlockList[random].getY() + ")");
                Debug.Log(Protocol.Instance.getMonsterInfoByID( Protocol.mapContentList[i].getMonsterBlock().getID() ).toString());
                //從格子清單中刪除
                unuseBlockList.RemoveAt(random);

                monsterNum++;
            }
        }

        Debug.Log("bomb Num = " + monsterNum);

        //把除外的格子加回去
        for (int i = 0; i < ExceptBlockList.Count; i++)
        {
            unuseBlockList.Add(ExceptBlockList[i]);
        }

        //生成數字
        //從還是空格的清單裡開始一個一個放置數字格子
        for (int i = 0; i < unuseBlockList.Count; i++)
        {
            if(contentMap[unuseBlockList[i].getX() ,unuseBlockList[i].getY()].getType() == BlockType.Type.BLOCK) { 
                int numberCount = 0;//要填的數字
                int rNum = 0;
                int gNum = 0;
                int bNum = 0;
                float R = 0, G = 0, B = 0;

                //取得四周是怪物的格子
                List<BlockContent> AroundList = getAroundBlockByType(unuseBlockList[i].getX(), unuseBlockList[i].getY(),BlockContent.Type.MONSTER);

                //Debug.Log("around list num = " + AroundList.Count);
            
                //從怪物的格子來取得數字跟顏色
                for (int j = 0; j < AroundList.Count; j++) {
                    //先改成怪物格子的型態
                    monsterBlockTemp = (MonsterBlock)AroundList[j];
                    //取得怪物資訊
                    monsterInfoTemp = Protocol.Instance.getMonsterInfoByID( monsterBlockTemp.getID());
                    //數字加上怪物的等級
                    numberCount += monsterInfoTemp.getLevel();
                    //依照怪物的屬性，增加其顏色的多寡
                    switch (monsterInfoTemp.getMonsterType())
                    {
                        case Protocol.Type.FIRE:
                            rNum += monsterInfoTemp.getLevel();
                            break;
                        case Protocol.Type.GRASS:
                            gNum += monsterInfoTemp.getLevel();
                            break;
                        case Protocol.Type.WATER:
                            bNum += monsterInfoTemp.getLevel();
                            break;
                    }
                }

                //如果有數字，則做顏色的相配
                if (numberCount > 0)
                {
                    R = (float)rNum / numberCount;
                    G = (float)gNum / numberCount;
                    B = (float)bNum / numberCount;
                }
            
                //給予新的格子資訊
                contentMap[unuseBlockList[i].getX(), unuseBlockList[i].getY()] = new NumBlock(unuseBlockList[i].getX(), unuseBlockList[i].getY(), R, G, B, numberCount);
                //Debug.Log("(" + unuseBlockList[i].getX() + "," + unuseBlockList[i].getY() + ") = " + numberCount);
            }
}
        m_bExist = true;
    }

    ///
    public void setWidth(string inputWidth) { width = int.Parse(inputWidth); }
    
    public void setHeight(string inputHeight) { height = int.Parse(inputHeight); }
    
    public void setBombNum(string inputBombNum) { monsterNum = int.Parse(inputBombNum); }

    public bool isMapContextExist() { return m_bExist; }

    public bool isFinished() { return m_bFinish; }

    public void setFinished(bool boolean) {  m_bFinish = boolean; }

    public GameObject getCloneBtnObject(int x, int y) {
        return m_gCloneBtnList[x*height+y];
    }

    public BlockState getCloneBtnState(int x, int y) { return stateMap[x, y]; }

    public BlockContent getCloneBtnContent(int x, int y) { return contentMap[x, y]; }

    public BlockContent.Type getCloneBtnType(int x, int y) { return contentMap[x, y].getType(); }

    public bool setCloneBtnState(int x, int y,BlockState state) {
        if (stateMap[x, y] != BlockState.PRESS_CLEAR)
        {
            stateMap[x, y] = state;
            return true;
        }
        else {
            return false;
        }

    }

    public BlockContent pressCloneBtn(int x,int y,btnClone mybtnClone) {
        BlockContent result = contentMap[x, y];
        
        
        stateMap[x, y] = BlockState.PRESS_CLEAR;

        switch(contentMap[x, y].getType())
        {
            case BlockType.Type.NUM:
                MessageManager.Instance.showGetItemMessage("+1", 1, mybtnClone.m_recX, mybtnClone.m_recY);

                NumBlock testNumBlock = (NumBlock)contentMap[x, y];

                //如果是零，則要四周也按
                if (testNumBlock.getNumber() == 0)
                {
                    StartCoroutine(delayAndPressAround(0.5f,x,y));
                    
                    return (NumBlock)result;
                }
                else
                {
                    return (NumBlock)result;
                }
                break;
            case BlockType.Type.TERRAIN:
                return (TerrainBlock)result;
                break;
            case BlockType.Type.MONSTER:
                MonsterBlock testMonsterBlock = (MonsterBlock)contentMap[x, y];

                BattleManager.Instance.battle(testMonsterBlock.getID());
                monsterNum--;

                //檢查是否結束
                m_bFinish = checkWin();

                return (MonsterBlock)result;
                break;
            default:
                break;
        }

        return result;
        //if (contentmap[x, y].gettype() != blockcontent.type.monster)//如果不是怪物
        //{
        //    messagemanager.instance.showgetitemmessage("+1", 1, mybtnclone.m_recx, mybtnclone.m_recy);

        //    numblock testnumblock = (numblock)contentmap[x, y];

        //    //如果是零，則要四周也按
        //    if (testnumblock.getnumber() == 0)
        //    {
        //        startcoroutine(delay());
        //        pressclonebtnaround(x, y);
        //        return contentmap[x, y];
        //    }
        //    else {
        //        return contentmap[x, y];
        //    }


        //}
        //else {
        //    monsterblock testmonsterblock = (monsterblock)contentmap[x, y];

        //    battlemanager.instance.battle(testmonsterblock.getid());
        //    monsternum--;

        //    //檢查是否結束
        //    m_bfinish = checkwin();

        //    return contentmap[x, y];
        //}

    }

    public void pressCloneBtnAround(int x, int y) {

        if (x - 1 >= 0 && y - 1 >= 0 && stateMap[x - 1, y - 1] == BlockState.UNKNOW)
        {
            this.getCloneBtnObject(x - 1, y - 1).GetComponent<btnClone>().leftClickCloneBtn(); //左上
        }

        if (y - 1 >= 0 && stateMap[x, y - 1] == BlockState.UNKNOW)
        {
            this.getCloneBtnObject(x, y - 1).GetComponent<btnClone>().leftClickCloneBtn();//上
        }

        if (x + 1 < width && y - 1 >= 0 && stateMap[x + 1, y - 1] == BlockState.UNKNOW)
        {
            this.getCloneBtnObject(x + 1, y - 1).GetComponent<btnClone>().leftClickCloneBtn();//右上
        }

        if (x - 1 >= 0 && stateMap[x - 1, y] == BlockState.UNKNOW)
        {
            this.getCloneBtnObject(x - 1, y).GetComponent<btnClone>().leftClickCloneBtn();//左
        }

        if (x + 1 < width && stateMap[x + 1, y] == BlockState.UNKNOW)
        {
            this.getCloneBtnObject(x + 1, y).GetComponent<btnClone>().leftClickCloneBtn();//右
        }

        if (x - 1 >= 0 && y + 1 < height && stateMap[x - 1, y + 1] == BlockState.UNKNOW)
        {
            this.getCloneBtnObject(x - 1, y + 1).GetComponent<btnClone>().leftClickCloneBtn();//左下
        }

        if (y + 1 < height && stateMap[x, y + 1] == BlockState.UNKNOW)
        {
            this.getCloneBtnObject(x, y + 1).GetComponent<btnClone>().leftClickCloneBtn();//下
        }

        if (x + 1 < width && y + 1 < height && stateMap[x + 1, y + 1] == BlockState.UNKNOW)
        {
            this.getCloneBtnObject(x + 1, y + 1).GetComponent<btnClone>().leftClickCloneBtn();//右下
        }
    }

    public void activeCloneBtnAround(int x, int y)
    {
        if (x - 1 >= 0 && y - 1 >= 0 && stateMap[x - 1, y - 1] != BlockState.PRESS_CLEAR)
        {
            this.getCloneBtnObject(x - 1, y - 1).GetComponent<Button>().interactable = true; //左上
        }

        if (y - 1 >= 0 && stateMap[x, y - 1] != BlockState.PRESS_CLEAR)
        {
            this.getCloneBtnObject(x, y - 1).GetComponent<Button>().interactable = true;//上
        }

        if (x + 1 < width && y - 1 >= 0 && stateMap[x + 1, y - 1] != BlockState.PRESS_CLEAR)
        {
            this.getCloneBtnObject(x + 1, y - 1).GetComponent<Button>().interactable = true;//右上
        }

        if (x - 1 >= 0 && stateMap[x - 1, y] != BlockState.PRESS_CLEAR)
        {
            this.getCloneBtnObject(x - 1, y).GetComponent<Button>().interactable = true;//左
        }

        if (x + 1 < width && stateMap[x + 1, y] != BlockState.PRESS_CLEAR)
        {
            this.getCloneBtnObject(x + 1, y).GetComponent<Button>().interactable = true;//右
        }

        if (x - 1 >= 0 && y + 1 < height && stateMap[x - 1, y + 1] != BlockState.PRESS_CLEAR)
        {
            this.getCloneBtnObject(x - 1, y + 1).GetComponent<Button>().interactable = true;//左下
        }

        if (y + 1 < height && stateMap[x, y + 1] != BlockState.PRESS_CLEAR)
        {
            this.getCloneBtnObject(x, y + 1).GetComponent<Button>().interactable = true;//下
        }

        if (x + 1 < width && y + 1 < height && stateMap[x + 1, y + 1] != BlockState.PRESS_CLEAR)
        {
            this.getCloneBtnObject(x + 1, y + 1).GetComponent<Button>().interactable = true;//右下
        }
    }

    public List<BlockContent> getAroundBlockByType(int x, int y, BlockContent.Type type) {
        List<BlockContent> result = new List<BlockContent>();

        if (x - 1 >= 0 && y - 1 >= 0 && contentMap[x - 1, y - 1].getType() == type)
        {
            result.Add(contentMap[x - 1, y - 1]); //左上
        }

        if (y - 1 >= 0 && contentMap[x, y - 1].getType() == type)
        {
            result.Add(contentMap[x, y - 1]);//上
        }

        if (x + 1 < width && y - 1 >= 0 && contentMap[x + 1, y - 1].getType() == type)
        {
            result.Add(contentMap[x + 1, y - 1]);//右上
        }

        if (x - 1 >= 0 && contentMap[x - 1, y].getType() == type)
        {
            result.Add(contentMap[x - 1, y]);//左
        }

        if (x + 1 < width && contentMap[x + 1, y].getType() == type)
        {
            result.Add(contentMap[x + 1, y]);//右
        }

        if (x - 1 >= 0 && y + 1 < height && contentMap[x - 1, y + 1].getType() == type)
        {
            result.Add(contentMap[x - 1, y + 1]);//左下
        }

        if (y + 1 < height && contentMap[x, y + 1].getType() == type)
        {
            result.Add(contentMap[x, y + 1]);//下
        }

        if (x + 1 < width && y + 1 < height && contentMap[x + 1, y + 1].getType() == type)
        {
            result.Add(contentMap[x + 1, y + 1]);//右下
        }

        return result;
    }

    public bool checkWin()
    {
        /*
        int unpressCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (stateMap[i, j] == BlockState.FLAGS || stateMap[i, j] == BlockState.UNKNOW)
                    unpressCount++;

                if (unpressCount > bombNum)
                    return false;
            }
        }
        return true;
        */

        switch (MainGame.gameMode)
        {
            case Protocol.gameMode.SWEEP_MODE:
                Debug.Log("monsterNum =" + monsterNum);
                return (monsterNum == 0);
                break;
            case Protocol.gameMode.STORY_MODE:
                return m_bFinish;
            default:
                return m_bFinish;
                break;
        }
        
    }

    IEnumerator delayAndPressAround(float seconds,int x,int y) {
        yield return wait(seconds);
        pressCloneBtnAround(x, y);
    }

    IEnumerator wait(float seconds)
    {
        Debug.Log("wait");
        yield return new WaitForSeconds(seconds);
    }
}


/*沒啥用的
            if (unuseBlockList[i].getX() - 1 >= 0 && unuseBlockList[unuseBlockList[i].getX()].getY() - 1 >= 0)
            {
                if (contentMap[unuseBlockList[i].getX() - 1, unuseBlockList[unuseBlockList[i].getX()].getY() - 1].getType() == BlockContent.Type.MONSTER)
                {
                    monsterBlockTemp = (MonsterBlock)contentMap[unuseBlockList[i].getX() - 1, unuseBlockList[unuseBlockList[i].getX()].getY() - 1];
                    numberCount +=Protocol.MonsterInfoList[monsterBlockTemp.getID()].getLevel();//左上

                    switch(monsterBlockTemp.getMonsterType()){
                        case MonsterBlock.MonsterType.FIRE:
                            rNum++;
                            break;
                        case MonsterBlock.MonsterType.GRASS:
                            gNum++;
                            break;
                        case MonsterBlock.MonsterType.WATER:
                            bNum++;
                            break;
                    }
                }
            }

            if (unuseBlockList[unuseBlockList[i].getX()].getY() - 1 >= 0)
            {
                if (contentMap[unuseBlockList[i].getX(), unuseBlockList[unuseBlockList[i].getX()].getY() - 1].getType() == BlockContent.Type.MONSTER)
                {
                    monsterBlockTemp = (MonsterBlock)contentMap[unuseBlockList[i].getX(), unuseBlockList[unuseBlockList[i].getX()].getY() - 1];
                    numberCount += Protocol.MonsterInfoList[monsterBlockTemp.getID()].getLevel();//上

                    switch (monsterBlockTemp.getMonsterType()){
                        case MonsterBlock.MonsterType.FIRE:
                            rNum++;
                            break;
                        case MonsterBlock.MonsterType.GRASS:
                            gNum++;
                            break;
                        case MonsterBlock.MonsterType.WATER:
                            bNum++;
                            break;
                    }
                }
            }

            if (unuseBlockList[i].getX() + 1 < width && unuseBlockList[unuseBlockList[i].getX()].getY() - 1 >= 0)
            {
                if (contentMap[unuseBlockList[i].getX() + 1, unuseBlockList[unuseBlockList[i].getX()].getY() - 1].getType() == BlockContent.Type.MONSTER)
                {
                    monsterBlockTemp = (MonsterBlock)contentMap[unuseBlockList[i].getX() + 1, unuseBlockList[unuseBlockList[i].getX()].getY() - 1];
                    numberCount += Protocol.MonsterInfoList[monsterBlockTemp.getID()].getLevel();

                    switch (monsterBlockTemp.getMonsterType()){
                        case MonsterBlock.MonsterType.FIRE:
                            rNum++;
                            break;
                        case MonsterBlock.MonsterType.GRASS:
                            gNum++;
                            break;
                        case MonsterBlock.MonsterType.WATER:
                            bNum++;
                            break;
                    }
                }//右上
            }

            if (unuseBlockList[i].getX() - 1 >= 0)
            {
                if (contentMap[unuseBlockList[i].getX() - 1, unuseBlockList[unuseBlockList[i].getX()].getY() ].getType() == BlockContent.Type.MONSTER)
                {
                    monsterBlockTemp = (MonsterBlock)contentMap[unuseBlockList[i].getX() - 1, unuseBlockList[unuseBlockList[i].getX()].getY()];
                    numberCount += Protocol.MonsterInfoList[monsterBlockTemp.getID()].getLevel();

                    switch (monsterBlockTemp.getMonsterType()){
                        case MonsterBlock.MonsterType.FIRE:
                            rNum++;
                            break;
                        case MonsterBlock.MonsterType.GRASS:
                            gNum++;
                            break;
                        case MonsterBlock.MonsterType.WATER:
                            bNum++;
                            break;
                    }
                }//左
            }

            if (unuseBlockList[i].getX() + 1 < width)
            {
                if (contentMap[unuseBlockList[i].getX() + 1, unuseBlockList[unuseBlockList[i].getX()].getY()].getType() == BlockContent.Type.MONSTER)
                {
                    monsterBlockTemp = (MonsterBlock)contentMap[unuseBlockList[i].getX() + 1, unuseBlockList[unuseBlockList[i].getX()].getY()];
                    numberCount += Protocol.MonsterInfoList[monsterBlockTemp.getID()].getLevel();

                    switch (monsterBlockTemp.getMonsterType()){
                        case MonsterBlock.MonsterType.FIRE:
                            rNum++;
                            break;
                        case MonsterBlock.MonsterType.GRASS:
                            gNum++;
                            break;
                        case MonsterBlock.MonsterType.WATER:
                            bNum++;
                            break;
                    }
                }//右
            }

            if (unuseBlockList[i].getX() - 1 >= 0 && unuseBlockList[unuseBlockList[i].getX()].getY() + 1 < height)
            {
                if (contentMap[unuseBlockList[i].getX() - 1, unuseBlockList[unuseBlockList[i].getX()].getY() + 1].getType() == BlockContent.Type.MONSTER)
                {
                    monsterBlockTemp = (MonsterBlock)contentMap[unuseBlockList[i].getX() - 1, unuseBlockList[unuseBlockList[i].getX()].getY() + 1];
                    numberCount += Protocol.MonsterInfoList[monsterBlockTemp.getID()].getLevel();

                    switch (monsterBlockTemp.getMonsterType()){
                        case MonsterBlock.MonsterType.FIRE:
                            rNum++;
                            break;
                        case MonsterBlock.MonsterType.GRASS:
                            gNum++;
                            break;
                        case MonsterBlock.MonsterType.WATER:
                            bNum++;
                            break;
                    }
                }//左下
            }

            if (unuseBlockList[unuseBlockList[i].getX()].getY() + 1 < height)
            {
                if (contentMap[unuseBlockList[i].getX(), unuseBlockList[unuseBlockList[i].getX()].getY() + 1].getType() == BlockContent.Type.MONSTER)
                {
                    monsterBlockTemp = (MonsterBlock)contentMap[unuseBlockList[i].getX(), unuseBlockList[unuseBlockList[i].getX()].getY() + 1];
                    numberCount += Protocol.MonsterInfoList[monsterBlockTemp.getID()].getLevel();

                    switch (monsterBlockTemp.getMonsterType()){
                        case MonsterBlock.MonsterType.FIRE:
                            rNum++;
                            break;
                        case MonsterBlock.MonsterType.GRASS:
                            gNum++;
                            break;
                        case MonsterBlock.MonsterType.WATER:
                            bNum++;
                            break;
                    }
                }//下
            }

            if (unuseBlockList[i].getX() + 1 < width && unuseBlockList[unuseBlockList[i].getX()].getY() + 1 < height)
            {
                if (contentMap[unuseBlockList[i].getX() + 1, unuseBlockList[unuseBlockList[i].getX()].getY() + 1].getType() == BlockContent.Type.MONSTER)
                {
                    monsterBlockTemp = (MonsterBlock)contentMap[unuseBlockList[i].getX() + 1, unuseBlockList[unuseBlockList[i].getX()].getY() + 1];
                    numberCount += Protocol.MonsterInfoList[monsterBlockTemp.getID()].getLevel();

                    switch (monsterBlockTemp.getMonsterType()){
                        case MonsterBlock.MonsterType.FIRE:
                            rNum++;
                            break;
                        case MonsterBlock.MonsterType.GRASS:
                            gNum++;
                            break;
                        case MonsterBlock.MonsterType.WATER:
                            bNum++;
                            break;
                    }
                }//右下
            }

            if (numberCount > 0) {
                rNum = 255 * rNum / (rNum + gNum + bNum);
                gNum = 255 * gNum / (rNum + gNum + bNum);
                bNum = 255 * bNum / (rNum + gNum + bNum);
            }
                   
            contentMap[unuseBlockList[i].getX(), unuseBlockList[unuseBlockList[i].getX()].getY()] = new NumBlock(unuseBlockList[i].getX(),unuseBlockList[unuseBlockList[i].getX()].getY(),rNum,gNum,bNum,numberCount);
            */
