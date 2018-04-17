using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitScript : MonoBehaviour
{

    //only planes, ships, ground vehicles
    //planes fly up higher and then swoop down to attack or drop bomb
    //ships turn sideways
    //ground vehicles most of the time have barrels or turrets that turn or something
    //planes fly to spot from off screen
    //ships are dropped into the water
    //ground vehicles are dropped in boxes
    //only certain boats and land units can hit air
    //not all air can hit ground units(bombers)

        //static unit idea, command post, allows for the creation of units at four points around it

    //[SerializeField]
    //private Animator _A;
    /*
    public enum UnitStates
    {
        IDLE,
        SELECTED,
        MOVING
    }
    */ //not sure if needed

   // private UnitStates _CurrentState;
    private TileScript _ParentTile;
    [HideInInspector]
    public UnitInformation Information;
    

    //Will later pull from data file, but for now just use random stuff

    public int MoveSize = 1;
    public bool ComingIn = false;

    [HideInInspector]
    public bool MovedThisTurn = false;
    [HideInInspector]
    public bool AttackedThisTurn = false;

    private Vector3 PosOffset;

    /*
    public void SetState(UnitStates NewState)
    {
        _CurrentState = NewState;
    }
    */

    public void MoveUnitTo(TileScript TS)
    {
        StartCoroutine(MoveToNewTile(TS));
    }

    IEnumerator MoveToNewTile(TileScript TS)
    {
        while(this.transform.position != TS.transform.position + PosOffset)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, TS.transform.position+PosOffset, .1f);
            yield return null;
        }
        Debug.LogWarning("Completed move");
        TS.OccupiedBy = this;
        TileAssistant.TurnManager.UnitSelected(TS);
        MovedThisTurn = true;
    }


    public bool AttemptToPlace(string PName)
    {
        if (TileAssistant.OverTile != null && TileAssistant.OverTile.OccupiedBy == null && !TileAssistant.OverTile.BankTile && TileAssistant.OverTile.SpawnTile)
        {
            Information = DataManagerInstance.Instance.DataClass.GetUnitInformationByPrefabName(PName);//will be wrong if multiple ids have the same prefab
            if(UnitSpecificCheck())
            {
                TileAssistant.CurrencyManager.SpendCurrency(PlayerCurrencyManager.CurrencyTypes.CASH, Information.SpawnCost);
                SetupAfterPlacement();
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    
    //will add three more types, LandStatic, SeaStatic,AirStatic
    private bool UnitSpecificCheck()
    {
        switch (Information.UnitType)
        {
            case "Land":
                return !TileAssistant.OverTile.WaterTile;
            case "Sea":
                return TileAssistant.OverTile.WaterTile;
            case "Air":
                return true;
        }
        return true;
    }

    private void SetupAfterPlacement()
    {
        this.transform.position = TileAssistant.OverTile.transform.position;
        TileAssistant.OverTile.OccupiedBy = this;
        TileAssistant.OverTile.RefreshVisualState();
        _ParentTile = TileAssistant.OverTile;
        SpecialSetupByUnitType();
    }

    private void SpecialSetupByUnitType()
    {
        ComingIn = true;
        switch (Information.UnitType)
        {
            case "Land":
                PosOffset = new Vector3();
                this.transform.position += new Vector3(0, 10, 0);
                this.gameObject.AddComponent<BoxCollider>();
                _ParentTile.GetComponent<BoxCollider>().isTrigger = false;
                StartCoroutine(WaitForArrivaLand());
                break;
            case "Air":
                //put in the air
                PosOffset =  new Vector3(0, 2, 0);
                this.transform.position = new Vector3(Random.Range(0, TileAssistant.Board.Count), 4, -10);
                //need to do rotation at some point
                StartCoroutine(WaitForArrivalAir());
                break;
            case "Sea":
                //drop in the water a bit
                this.transform.position += new Vector3(0, -2, 0);
                PosOffset = new Vector3(0, -.25f, 0);
                StartCoroutine(WaitForArrivalSea());
                break;
        }
        
        this.transform.position += PosOffset;
    }

    IEnumerator WaitForArrivaLand()
    {
        while (this.transform.position.y >= _ParentTile.transform.position.y + .1f)
        {
            yield return null;
        }
        Destroy(gameObject.GetComponent<BoxCollider>());
        Destroy(gameObject.GetComponent<Rigidbody>());//might need at some point for death explosion
        _ParentTile.GetComponent<BoxCollider>().isTrigger = true;
        this.transform.position = _ParentTile.transform.position;
        ComingIn = false;
    }

    IEnumerator WaitForArrivalAir()
    {
        while(this.transform.position != _ParentTile.transform.position + PosOffset)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, _ParentTile.transform.position + PosOffset, .1f);//might need to tweak this depending on plane type
            yield return null;
        }
        ComingIn = false;       
    }

    //for now just have it come up from below
    IEnumerator WaitForArrivalSea()
    {
        while (this.transform.position != _ParentTile.transform.position + PosOffset)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, _ParentTile.transform.position + PosOffset, .1f);//might need to tweak this depending on plane type
            
            yield return null;
        }
        ComingIn = false;
    }




}
