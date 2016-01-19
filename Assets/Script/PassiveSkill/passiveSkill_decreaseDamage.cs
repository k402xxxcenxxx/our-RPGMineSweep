using UnityEngine;
using System.Collections;

public class passiveSkill_decreaseDamage : passiveSkill {
    
    public passiveSkill_decreaseDamage(int ID,string name,float value,Protocol.Type targetType){
        setID(ID);
        setName(name);
        setType(passiveSkillType.DECREASE_DAMAGE);
        m_fValue = value;
        m_tTargetType = targetType;
    }

    private float m_fValue;
    private Protocol.Type m_tTargetType;

    public float getValue() { return m_fValue; }
    public Protocol.Type getTargetType() { return m_tTargetType; }
   
}
