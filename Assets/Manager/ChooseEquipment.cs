using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
public class ChooseEquipment : MonoBehaviour
{

    PlayerContainer container = new PlayerContainer();
    PlayerInfo Player = new PlayerInfo();
    public GameObject EquipmentBtn;
    public GameObject Anchor;
    GameObject EquipmentBtnPrefab;
    List<GameObject> PrefabList = new List<GameObject>();
    // Execute on enable
    void OnEnable()
    {
        Player = container.Load();
        for (int i = 0; i < Player.Equipments.Count; i++)
        {
            EquipmentBtnPrefab = Instantiate(EquipmentBtn);
            EquipmentBtnPrefab.transform.SetParent(Anchor.transform, false);
            EquipmentBtnPrefab.GetComponent<RectTransform>().transform.localPosition = new Vector3(EquipmentBtnPrefab.GetComponent<RectTransform>().sizeDelta.x * (i % 6), EquipmentBtnPrefab.GetComponent<RectTransform>().sizeDelta.y * (-i / 6), 0);
            EquipmentBtnPrefab.GetComponent<Equipment>().setID(Player.Equipments[i]);
            EquipmentBtnPrefab.GetComponentInChildren<Text>().text = Player.Equipments[i].ToString();
            PrefabList.Add(EquipmentBtnPrefab);
        }
    }
    // Destroy On Exit
    void OnDisable()
    {
        for (int i = 0; i < PrefabList.Count; i++)
        {
            Destroy(PrefabList[i]);
        }
    }
}
