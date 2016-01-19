using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;


public class Protocol : MonoBehaviour {

    private static Protocol m_Instance = new Protocol();
    public static Protocol Instance
    {
        get
        {
            if (m_Instance == null)
            {
                if (GameObject.Find("Protocol"))
                    m_Instance = GameObject.Find("Protocol").GetComponent<Protocol>();
                else
                    m_Instance = new Protocol();
            }
            return m_Instance;
        }
    }

    string ip = "140.118.175.96";
    private XmlDocument m_xmlMonsterInfoList;
    private XmlDocument m_xmlCharacterInfoList;
    private XmlDocument m_xmlMapContentList;
    private XmlDocument m_xmlKnowedBlockList;
    private XmlDocument m_xmlExpList;
    private XmlDocument m_xmlSettingList;

    private int checkFinishDownloads = 0;
    public static protocolState state = protocolState.NONE;


    public static List<mapContent> mapContentList = new List<mapContent>();
    public static List<BlockContent> KnowedBlockList = new List<BlockContent>();
    public static List<MonsterInfo> MonsterInfoList = new List<MonsterInfo>();
    public static List<CharacterInfomation> CharacterInfoList = new List<CharacterInfomation>();
    public static List<int> ExpList = new List<int>();

    public static int width;
    public static int height;
    public static int startPosX;
    public static int startPosY;
    public static int endPosX;
    public static int endPosY;
    public static gameMode m_gameMode;

    public delegate void LoadInfoFunc();
    LoadInfoFunc myLoadInfoFunc;
    public delegate void LoadStageInfo(string stageId);

    public enum protocolState
    {
        NONE,
        INIT,
        PREPARE,
        READY
    }

    public enum gameMode
    {
        /// <summary>
        /// 故事模式，從起點走到終點
        /// </summary>
        STORY_MODE,
        /// <summary>
        /// 掃蕩模式，將所有怪物擊敗
        /// </summary>
        SWEEP_MODE
    }

    public enum Type
    {
        NONE,
        WATER,
        FIRE,
        GRASS,
        WATER_FIRE,
        WATER_GRASS,
        FIRE_GRASS,
        ALL
    }

    public static float getTypeAffect(Type user,Type target) {
        //被剋
        if ((user == Type.FIRE && target == Type.WATER) ||
           (user == Type.WATER && target == Type.GRASS) ||
           (user == Type.GRASS && target == Type.FIRE))
        {
            return 0.5f;
        }
        else if ((user == Type.WATER && target == Type.FIRE) ||
           (user == Type.GRASS && target == Type.WATER) ||
           (user == Type.FIRE && target == Type.GRASS))
        {
            return 2.0f;
        }
        else {
            return 1f;
        }
    }

    // Use this for initialization
    void Start()
    {
        Init();
    }

    public void Init() {
        state = protocolState.INIT;

        m_xmlMonsterInfoList = new XmlDocument();
        m_xmlCharacterInfoList = new XmlDocument();
        m_xmlMapContentList = new XmlDocument();
        m_xmlKnowedBlockList = new XmlDocument();
        m_xmlExpList = new XmlDocument();
        m_xmlSettingList = new XmlDocument();

        checkFinishDownloads = 0;

        Prepare();
    }

    void Prepare() {
        state = protocolState.PREPARE;
        StartCoroutine(downloadXml(m_xmlExpList, "ExpInfo.xml"));
        StartCoroutine(downloadXml(m_xmlMonsterInfoList , "MonsterInfoList.xml"));
        StartCoroutine(downloadXml(m_xmlCharacterInfoList , "CharacterInfoList.xml"));
        
        StartCoroutine(downloadXml(m_xmlSettingList , "GameSetting.xml"));
        StartCoroutine(downloadXml(m_xmlMapContentList , "MapContentList.xml"));
        StartCoroutine(downloadXml(m_xmlKnowedBlockList, "KnowedBlockList.xml"));
    }
    
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case protocolState.PREPARE:
                if (checkFinishDownloads == 6)
                {
                    loadMonsterInfo();
                    loadExpInfo();
                    loadCharacterInfo();
                    state = protocolState.READY;
                }
                break;
            case protocolState.READY:
                break;
            default:
                break;

        }
    }

    public void PrepareGameSetting(string stageId) {

        mapContentList.Clear();
        KnowedBlockList.Clear();

        loadGameSetting(stageId);
        loadMapContent(stageId);
        loadKnowedBlock(stageId);
    }

    /// <summary>
    /// 下載檔到指定的xmlDocument
    /// </summary>
    /// <param name="targetXmlDocument">目標xmlDocument</param>
    /// <param name="filePath">xml檔的位置</param>
    /// <param name="callbackFunc">下載完之後執行的callback</param>
    /// <returns></returns>
    IEnumerator downloadXml(XmlDocument targetXmlDocument, string filePath)
    {
        WWW Download = new WWW("http://" + ip + "/" + filePath);
        yield return Download;

        if (Download.error == null)
        {
            Debug.Log("finish download " + filePath);
            targetXmlDocument.LoadXml(Download.text);

            checkFinishDownloads++;
        }
        else
        {
            Debug.LogWarning("FAIL TO DOWNLOAD "+filePath+", "+Download.error);
            StartCoroutine(downloadXml(targetXmlDocument,filePath));
        }
    }
    
    //把下載完的xml讀取到遊戲的
    void loadGameSetting(string stageId)
    {

        //從GameSetting.xml取得遊戲設定資訊
        XmlNode itemNodeRoot = m_xmlSettingList.SelectSingleNode("/Setting/stage[@sceneName='" + stageId+"']");
        XmlNode itemNode = itemNodeRoot.SelectSingleNode("width");
        width = int.Parse(itemNode.InnerText);
        Debug.Log("width =: " + width);

        itemNode = itemNodeRoot.SelectSingleNode("height");
        height = int.Parse(itemNode.InnerText);

        Debug.Log("height =: " + height);

        itemNode = itemNodeRoot.SelectSingleNode("mode");
        m_gameMode = (gameMode)int.Parse(itemNode.InnerText);

        if (m_gameMode == gameMode.STORY_MODE)
        {
            itemNode = itemNodeRoot.SelectSingleNode("startPosX");
            startPosX = int.Parse(itemNode.InnerText);

            itemNode = itemNodeRoot.SelectSingleNode("startPosY");
            startPosY = int.Parse(itemNode.InnerText);

            itemNode = itemNodeRoot.SelectSingleNode("endPosX");
            endPosX = int.Parse(itemNode.InnerText);

            itemNode = itemNodeRoot.SelectSingleNode("endPosY");
            endPosY = int.Parse(itemNode.InnerText);
        }
        else
        {
            startPosX = 0;
            startPosY = 0;
            endPosX = 0;
            endPosY = 0;
        }
    }
    void loadExpInfo() {
        //從ExpInfo.xml取得經驗值資訊
        XmlNodeList itemNodes = m_xmlExpList.SelectNodes("/Exp/value");//取得所有item的node
        for (int i = 0; i < itemNodes.Count; i++)
        {
            ExpList.Add(int.Parse(itemNodes[i].InnerText));
            Debug.Log("ExpList[" + i + "].Add(" + int.Parse(itemNodes[i].InnerText) + ")");
        }
    }
    void loadMonsterInfo() {
        //從MonsterInfoList取得sceneName
        XmlNodeList itemNodes = m_xmlMonsterInfoList.SelectNodes("/MonsterInfo/Monster");//取得所有item的node
        for (int i = 0; i < itemNodes.Count; i++)
        {
            int id = int.Parse(itemNodes[i].SelectSingleNode("ID").InnerText);
            int level = int.Parse(itemNodes[i].SelectSingleNode("Level").InnerText);
            int textureID = int.Parse(itemNodes[i].SelectSingleNode("TextureID").InnerText);
            Protocol.Type monsterType = (Protocol.Type)int.Parse(itemNodes[i].SelectSingleNode("MonsterType").InnerText);
            int exp = int.Parse(itemNodes[i].SelectSingleNode("Exp").InnerText);
            int attack = int.Parse(itemNodes[i].SelectSingleNode("Attack").InnerText);
            MonsterInfoList.Add(new MonsterInfo(id, level, textureID, monsterType, attack, exp));
            Debug.Log(new MonsterInfo(id, level, textureID, monsterType, attack, exp).toString());
        }
    }
    void loadCharacterInfo()
    {
        //從MonsterInfoList取得sceneName
        XmlNodeList itemNodes = m_xmlMonsterInfoList.SelectNodes("/CharacterInfo/Character");//取得所有item的node
        for (int i = 0; i < itemNodes.Count; i++)
        {
            int id = int.Parse(itemNodes[i].SelectSingleNode("ID").InnerText);
            string Name = itemNodes[i].SelectSingleNode("Name").InnerText;
            int textureID = int.Parse(itemNodes[i].SelectSingleNode("TextureID").InnerText);
            Protocol.Type characterType = (Protocol.Type)int.Parse(itemNodes[i].SelectSingleNode("Type").InnerText);
            CharacterInfoList.Add(new CharacterInfomation(id, Name, textureID, characterType));
            Debug.Log(new CharacterInfomation(id, Name, textureID, characterType).toString());
        }
    }
	void loadMapContent(string stageId)
    {
        //從MonsterInfoList取得地圖資訊
        XmlNode itemNode = m_xmlMapContentList.SelectSingleNode("/MapContent/Map[@sceneName='" + stageId + "']");//取得所有item的node
        
        //從MonsterInfoList取得怪物資訊
        XmlNodeList itemNodes = itemNode.SelectNodes("Data");//取得所有item的node
        for (int i = 0; i < itemNodes.Count; i++)
        {
            int id = int.Parse(itemNodes[i].SelectSingleNode("ID").InnerText);
            int num = int.Parse(itemNodes[i].SelectSingleNode("Number").InnerText);

            mapContentList.Add(new mapContent(id, num));
            Debug.Log(new mapContent(id, num).toString());
        }
    }
    void loadKnowedBlock(string stageId)
    {
        //從MapContentList取得地圖資訊
        XmlNode itemNodeRoot = m_xmlKnowedBlockList.SelectSingleNode("/KnowedBlock/Map[@sceneName='" + stageId + "']");
        XmlNodeList itemNodes = itemNodeRoot.SelectNodes("Block");//取得所有item的node
        for (int i = 0; i < itemNodes.Count; i++)
        {
            int PosX = int.Parse(itemNodes[i].SelectSingleNode("PosX").InnerText);
            int PosY = int.Parse(itemNodes[i].SelectSingleNode("PosY").InnerText);
            Debug.Log("PosX :" + PosX);
            Debug.Log("PosY :" + PosY);

            BlockContent.Type BlockType = (BlockContent.Type)int.Parse(itemNodes[i].SelectSingleNode("BlockType").InnerText);
            Debug.Log("BlockType :" + BlockType);
            switch (BlockType)
            {
                case BlockContent.Type.TERRAIN:
                    int TextureID = int.Parse(itemNodes[i].SelectSingleNode("TextureID").InnerText);
                    TerrainBlock terrainBlock = new TerrainBlock(PosX, PosY, TextureID);
                    KnowedBlockList.Add(terrainBlock);

                    break;
                case BlockContent.Type.MONSTER:
                    int MonsterID = int.Parse(itemNodes[i].SelectSingleNode("MonsterID").InnerText);
                    MonsterBlock monsterBlock = new MonsterBlock(PosX, PosY, MonsterID);
                    KnowedBlockList.Add(monsterBlock);
                    break;
            }
        }
    }

   

    /// <summary>
    /// 利用MonsterID取得該monsterInfo
    /// </summary>
    /// <param name="id">MonsterID</param>
    /// <returns>該ID的MonsterInfo</returns>
    public MonsterInfo getMonsterInfoByID(int id){
        return MonsterInfoList.Find(x => x.getID() == id);
    }
}
