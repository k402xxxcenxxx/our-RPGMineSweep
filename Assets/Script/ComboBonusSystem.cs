using UnityEngine;
using System.Collections;

public class ComboBonusSystem : MonoBehaviour {
    private static ComboBonusSystem m_Instance = new ComboBonusSystem();
    public static ComboBonusSystem Instance
    {
        get
        {
            return m_Instance;
        }
    }

    private int comboCount = 0;
    private float bonusRate = 0.25f;
    private int maxComboCount = 0;

    ComboBonusSystem() {
        Debug.Log("new combo system");
    }

    void Start()
    {
        reset();
    }
    public void addCombo() {
        comboCount++;
        if (maxComboCount < comboCount)
            maxComboCount++;

        Debug.Log("add combo, combo = "+ comboCount);
    }

    public void addCombo(int num)
    {
        comboCount+= num;
        if (maxComboCount < comboCount)
            maxComboCount++;
    }

    public void resetCombo() {
        Debug.Log("Reset Combo");
        comboCount = 0;
    }

    public void setBonusRate(float rate) {
        bonusRate = rate;
    }

    public float getComboBonus()
    {
        return (1 + ((float)(comboCount-1) * bonusRate));
    }

    public string getComboBonusToString()
    {
        return "+"+ ((1 + ((float)(comboCount - 1) * bonusRate)) * 100) + "%";
    }

    public int getComboCount()
    {
        return comboCount;
    }

    public bool isComboContinue()
    {
        if (comboCount == 0)
            return false;
        else
            return true;
    }

    public void reset() {
        comboCount = 0;
        maxComboCount = 0;
        bonusRate = 0.25f;
    }

    void Update() {
        if (comboCount > 0)
            Debug.Log("OCMBO!");
    }
}
