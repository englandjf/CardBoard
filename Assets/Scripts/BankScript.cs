using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BankScript : MonoBehaviour {

    [HideInInspector]
    public BankInformation Information;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool AttemptToPlace(string PrefabName)
    {
        if (TileAssistant.OverTile != null && TileAssistant.OverTile.BankTile && TileAssistant.OverTile.OccupiedByBank == null)
        {
            this.transform.position = TileAssistant.OverTile.transform.position + new Vector3(0, 0, 0);
            Information = DataManagerInstance.Instance.DataClass.GetBankInformationByPrefabName(PrefabName);
            TileAssistant.CurrencyManager.AddCurrency(PlayerCurrencyManager.CurrencyTypes.CASH, Information.GoldPerTurn);
            TileAssistant.OverTile.OccupiedByBank = this;
            TileAssistant.OverTile.RefreshVisualState();
            
            return true;
        }
        else
        {
            return false;
        }
    }
}
