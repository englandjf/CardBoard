using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour {

    public DataClass DataClass;
    [SerializeField] private CardManager _CardManager;

	// Use this for initialization
	void Start () {
        DataManagerInstance.Instance = this;
        StartCoroutine(LoadData());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator LoadData()
    {
        Debug.Log("Loading Data");
        TextAsset TA = Resources.Load("Data/Data") as TextAsset;
        yield return TA;
        Debug.Log("Finished Loading Data");
        this.DataClass = JsonUtility.FromJson<DataClass>(TA.text);
        yield return this.DataClass;
        _CardManager.DataLoadFinished();
    }
}

public static class DataManagerInstance{
    public static DataManager Instance;
}
