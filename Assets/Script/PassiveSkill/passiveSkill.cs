using UnityEngine;
using System.Collections;

public class passiveSkill : MonoBehaviour {

    public enum passiveSkillType {
        /// <summary>
        /// 對[屬性]減少傷害[數值]%
        /// </summary>
        DECREASE_DAMAGE,
        /// <summary>
        /// 對[屬性]增加戰鬥力[數值]
        /// </summary>
        INCREASE_DAMAGE,
        /// <summary>
        /// 擊敗[屬性]敵人增加戰鬥力[數值]
        /// </summary>
        INCREASE_DAMAGE_WIN,
        /// <summary>
        /// 點擊[數值]次恢復[數值]HP
        /// </summary>
        RECOVER_CONSTANT,
        /// <summary>
        /// 打敗[屬性]怪物恢復[數值]HP
        /// </summary>
        RECOVER_WIN,
        /// <summary>
        /// 每次戰鬥獲得[數值]充能，集滿[數值]充能，增加下一次戰鬥力[數值]
        /// </summary>
        CHARGE,
        /// <summary>
        /// [數值]%的機率增加戰鬥力[數值]%
        /// </summary>
        CRITICAL,
        /// <summary>
        /// 增加[數值]%掉落率
        /// </summary>
        DROP
    }

    private string m_sName;
    private int m_iID;
    private passiveSkillType m_tType;

    public string getName() { return m_sName; }
    public int getID() { return m_iID; }
    public passiveSkillType getType() { return m_tType; }

    public void setName(string newName) { m_sName = newName; }
    public void setID(int newID) { m_iID = newID; }
    public void setType(passiveSkillType newType) { m_tType = newType; }
    
}

