using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileAssistant
{
    public static TileScript OverTile;
    public static TileScript SelectedTile;
    public static TileScript PrevSelectedTile;
    public static List<List<TileScript>> Board;
    public static bool IsInSpawnState = false;
    public static UnitScript SelectedUnit = null;
    public static UnitTurnManager TurnManager;
    public static bool IgnoreClicks = false;
    public static ToolTipText ToolTip;
    public static bool CurrentUserTurn = true;
    public static List<UnitScript> ListOfCreatedUnits;
    public static List<BankScript> ListOfCreatedBanks;
    public static PlayerCurrencyManager CurrencyManager;

}
