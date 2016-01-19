using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
public class ChooseCharacter : MonoBehaviour {

    PlayerContainer container = new PlayerContainer();
    PlayerInfo Player = new PlayerInfo();
    public GameObject CharacterBtn;
    public GameObject Anchor;
    GameObject CharacterBtnPrefab;
    List<GameObject> PrefabList = new List<GameObject>();
	// Execute On Enable
	void OnEnable () {
        Player = container.Load();
        for (int i = 0; i < Player.Characters.Count; i++)
        {
            CharacterBtnPrefab = Instantiate(CharacterBtn);
            CharacterBtnPrefab.transform.SetParent(Anchor.transform, false);
            CharacterBtnPrefab.GetComponent<RectTransform>().transform.localPosition = new Vector3(CharacterBtnPrefab.GetComponent<RectTransform>().sizeDelta.x * (i % 6), CharacterBtnPrefab.GetComponent<RectTransform>().sizeDelta.y * (-i / 6), 0);
            CharacterBtnPrefab.GetComponent<CharacterInfomation>().setID(Player.Characters[i]);
            CharacterBtnPrefab.GetComponentInChildren<Text>().text = Player.Characters[i].ToString();
            PrefabList.Add(CharacterBtnPrefab);
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
