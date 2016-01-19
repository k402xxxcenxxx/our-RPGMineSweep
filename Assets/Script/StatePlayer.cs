using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class StatePlayer : MonoBehaviour {
    private static StatePlayer m_Instance = new StatePlayer();
    public static StatePlayer Instance
    {
        get
        {
            return m_Instance;
        }
    }
    public State m_sState = State.NONE;

    private int m_iIconId = 1;

    private int m_iHp = 0;
    private int maxHP = 0;
    private int m_iMp = 0;
    private int maxMP = 0;
    private int m_iExp = 0;
    private int maxExp = 0;
    private int m_iLv = 0;
    private Protocol.Type m_tType;

    public List<passiveSkill> playerSkillList;

    public Image CharacterIcon;
    public Text LabelHP;
    public RectTransform ImageHP;
    public Text LabelMP;
    public RectTransform ImageMP;
    public Text LabelLv;
    public Text LabelExp;
    public Image ImageType;
    public GameObject ComboSystem;
    public Text LabelCombo;
    public Text LabelComboBonus;

    public Text SmallPanelLv;
    public Text SmallPanelHP;
    public Text SmallPanelExp;

    public enum State {
        NONE,
        PREPARE,
        NORMAL,
        BATTLE,
        DEAD
    }

	// Use this for initialization
	void Start () {
        m_sState = State.PREPARE;
        Init();
    }
	
	// Update is called once per frame
	void Update () {
        switch (m_sState) {
            case State.NORMAL:
                updateCharacterPanel();
                break;
            case State.BATTLE:
                break;
            case State.DEAD:
                break;
        }
        
    }

    public void Init() {
        m_iLv = 1;
        maxExp = Protocol.ExpList[m_iLv];
        m_iExp = 0;
        maxHP = 10;
        m_iHp = 10;
        maxMP = 10;
        m_iMp = 0;
        m_tType = Protocol.Type.FIRE;

        updateCharacterPanel();
        m_sState = State.NORMAL;
    }

    public void updateCharacterPanel()
    {
        updateHP();
        updateMP();
        updateIcon();
        updateLv();
        updateExp();
        updateType();
        updateCombo();

    }
    public void addPassiveSkill(passiveSkill mySkill)
    {
        playerSkillList.Add(mySkill);
    }

    public void updateType() {
        Sprite newSprite = Resources.Load("Textures/UI/type_icon_" + (int)m_tType, typeof(Sprite)) as Sprite;
        if (newSprite != null)
            ImageType.sprite = newSprite;
    }

    public void updateIcon()
    {
        Sprite newSprite = Resources.Load("Textures/Character/icon_" + m_iIconId,typeof(Sprite)) as Sprite;
        if (newSprite != null)
            CharacterIcon.sprite = newSprite;
    }

    public void updateHP() {
        LabelHP.text = "" + m_iHp + "/" + maxHP;
        SmallPanelHP.text = "" + m_iHp;
        ImageHP.offsetMax = new Vector2(100 * (m_iHp - maxHP) / maxHP, ImageHP.offsetMax.y);
    }

    public bool hpDamage(int damage) {
        if (m_iHp <= damage)
        {
            m_iHp = 0;
            updateHP();
            m_sState = State.DEAD;
            return false;
        }
        else {
            m_iHp -= damage;
            updateHP();
            return true;
        }
    }

    public void updateMP()
    {
        LabelMP.text = "" + m_iMp + "/" + maxMP;
        ImageMP.offsetMax = new Vector2(100 * (m_iMp - maxMP) / maxMP, ImageMP.offsetMax.y);
    }

    public bool mpDamage(int damage)
    {
        if (m_iMp <= damage)
        {
            m_iMp = 0;
            updateMP();
            return false;
        }
        else
        {
            m_iMp -= damage;
            updateMP();
            return true;
        }
    }

    public void updateExp() {
        //表示升等
        if (m_iExp >= maxExp) {
            m_iLv += 1;
            m_iExp -= maxExp;

            MessageManager.Instance.showInfoMessage("Level Up!!",Color.yellow);
            MessageManager.Instance.showMask(Color.yellow,0.2f);

            //如果沒有滿等，則更改上限
            if (Protocol.ExpList.Count > m_iLv-1)
                maxExp = Protocol.ExpList[m_iLv-1];
            else
                maxExp = -2;
        }

        if (maxExp > 0)
            LabelExp.text = m_iExp + "/" + maxExp;
        else if (maxExp == -2)
        {
            LabelExp.text = "max";
            LabelExp.color = Color.red;
            m_iExp = 0;
        }
        else {
            LabelExp.text = "error";
        }

        SmallPanelExp.text = LabelExp.text;

        updateLv();
    }

    public void updateLv() {
        LabelLv.text = "" + m_iLv;
        SmallPanelLv.text = LabelLv.text;
    }

    public void addLv(int lv) {
        m_iLv += lv;
        updateLv();
    }

    public void addExp(int exp) {
        if(LabelExp.text != "max") {
            m_iExp += exp;
            updateExp();
        }
        
    }

    public int getLv() {
        return m_iLv;
    }

    public void updateCombo()
    {
        if (ComboBonusSystem.Instance.isComboContinue())
        {
            LabelCombo.text = ""+ComboBonusSystem.Instance.getComboCount();
            LabelComboBonus.text = ComboBonusSystem.Instance.getComboBonusToString();
            ComboSystem.SetActive(true);
        }
        else
        {
            ComboSystem.SetActive(false);
        }
    }

    public Protocol.Type getType() { return m_tType; }
    public int getId() { return m_iIconId; }
}
