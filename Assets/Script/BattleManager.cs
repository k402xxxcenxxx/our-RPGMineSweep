using UnityEngine;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour {
    private static BattleManager m_Instance = new BattleManager();
    public static BattleManager Instance
    {
        get
        {
            return m_Instance;
        }
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void battle(int monsterId) {
        StatePlayer player = MainGame.Instance.m_StatePlayer;
        MonsterInfo monster = Protocol.Instance.getMonsterInfoByID(monsterId);
        float player_Atk = player.getLv();
        float monster_Atk = monster.getLevel();
        ComboBonusSystem.Instance.addCombo();
        //check type
        player_Atk *= Protocol.getTypeAffect(player.getType(), monster.getMonsterType());

        bool result = player_Atk >= monster_Atk;
        
        AttackManagement.Instance.PlayBattleAnimation(player.getId(), monster.getTextureID(), result);

        //StartCoroutine(WaitAndProcessResult(result, player, monster));
    }

    public void ProcessResult(bool result,StatePlayer player, MonsterInfo monster)
    {
        //無損戰鬥
        if (result)
        {
            player.addExp((int)(monster.getExp() * ComboBonusSystem.Instance.getComboBonus()));
            
        }
        else
        {
            List<passiveSkill> list = MainGame.Instance.m_StatePlayer.playerSkillList;
            int resultDamage = monster.getAttack();
            for(int i = 0;i < list.Count; i++)
            {
                if(list[i].getType() == passiveSkill.passiveSkillType.DECREASE_DAMAGE)
                {
                    resultDamage = (int)(resultDamage * ((passiveSkill_decreaseDamage)list[i]).getValue());
                }
            }

            MessageManager.Instance.showMask(Color.red, 0.1f);
            MessageManager.Instance.showInfoMessage("HP - " + monster.getAttack(), Color.red);
            player.hpDamage(monster.getAttack());
            player.addExp((int)(monster.getExp() * ComboBonusSystem.Instance.getComboBonus()));
        }

        TouchControl.mybtnClone.showContent();
    }
}
