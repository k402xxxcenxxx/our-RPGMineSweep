using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class btnClone : MonoBehaviour {
    private int m_posX, m_posY;
    
    private bool isFlag = false;
    private static bool isLock = false;
    private MapManager myMapManager;
    public Text myText;
    public Image myBackgroundImage;
    public Image myImage;
    public Button myButton;
    private MapManager.BlockState myState;
    private BlockContent myButtonContent;

    public AudioClip m_aClickSound;
    public AudioSource m_aSource;

    public float m_recX, m_recY;
    void Awake() {
        myMapManager = MainGame.Instance.m_MapManager;
        m_aSource.clip = m_aClickSound;
    }

    void Start() {
    }

    public void playSound() { m_aSource.Play(); }

    public void OpenBlock() {
        GetComponent<Animation>().Play();
    }

    public void leftClickCloneBtn() {
        

        if(myButton.interactable == true) {
            OpenBlock();
            playSound();
            if (Protocol.m_gameMode == Protocol.gameMode.STORY_MODE) { 
                if(m_posX == Protocol.endPosX && m_posY == Protocol.endPosY)
                {
                    MapManager.Instance.setFinished(true);
                    return;
                }
            }

            m_recX = transform.position.x;
            m_recY = transform.position.y;

            if (myState == MapManager.BlockState.UNKNOW) { 
                if (!myMapManager.isMapContextExist()) {
                    myMapManager.buildRandomMapContent(m_posX,m_posY);
                }

                myButtonContent = myMapManager.pressCloneBtn(m_posX, m_posY,this);
                if(myButtonContent.getType() != BlockType.Type.MONSTER)
                    showContent();
                Debug.Log("Press " + myButtonContent.toString());
                if(myButtonContent.getType() != BlockType.Type.TERRAIN)
                    MapManager.Instance.activeCloneBtnAround(m_posX, m_posY);
            }
        }
    }
    public void rightClickCloneBtn() {
        switch (myState) {
            case MapManager.BlockState.UNKNOW:
                myText.text = "旗子";
                myMapManager.setCloneBtnState(m_posX, m_posY, MapManager.BlockState.FLAGS);
                break;
            case MapManager.BlockState.FLAGS:
                myText.text = "?";
                myMapManager.setCloneBtnState(m_posX, m_posY, MapManager.BlockState.NOTSURE);
                break;
            case MapManager.BlockState.NOTSURE:
                myText.text = "";
                myMapManager.setCloneBtnState(m_posX, m_posY, MapManager.BlockState.UNKNOW);
                break;
        }
    }
    public void clickCloneBtn() {
        myState = myMapManager.getCloneBtnState(m_posX, m_posY);

        if (Input.GetMouseButtonDown(1)) {

            if (Input.GetMouseButton(0)) {
                Debug.Log("PressBoth");
                if(myState == MapManager.BlockState.PRESS_CLEAR)
                    myMapManager.pressCloneBtnAround(m_posX,m_posY);

            }
            else
                rightClickCloneBtn();
        }

        if (Input.GetMouseButtonDown(0)) {
            
            leftClickCloneBtn();
        }
           
    }
    public void showContent() {
        
        myButtonContent = myMapManager.getCloneBtnContent(m_posX, m_posY);

        switch (myButtonContent.getType())
        {
            case BlockContent.Type.NUM:
                ComboBonusSystem.Instance.resetCombo();
                NumBlock myNumBlock = (NumBlock)myButtonContent;
                if (myNumBlock.getNumber() > 0)
                {
                    myText.text = "" + myNumBlock.getNumber();

                    myText.color = new Color(myNumBlock.getR(), myNumBlock.getG(), myNumBlock.getB());
                }
                break;
            case BlockContent.Type.MONSTER:
                
                MonsterBlock myMonsterBlock = (MonsterBlock)myButtonContent;
                myImage.sprite = Resources.Load("Textures/Character/monster_" + Protocol.Instance.getMonsterInfoByID(myMonsterBlock.getID()).getID(), typeof(Sprite)) as Sprite;

                switch (Protocol.Instance.getMonsterInfoByID(myMonsterBlock.getID()).getMonsterType())
                {
                    case Protocol.Type.FIRE:
                        myText.color = new Color(255, 0, 0);
                        break;
                    case Protocol.Type.GRASS:
                        myText.color = new Color(0, 255, 0);
                        break;
                    case Protocol.Type.WATER:
                        myText.color = new Color(0, 0, 255);
                        break;
                }
                break;
            case BlockContent.Type.TERRAIN:
                ComboBonusSystem.Instance.resetCombo();
                TerrainBlock myTerrainBlock = (TerrainBlock)myButtonContent;
                myImage.sprite = Resources.Load("Textures/UI/terrain_" + myTerrainBlock.getTextureID(), typeof(Sprite)) as Sprite;
                if (m_posX != Protocol.endPosX || m_posY != Protocol.endPosY)
                    MapManager.Instance.setCloneBtnState(m_posX, m_posY, MapManager.BlockState.PRESS_CLEAR);
                break;
        }
        Debug.Log("Press "+ myButtonContent.ToString());
        myButton.interactable = false;
    }

    public void selectBtn() { TouchControl.setBtnHold(this); }
    public int getX() { return m_posX; }
    public int getY() { return m_posY; }
    public void setX(int newX) { m_posX = newX; }
    public void setY(int newY) { m_posY = newY; }
    public static void lockBtn() { isLock = true; }
    public static void unlockBtn() { isLock = false; }
}
