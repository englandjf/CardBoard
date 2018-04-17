using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitTurnManager : MonoBehaviour {

    [SerializeField] private GameObject _UnitMenu;
    [SerializeField] private Text _CurrentSelectedName;

    public bool HasPlacedBank = false;

    private TileScript _ActiveTile;
    private TileNeighbors _TileNeighbors;
    

    struct TileNeighbors
    {
        public TileScript UpTile;
        public TileScript RightTile;
        public TileScript DownTile;
        public TileScript LeftTile;
    }



    // Use this for initialization
    void Start() {
        TileAssistant.TurnManager = this;
    }

    // Update is called once per frame
    void Update() {

    }

    public void ActivateMove()
    {
        if (_ActiveTile.OccupiedBy.MovedThisTurn || !TileAssistant.CurrentUserTurn)
        {
            return;
        }
        //Show potential moves around current tile
        //for now just up,down,left,right
        PopulateNeighbors(true);
        if (_TileNeighbors.UpTile != null && _TileNeighbors.UpTile.OccupiedBy == null)
        {
            _TileNeighbors.UpTile.PotentialMoveOverride();
        }
        if (_TileNeighbors.RightTile != null && _TileNeighbors.RightTile.OccupiedBy == null)
        {
            _TileNeighbors.RightTile.PotentialMoveOverride();
        }
        if (_TileNeighbors.DownTile != null && _TileNeighbors.DownTile.OccupiedBy == null)
        {
            _TileNeighbors.DownTile.PotentialMoveOverride();
        }
        if (_TileNeighbors.LeftTile != null && _TileNeighbors.LeftTile.OccupiedBy == null)
        {
            _TileNeighbors.LeftTile.PotentialMoveOverride();
        }
    }
    public void ActivateAttack()
    {
        if (_ActiveTile.OccupiedBy.AttackedThisTurn || !TileAssistant.CurrentUserTurn)
        {
            return;
        }
        //Show potential moves around current tile
        //for now just up,down,left,right
        PopulateNeighbors(false);
        if (_TileNeighbors.UpTile != null && _TileNeighbors.UpTile.OccupiedBy != null)
        {
            _TileNeighbors.UpTile.PotentialAttackOverride();
        }
        if (_TileNeighbors.RightTile != null && _TileNeighbors.RightTile.OccupiedBy != null)
        { 
            _TileNeighbors.RightTile.PotentialAttackOverride();
        }
        if (_TileNeighbors.DownTile != null && _TileNeighbors.DownTile.OccupiedBy != null)
        {
            _TileNeighbors.DownTile.PotentialAttackOverride();
        }
        if (_TileNeighbors.LeftTile != null && _TileNeighbors.LeftTile.OccupiedBy != null)
        {
            _TileNeighbors.LeftTile.PotentialAttackOverride();
        }
    }

    public void AttackEachOther(UnitScript Attacker, UnitScript Defender)
    {
        Attacker.AttackedThisTurn = true;
        Debug.LogWarning(Attacker.name + " attacks " + Defender.name);
    }

    private void PopulateNeighbors(bool Move)
    {
        int Range = Move ? _ActiveTile.OccupiedBy.Information.MovementValue : _ActiveTile.OccupiedBy.Information.RangeValue;
        _TileNeighbors = new TileNeighbors();
        if (_ActiveTile.ListPosY + Range < TileAssistant.Board[0].Count)
        {
            TileScript UpTile = TileAssistant.Board[_ActiveTile.ListPosX][_ActiveTile.ListPosY + Range];
            if (UpTile != null && UnitSpecificCheck(UpTile))
            {
                _TileNeighbors.UpTile = UpTile;
            }
        }
        if (_ActiveTile.ListPosX + Range < TileAssistant.Board.Count)
        {
            TileScript RightTile = TileAssistant.Board[_ActiveTile.ListPosX + Range][_ActiveTile.ListPosY];
            if (RightTile != null && UnitSpecificCheck(RightTile))
            {
                _TileNeighbors.RightTile = RightTile;

            }
        }
        if (_ActiveTile.ListPosY - Range >= 0)
        {
            TileScript DownTile = TileAssistant.Board[_ActiveTile.ListPosX][_ActiveTile.ListPosY - Range];
            if (DownTile != null && UnitSpecificCheck(DownTile))
            {
                _TileNeighbors.DownTile = DownTile;

            }
        }
        if (_ActiveTile.ListPosX - Range >= 0)
        {
            TileScript LeftTile = TileAssistant.Board[_ActiveTile.ListPosX - Range][_ActiveTile.ListPosY];
            if (LeftTile != null && UnitSpecificCheck(LeftTile))
            {
                _TileNeighbors.LeftTile = LeftTile;

            }
        }
    }

    private bool UnitSpecificCheck(TileScript TS)
    {
        if(_ActiveTile.OccupiedBy != null && _ActiveTile.OccupiedBy.Information != null)
        {
            switch (_ActiveTile.OccupiedBy.Information.UnitType)
            {
                case "Land":
                    return !TS.WaterTile;
                case "Air":
                    return true;
                case "Sea":
                    return TS.WaterTile;
            }
        }
        return false;
}



    public void UnitSelected(TileScript TS)
    { 
        _UnitMenu.SetActive(true);
        _ActiveTile = TS;
        if (_ActiveTile.OccupiedBy != null)
        {
            _CurrentSelectedName.text =  _ActiveTile.OccupiedBy.Information.Name;
        }
    }

    public void UnitDeselected()
    {
        _UnitMenu.SetActive(false);
        _ActiveTile = null;
        
    }

    public void OverButton(bool Enter)
    {
        TileAssistant.IgnoreClicks = Enter;
    }

    
}
