using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GatchaManager : MonoBehaviour {
	/// <summary>
	/// Statement
	/// </summary>
    public Text gatcha_result;
	public GameObject FIVESTAR;
	public GameObject FOURSTAR;
	public GameObject THREESTAR;
	List<GameObject> result_list;
	GameObject result;
    void Start () {
		result_list = new List<GameObject> ();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnGUI()
    {
        
    }
	/// <summary>
	/// Gatcha
	/// </summary>
    public void gatcha_time()
	{
		int i = (int)(Random.value * 1000);
		if (i % 100 == 0) {
			result = Instantiate(FIVESTAR) as GameObject;
		} else if (i % 5 == 0) {
			result = Instantiate(FOURSTAR) as GameObject;
		} else if (i % 1 == 0) {
			result = Instantiate(THREESTAR) as GameObject;
		}
		result_list.Add (result);
		result.transform.SetParent(gatcha_result.transform, false);
		result.GetComponent<RectTransform>().transform.localPosition = new Vector3 (result_list [result_list.Count - 1].GetComponent<RectTransform> ().sizeDelta.x * result_list.Count,0,0);
    }
	/// <summary>
	/// 10 Gatchas.
	/// </summary>
	public void gatcha_ten()
	{
		for (int i = 0; i < 10; i++) {
			gatcha_time ();
		}
	}
	/// <summary>
	/// Clears the result.
	/// </summary>
	public void clear_result(){
		for (int i = 0; i < result_list.Count; i++) {
			Destroy (result_list[i]);
		}
		result_list.Clear ();
		gatcha_result.text = "";
	}
}
