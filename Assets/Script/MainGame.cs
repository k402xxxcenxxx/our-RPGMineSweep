using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainGame : MonoBehaviour {
    private static MainGame m_Instance = new MainGame();
    public static MainGame Instance {
        get {
            if(m_Instance == null)
            {
                m_Instance = new MainGame();
            }
            return m_Instance;
        }
    }

    public StatePlayer m_StatePlayer;
    public MapManager m_MapManager;

    public static string stageName ="TestStage";

    public static Protocol.gameMode gameMode = Protocol.gameMode.STORY_MODE;

    GameObject resultPanel;

    enum State {
        NONE,
        PREPARE,
        START,
        PLAY,
        END
    }

    private State m_iState;

    void Awake()
    {
        m_Instance = this;
    }

    // Use this for initialization
    void Start () {
        m_iState = State.PREPARE;
        resultPanel = GameObject.Find("resultPanel");
        Init();
    }
	
	// Update is called once per frame
	void Update () {
        switch (m_iState) {
            case State.START:
                m_iState = State.PLAY;
                break;
            case State.PLAY:
                if (m_StatePlayer.m_sState == StatePlayer.State.DEAD)
                {
                    resultPanel.SetActive(true);
                    resultPanel.GetComponentInChildren<Text>().text = "你死惹";

                    m_iState = State.END;
                }
                else if (m_MapManager.isFinished()) {
                    resultPanel.SetActive(true);
                    resultPanel.GetComponentInChildren<Text>().text = "你贏惹";

                    m_iState = State.END;
                }
                break;
            case State.END:
                
                break;
            default:
                break;
        }
	}

    public void Init() {
        resultPanel.SetActive(false);
        m_StatePlayer.Init();
        MapManager.Instance.buildMapObject();
        gameMode = Protocol.m_gameMode;

        if (gameMode == Protocol.gameMode.STORY_MODE)
        {
            MapManager.Instance.buildKnowedMapContent();
            MapManager.Instance.buildRandomMapContent(Protocol.startPosX, Protocol.startPosY);
            for(int i = 0;i < Protocol.KnowedBlockList.Count; i++)
            {
                if(Protocol.KnowedBlockList[i].getType() == BlockType.Type.TERRAIN)
                    MapManager.Instance.getCloneBtnObject(Protocol.KnowedBlockList[i].getX(), Protocol.KnowedBlockList[i].getY()).GetComponent<btnClone>().showContent();
            }
            
            MapManager.Instance.activeCloneBtnAround(Protocol.startPosX, Protocol.startPosY);
        }else if(gameMode == Protocol.gameMode.SWEEP_MODE)
        {
            MapManager.Instance.buildKnowedMapContent();
            for (int i = 0; i < Protocol.KnowedBlockList.Count; i++)
            {
                if (Protocol.KnowedBlockList[i].getType() == BlockType.Type.TERRAIN)
                    MapManager.Instance.getCloneBtnObject(Protocol.KnowedBlockList[i].getX(), Protocol.KnowedBlockList[i].getY()).GetComponent<btnClone>().showContent();
            }
        }
        m_iState = State.START;
    }
    
}
