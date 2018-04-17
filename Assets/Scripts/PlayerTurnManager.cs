using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnManager : MonoBehaviour {

    [SerializeField] private CardManager _CardManager;
    [SerializeField] private Button _FinishTurnButton;
    [SerializeField] private GameObject _CardSlots;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClickFinishTurn()
    {
        TileAssistant.CurrentUserTurn = false;
        _FinishTurnButton.gameObject.SetActive(false);
        _CardSlots.SetActive(false);
        Invoke("StartTurn", 2.0f);//simulate other player
    }

    public void StartTurn()
    {
        TileAssistant.CurrencyManager.HandleStartOfTurnLogic();
        TileAssistant.CurrentUserTurn = true;
        _FinishTurnButton.gameObject.SetActive(true);
        _CardSlots.SetActive(true);
        _CardManager.AddCard();
        TileAssistant.TurnManager.HasPlacedBank = false;
        foreach(UnitScript US in TileAssistant.ListOfCreatedUnits)
        {
            US.MovedThisTurn = false;
            US.AttackedThisTurn = false;
        }
    }


}
