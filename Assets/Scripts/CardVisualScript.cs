using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardVisualScript : MonoBehaviour
{ 

    public enum CardTypes
    {
        UNIT,
        BANK
    }

    [SerializeField] private Text _Name;
    [SerializeField] private Text _Attack;
    [SerializeField] private Text _Defense;
    [SerializeField] private Text _Movement;
    [SerializeField] private Text _SpawnCost;
    [SerializeField] private GameObject _CancelCard;
    [SerializeField] private GameObject _BaseCard;
    //maybe range for ranged units?

    [SerializeField] private Text _Description;

    [SerializeField] private Image _Image;

    [SerializeField] private LayoutElement _Layout;

    public System.Action<BaseObjectInformation,CardTypes,System.Action,System.Action<GameObject>> OnSelectAction;//at some point it will return a data object that will be attached to it
    public CardTypes CardType;

    private UnitInformation _UnitInformation = null;//may at some point need something different for upgrade cards
    private BankInformation _BankInformation = null;

    private CardManager _CardManager;

    private bool _CancelState = false;
    private GameObject _CreatedObject = null;

    //private Transform _Parent;
    //private int _SiblingIndex;

   // private bool _Selected = false;

	// Use this for initialization
	void Start () {
       // _Parent = this.transform.parent;
        //_SiblingIndex = this.transform.GetSiblingIndex();
	}
	
	// Update is called once per frame
	void Update () {
        /*
        if(_CreatedObject != null && Input.GetMouseButtonDown(1))
        {
            CancelAction();
        }
        */
	}

    private void CancelAction()
    {
        _CancelState = false;
        _CancelCard.SetActive(false);
        _BaseCard.SetActive(true);
        _Image.enabled = true;
        if (_CreatedObject != null)
        {
            Destroy(_CreatedObject);
        }
    }

    public void OnClickCard()
    {
        if(_CancelState)
        {
            CancelAction();
            return;
        }
        if(OnSelectAction != null)
        {
            BaseObjectInformation BOI = null;
            if (_UnitInformation != null)
            {
                BOI = _UnitInformation;
                if (TileAssistant.CurrencyManager.GetAmountOfCurrency(PlayerCurrencyManager.CurrencyTypes.CASH) < _UnitInformation.SpawnCost) //just one currency right now
                {
                    return;
                }
            }
            else if (_BankInformation != null)
            {
                BOI = _BankInformation;
                if (TileAssistant.TurnManager.HasPlacedBank)
                {
                    return;
                }
            }
            else
            {
                Debug.LogWarning("Error, information is null");
            }

            OnSelectAction.Invoke(BOI,CardType,PlacedCallback,CreatedCallback);
            _CancelState = true;
            _CancelCard.SetActive(true);
            _BaseCard.SetActive(false);
            _Image.enabled = false;
        }
        //Destroy self?
        //_CancelState = true;
        //_CancelCard.SetActive(true);
        //_BaseCard.SetActive(false);

        //_CardManager.RemoveCard(this);
        //Destroy(this.gameObject);
    }

    public void SetCardInformation(UnitInformation information,CardManager parent)
    {
        if(information == null)
        {
            Debug.LogWarning("Info is null!");
            return;
        }
        _UnitInformation = information;
        _Name.text = _UnitInformation.Name;
        _Attack.text = _UnitInformation.AttackValue.ToString();
        _Defense.text = _UnitInformation.DefenseValue.ToString();
        _Movement.text = _UnitInformation.MovementValue.ToString();
        _SpawnCost.text = _UnitInformation.SpawnCost.ToString();
        _Description.text = _UnitInformation.Description;
        _CardManager = parent;
    }

    public void SetCardInformation(BankInformation information,CardManager parent)
    {
        if (information == null)
        {
            Debug.LogWarning("Info is null!");
            return;
        }
        _BankInformation = information;
        _Name.text = _BankInformation.Name;
        _Attack.text = _BankInformation.GoldPerTurn.ToString();
        _Attack.GetComponent<ToolTipHelper>().CurrentHelpType = ToolTipHelper.HelpType.MONEYAMT;
        _Description.text = _BankInformation.Description;
        _SpawnCost.gameObject.SetActive(false);
        _Defense.gameObject.SetActive(false);
        _Movement.gameObject.SetActive(false);
        _CardManager = parent;
    }

    private void PlacedCallback()
    {
        _CardManager.RemoveCard(this);
        Destroy(this.gameObject);
    }

    private void CreatedCallback(GameObject g)
    {
        _CreatedObject = g;
    }

    //do later, some fancy spawn

    /*
    public virtual void OnPointerDown(PointerEventData eventData)
    {
        Debug.LogWarning("Doi");
        _Layout.ignoreLayout = true;
        this.transform.SetParent(_CanvasRef);
        _Selected = true;
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        this.transform.SetParent(_Parent);
        _Layout.ignoreLayout = false;
        _Selected = false;
    }
    */
}
