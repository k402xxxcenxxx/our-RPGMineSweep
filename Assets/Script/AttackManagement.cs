using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class AttackManagement : MonoBehaviour
{
    private static AttackManagement m_Instance = new AttackManagement();
    public static AttackManagement Instance
    {
        get
        {
            if (m_Instance == null)
            {
                if (GameObject.Find("AttackManagement"))
                    m_Instance = GameObject.Find("AttackManagement").GetComponent<AttackManagement>();
                else
                    m_Instance = new AttackManagement();
            }
            return m_Instance;
        }
    }
    public GameObject AttackPanel;
    public GameObject Player;
    public Animation PlayerAnimation;
    public GameObject Monster;
    public Animation MonsterAnimation;
    public Image PlayerImage;
    public Image MonsterImage;
    public Image MonsterMask;
    public float TimeScale = 1;

    int m_iCharacterId;
    int m_iMonsterId;
    bool m_bIsWin;

    public State m_state;
    public StateOK m_stateCheck;
   
    int fake_attack_count = 0;

    public enum State
    {
        NONE,
        INIT,
        READY,
        SHOW_PLAYER,
        SHOW_MONSTER,
        FAKE_ATTACK,
        SHOW_MONSTER_REAL,
        SHOW_RESULT,
        SHOW_DAMAGE,
        END
    }

    public enum StateOK
    {
        NONE,
        INIT_OK,
        READY_OK,
        SHOW_PLAYER_OK,
        SHOW_MONSTER_OK,
        FAKE_ATTACK_OK,
        SHOW_MONSTER_REAL_OK,
        SHOW_RESULT_OK,
        SHOW_DAMAGE_OK
    }

    // Use this for initialization
    void Start()
    {

    }

    public void PlayBattleAnimation(int CharacterId,int MonsterId,bool IsWin)
    {
        m_bIsWin = IsWin;
        m_iCharacterId = CharacterId;
        m_iMonsterId = MonsterId;

        m_state = State.INIT;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case State.INIT:
                Init();
                m_state = State.READY;
                break;
            case State.READY:
                if (m_stateCheck == StateOK.INIT_OK)
                {
                    SetAttackInfo();
                    m_state = State.SHOW_PLAYER;
                }
                break;
            case State.SHOW_PLAYER:
                if (m_stateCheck == StateOK.READY_OK)
                {
                    ShowPlayer();
                    m_state = State.SHOW_MONSTER;
                }
                break;
            case State.SHOW_MONSTER:
                if (m_stateCheck == StateOK.SHOW_PLAYER_OK)
                {
                    if (!PlayerAnimation.isPlaying)
                    {
                        ShowMonster();
                        m_state = State.FAKE_ATTACK;
                    }
                }

                break;
            case State.FAKE_ATTACK:
                if (m_stateCheck == StateOK.SHOW_MONSTER_OK)
                {
                    if (fake_attack_count == 0)
                    {
                        if (!MonsterAnimation.isPlaying)
                        {
                            FakeAttack();
                        }
                    }else if(fake_attack_count == 1)
                    {
                        if (!PlayerAnimation.isPlaying)
                        {
                            FakeAttack();
                            m_state = State.SHOW_MONSTER_REAL;
                        }
                    }
                }

                break;
            case State.SHOW_MONSTER_REAL:
                if (m_stateCheck == StateOK.FAKE_ATTACK_OK)
                {
                    if (!MonsterAnimation.isPlaying && !PlayerAnimation.isPlaying)
                    {
                        ShowMonsterReal();
                        m_state = State.SHOW_RESULT;
                    }
                }
                break;
            case State.SHOW_RESULT:
                if (m_stateCheck == StateOK.SHOW_MONSTER_REAL_OK)
                {
                    if (!PlayerAnimation.isPlaying && !MonsterAnimation.isPlaying)
                    {
                        ShowResult();
                        m_state = State.END;
                    }
                }
                break;
            case State.SHOW_DAMAGE:
                //ShowDamage();
                break;
            case State.END:
                if(m_stateCheck == StateOK.SHOW_RESULT_OK)
                {
                    if (!PlayerAnimation.isPlaying && !MonsterAnimation.isPlaying)
                    {
                        AttackPanel.SetActive(false);
                        BattleManager.Instance.ProcessResult(m_bIsWin, MainGame.Instance.m_StatePlayer, Protocol.Instance.getMonsterInfoByID(m_iMonsterId));
                        m_state = State.NONE;
                    }
                }
                
                break;
        }
    }

    private void Init()
    {
        Player.GetComponent<RectTransform>().localScale = Vector2.zero;
        Monster.GetComponent<RectTransform>().localScale = Vector2.zero;
        Player.GetComponent<RectTransform>().localPosition = new Vector2(-60, 0);
        Monster.GetComponent<RectTransform>().localPosition = new Vector2(60, 0);
        Player.GetComponent<RectTransform>().localRotation = new Quaternion(0, 0, 0, 0);
        Monster.GetComponent<RectTransform>().localRotation = new Quaternion(0, 0, 0, 0);
        MonsterMask.GetComponent<RectTransform>().sizeDelta = new Vector2(100,100);
        MonsterMask.GetComponent<RectTransform>().localPosition = new Vector2(0, 0);
        fake_attack_count = 0;

        m_stateCheck = StateOK.INIT_OK;
    }

    private void SetAttackInfo()
    {
        Sprite newSprite = Resources.Load("Textures/Character/icon_" + m_iCharacterId, typeof(Sprite)) as Sprite;
        if (newSprite != null)
            PlayerImage.sprite = newSprite;

        newSprite = Resources.Load("Textures/Character/monster_" + m_iMonsterId, typeof(Sprite)) as Sprite;
        if (newSprite != null)
            MonsterImage.sprite = newSprite;

        AttackPanel.SetActive(true);
        m_stateCheck = StateOK.READY_OK;
    }

    private void ShowPlayer()
    {
        PlayerAnimation.clip = PlayerAnimation.GetClip("ShowCard");
        PlayerAnimation.Play();

        m_stateCheck = StateOK.SHOW_PLAYER_OK;
    }

    private void ShowMonster()
    {
        MonsterAnimation.clip = MonsterAnimation.GetClip("ShowCard"); 
        MonsterAnimation.Play();

        m_stateCheck = StateOK.SHOW_MONSTER_OK;
    }

    private void FakeAttack()
    {
        PlayerAnimation.clip = PlayerAnimation.GetClip("FakeAttack");
        PlayerAnimation.Play();
        fake_attack_count++;
        SoundManagement.Instance.playFight();

        if(fake_attack_count == 2)
            m_stateCheck = StateOK.FAKE_ATTACK_OK;
    }

    private void ShowMonsterReal() {
        MonsterAnimation.clip = MonsterAnimation.GetClip("ShowMonster");
        MonsterAnimation.Play();

        m_stateCheck = StateOK.SHOW_MONSTER_REAL_OK;
    }

    private void ShowResult()
    {
        if (m_bIsWin)
        {
            MonsterAnimation.clip = MonsterAnimation.GetClip("MonsterLose");
            MonsterAnimation.Play();
            SoundManagement.Instance.playFight();
            PlayerAnimation.clip = PlayerAnimation.GetClip("AttackWin");
            PlayerAnimation.Play();
            SoundManagement.Instance.playWin();
            m_stateCheck = StateOK.SHOW_RESULT_OK;
            m_state = State.END;
        }
        else
        {
            MonsterAnimation.clip = MonsterAnimation.GetClip("MonsterWin");
            MonsterAnimation.Play();
            SoundManagement.Instance.playFight();
            PlayerAnimation.clip = PlayerAnimation.GetClip("AttackLose");
            PlayerAnimation.Play();
            SoundManagement.Instance.playLose();
            m_stateCheck = StateOK.SHOW_RESULT_OK;
            m_state = State.END;
        }

        
    }
}
