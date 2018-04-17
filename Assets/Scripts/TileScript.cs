using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour {

    //maybe create a power tile as well?

    [SerializeField] private GameObject _BlankTileSprite;
    [SerializeField] private GameObject _OverTileSprite;
    [SerializeField] private GameObject _SelectedTileSprite;
    [SerializeField] private GameObject _SelectedUnitTileSprite;
    [SerializeField] private GameObject _PotentialMoveSprite;
    [SerializeField] private GameObject _PotentialMoveOverSprite;
    [SerializeField] private GameObject _PotentialMoveSelectedSprite;
    [SerializeField] private GameObject _PotentialAttackSprite;
    [SerializeField] private GameObject _BankTileSprite;
    [SerializeField] private GameObject _WaterTile;
    [SerializeField] private MeshRenderer _Renederer;

    enum VisualTileStates
    {
        BLANK,
        OVER,
        SELECTED,
        POTENTIALMOVE,
        POTENTIALATTACK,
    }

    public bool BankTile = false;
    public bool WaterTile = false;
    public bool SpawnTile = false;

    public UnitScript OccupiedBy;
    [HideInInspector]
    public BankScript OccupiedByBank;
    public int ListPosX, ListPosY;

    private VisualTileStates _CurrentVisualState;
    private bool _IgnoreThisClick = false;

    // Use this for initialization
    void Start() {

    }

    public void SetAsBankTile()
    {
        BankTile = true;
        _BlankTileSprite.SetActive(false);
        _BankTileSprite.SetActive(true);
    }

    public void SetAsWaterTile()
    {
        WaterTile = true;
        _Renederer.enabled = false;
        _WaterTile.SetActive(true);
    }

    public void SetAsSpawnTile()
    {
        SpawnTile = true;
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetMouseButtonDown(0) && !TileAssistant.IgnoreClicks)
        {
            if(TileAssistant.OverTile != this)
            {
                if(_CurrentVisualState == VisualTileStates.POTENTIALMOVE || _CurrentVisualState == VisualTileStates.POTENTIALATTACK)
                {
                    _CurrentVisualState = VisualTileStates.BLANK;
                }
                ChangeVisualState(VisualTileStates.BLANK);
                if (TileAssistant.SelectedUnit == OccupiedBy)
                {
                    TileAssistant.TurnManager.UnitDeselected();
                }
            }
        }
    }

    void OnMouseEnter()
    {
        if(TileAssistant.IsInSpawnState && OccupiedBy != null)
        {
            return;
        }
        TileAssistant.OverTile = this;
        if (TileAssistant.SelectedTile != this)
        {
            ChangeVisualState(VisualTileStates.OVER);
            if(TileAssistant.SelectedUnit == OccupiedBy)
            {
                TileAssistant.SelectedUnit = null;
            }
        }
    }

    void OnMouseOver()
    {
        if (TileAssistant.IsInSpawnState && OccupiedBy != null)
        {
            return;
        }
        if (TileAssistant.OverTile == this)
        {
            if(OccupiedBy != null && OccupiedBy.ComingIn)
            {
                return;
            }
            if (Input.GetMouseButtonDown(0) && !TileAssistant.IgnoreClicks && !TileAssistant.IsInSpawnState && !_IgnoreThisClick)
            {
                
                if (TileAssistant.SelectedTile != null)
                {
                    TileAssistant.PrevSelectedTile = TileAssistant.SelectedTile;
                }
                TileAssistant.SelectedTile = this;
                ChangeVisualState(VisualTileStates.SELECTED);
                
                
            }
        }
    }

    void OnMouseExit()
    {
        if (TileAssistant.IsInSpawnState && OccupiedBy != null)
        {
            return;
        }
        TileAssistant.OverTile = null;
        if (TileAssistant.SelectedTile != this)
        {
            ChangeVisualState(VisualTileStates.BLANK);
        }

    }

    private void ChangeVisualState(VisualTileStates NewState)
    {
        bool WillBeOccupied = false;
        if(_CurrentVisualState == VisualTileStates.POTENTIALMOVE || _CurrentVisualState == VisualTileStates.POTENTIALATTACK)
        {
            if (NewState != VisualTileStates.SELECTED)
            {
                _PotentialMoveOverSprite.SetActive(NewState != VisualTileStates.BLANK);
                return;
            }
            else
            {
                if (_CurrentVisualState == VisualTileStates.POTENTIALMOVE)
                {
                    TileAssistant.PrevSelectedTile.OccupiedBy.MoveUnitTo(this);
                    TileAssistant.PrevSelectedTile.OccupiedBy = null;
                    WillBeOccupied = true;
                }
                else if(_CurrentVisualState == VisualTileStates.POTENTIALATTACK)
                {
                    TileAssistant.TurnManager.AttackEachOther(TileAssistant.PrevSelectedTile.OccupiedBy, this.OccupiedBy);
                    NewState = VisualTileStates.BLANK;
                }

            }
        }
        TurnOffAllSprites();
        switch (NewState)
        {
            case VisualTileStates.BLANK:
                _BlankTileSprite.SetActive(true);
                break;
            case VisualTileStates.OVER:
                _OverTileSprite.SetActive(true);
                break;
            case VisualTileStates.SELECTED:
                if (OccupiedBy != null || WillBeOccupied)
                {
                    TileAssistant.SelectedUnit = OccupiedBy;
                    TileAssistant.TurnManager.UnitSelected(this);
                    _SelectedUnitTileSprite.SetActive(true);
                }
                else
                {
                    _SelectedTileSprite.SetActive(true);
                }
                break;
        }
        _CurrentVisualState = NewState;

    }

    private void TurnOffAllSprites()
    {
        _BlankTileSprite.SetActive(false);
        _OverTileSprite.SetActive(false);
        _SelectedTileSprite.SetActive(false);
        _SelectedUnitTileSprite.SetActive(false);
        _PotentialMoveSprite.SetActive(false);
        _PotentialMoveOverSprite.SetActive(false);
        _PotentialMoveSelectedSprite.SetActive(false);
        _PotentialAttackSprite.SetActive(false);
    }

    public void RefreshVisualState()
    {
        ChangeVisualState(_CurrentVisualState);
    }

    public void PotentialMoveOverride()
    {
        TurnOffAllSprites();
        _PotentialMoveSprite.SetActive(true);
        _CurrentVisualState = VisualTileStates.POTENTIALMOVE;
    }

    public void PotentialAttackOverride()
    {
        Debug.LogWarning("C");
        
        TurnOffAllSprites();
        _PotentialAttackSprite.SetActive(true);
        _CurrentVisualState = VisualTileStates.POTENTIALATTACK;
    }
    
}
