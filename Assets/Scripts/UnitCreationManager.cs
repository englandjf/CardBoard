using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCreationManager : MonoBehaviour {

    //[SerializeField] private UnitScript _TestUnit;

    private GameObject _CreatedObject;
    private CardVisualScript.CardTypes _CurrentCreatedType;
    private string _CurrentPrefabName;

    private Vector3 _MouseWorldPos;

    private Dictionary<string, GameObject> _LoadedUnits;

    private System.Action _PlacedCallback = null;

	// Use this for initialization
	void Start () {
        _LoadedUnits = new Dictionary<string, GameObject>();
        TileAssistant.ListOfCreatedUnits = new List<UnitScript>();
        TileAssistant.ListOfCreatedBanks = new List<BankScript>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 ScreenPT = Input.mousePosition;
        ScreenPT.z = 15;
        _MouseWorldPos = Camera.main.ScreenToWorldPoint(ScreenPT);
        if(_CreatedObject != null)
        {
            _CreatedObject.transform.position = _MouseWorldPos;
            if(Input.GetMouseButtonDown(0) && !TileAssistant.IgnoreClicks)
            {
                AttemptToPlace();
            }
        }
	}

    private void AttemptToPlace()
    {
        bool Success = false;
        switch (_CurrentCreatedType)
        {
            case CardVisualScript.CardTypes.UNIT:
                Success = _CreatedObject.GetComponent<UnitScript>().AttemptToPlace(_CurrentPrefabName);
                if(Success)
                {
                    TileAssistant.ListOfCreatedUnits.Add(_CreatedObject.GetComponent<UnitScript>());
                }
                break;
            case CardVisualScript.CardTypes.BANK:
                Success = _CreatedObject.GetComponent<BankScript>().AttemptToPlace(_CurrentPrefabName);
                if(Success)
                {
                    TileAssistant.TurnManager.HasPlacedBank = true;
                    TileAssistant.ListOfCreatedBanks.Add(_CreatedObject.GetComponent<BankScript>());
                }
                break;
        }        
        if (Success)
        {
            _CreatedObject = null;
            TileAssistant.IsInSpawnState = false;
            if(_PlacedCallback != null)
            {
                _PlacedCallback.Invoke();
                _PlacedCallback = null;
            }
        }
    }
    
    /*
    public void SpawnTestUnit()
    {
        _CreatedUnit = (UnitScript)Instantiate(_TestUnit, _MouseWorldPos, _TestUnit.transform.rotation);
        TileAssistant.IsInSpawnState = true;
    }
    */

    public void SpawnUnit(string ID,CardVisualScript.CardTypes CardType,System.Action PlacedCallback,System.Action<GameObject> CreatedCallback)
    {
        //load ID from resource or grab from local list or something?
        //maybe load from resource once and store in list
        _CurrentCreatedType = CardType;
        _PlacedCallback = PlacedCallback;
        GameObject DataObject = null;
        string path = "";
        _CurrentPrefabName = ID;
        switch(CardType)
        {
            case CardVisualScript.CardTypes.UNIT:
                path = "Units/";
                break;
            case CardVisualScript.CardTypes.BANK:
                path = "Statics/";
                break;
        }
        if(_LoadedUnits.TryGetValue(ID, out DataObject))
        {
            Debug.LogWarning(ID + "Loaded from loaded list");
            _CreatedObject = ((GameObject)Instantiate(DataObject, _MouseWorldPos, DataObject.transform.rotation));
            TileAssistant.IsInSpawnState = true;
            CreatedCallback.Invoke(_CreatedObject);
        }
        else
        {
            DataObject = Resources.Load(path + ID) as GameObject;
            if(DataObject != null)
            {
                _CreatedObject = ((GameObject)Instantiate(DataObject, _MouseWorldPos, DataObject.transform.rotation));
                TileAssistant.IsInSpawnState = true;
                _LoadedUnits.Add(ID, DataObject);
                CreatedCallback.Invoke(_CreatedObject);
            }
            else
            {
                Debug.LogWarning("Object not found for " + ID);
            }
        }
        
    }
}
