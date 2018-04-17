using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataClass
{
    public List<UnitInformation> Units;
    public List<BankInformation> Banks;

    
    public UnitInformation GetUnitInformationByID(string ID)
    {
        foreach(UnitInformation UI in Units)
        {
            if (UI.ID == ID)
            {
                return UI;
            }
        }
        return null;
    }

    public UnitInformation GetUnitInformationByPrefabName(string Name)
    {
        foreach (UnitInformation UI in Units)
        {
            if (UI.PrefabName == Name)
            {
                return UI;
            }
        }
        return null;
    }

    public BankInformation GetBankInformationByPrefabName(string Name)
    {
        foreach (BankInformation BI in Banks)
        {
            if (BI.PrefabName == Name)
            {
                return BI;
            }
        }
        return null;
    }

    //Mostly for testin
    public UnitInformation GetRandomUnit()
    {
        int index = Random.Range(0, Units.Count);
        return Units[index];
    }

    public BankInformation GetRandomBank()
    {
        int index = 0;//Random.Range(0, Banks.Count);
        return Banks[index];
    }

}

[System.Serializable]
public class UnitInformation:BaseObjectInformation
{
    public int AttackValue;
    public int RangeValue;
    public int DefenseValue;
    public int MovementValue;
    public int SpawnCost;
    public string UnitType;
}

[System.Serializable]
public class BankInformation:BaseObjectInformation
{
    public int GoldPerTurn;
    public int UpgradeLimit;
    public int UpgradeMultiplier;
}

[System.Serializable]
public class BaseObjectInformation
{
    public string ID;
    public string Name;
    public string PrefabName;
    public string Description;
}
