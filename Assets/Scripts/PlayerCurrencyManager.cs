using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCurrencyManager : MonoBehaviour {

    [SerializeField] private Text _CashAmt;

    public enum CurrencyTypes
    {
        CASH
    }

    Dictionary<CurrencyTypes, int> _CurrencyAmts;

	// Use this for initialization
	void Start () {
        TileAssistant.CurrencyManager = this;
        HandleStartOfGameLogic();
    }

    public int GetAmountOfCurrency(CurrencyTypes CT)
    {
        int Amt = 0;
        _CurrencyAmts.TryGetValue(CurrencyTypes.CASH, out Amt);
        return Amt;
    }

    public void HandleStartOfTurnLogic()
    {
        //increase currencies as needed at start of turn
        int CashToAdd = 0;
        int CurrentAmt = 0;
        if (TileAssistant.ListOfCreatedBanks != null)
        {
            foreach (BankScript BS in TileAssistant.ListOfCreatedBanks)
            {
                if (BS.Information != null)
                {
                    CashToAdd += BS.Information.GoldPerTurn;
                }

            }
            
            _CurrencyAmts.TryGetValue(CurrencyTypes.CASH, out CurrentAmt);
            if (_CurrencyAmts.Remove(CurrencyTypes.CASH))
            {
                _CurrencyAmts.Add(CurrencyTypes.CASH, CurrentAmt + CashToAdd);
            }
        }
        _CashAmt.text = string.Format("Cash: {0}", (CurrentAmt + CashToAdd));
    }

    public void SpendCurrency(CurrencyTypes CT,int Amt)
    {
        int CurrentAmt;
        _CurrencyAmts.TryGetValue(CurrencyTypes.CASH, out CurrentAmt);
        if (_CurrencyAmts.Remove(CurrencyTypes.CASH))
        {
            _CurrencyAmts.Add(CurrencyTypes.CASH, CurrentAmt - Amt);
        }
        _CashAmt.text = string.Format("Cash: {0}", (CurrentAmt - Amt));
    }

    public void AddCurrency(CurrencyTypes CT, int Amt)
    {
        int CurrentAmt;
        _CurrencyAmts.TryGetValue(CurrencyTypes.CASH, out CurrentAmt);
        if (_CurrencyAmts.Remove(CurrencyTypes.CASH))
        {
            _CurrencyAmts.Add(CurrencyTypes.CASH, CurrentAmt + Amt);
        }
        _CashAmt.text = string.Format("Cash: {0}", (CurrentAmt + Amt));
    }

    public void HandleStartOfGameLogic()
    {
        _CurrencyAmts = new Dictionary<CurrencyTypes, int>();
        _CurrencyAmts.Add(CurrencyTypes.CASH, 0);

        HandleStartOfTurnLogic();
    }
}
