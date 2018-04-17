using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {

    [SerializeField] private CardVisualScript _Card;
    [SerializeField] private Transform _HandParent;
    [SerializeField] private Transform _CanvasTrans;
    [SerializeField] private UnitCreationManager _CreationManager;

    private List<CardVisualScript> _ActiveCards;

    const int HAND_SIZE = 5;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void OnClickShowCards()
    {
        //maybe do some animation at some pt
    }

    public void OnCardSelected(BaseObjectInformation UI, CardVisualScript.CardTypes CardType, System.Action PlacedCallback, System.Action<GameObject> CreatedCallback)
    {
        _CreationManager.SpawnUnit(UI.PrefabName,CardType,PlacedCallback,CreatedCallback);
    }

    public void DataLoadFinished()
    {
        _ActiveCards = new List<CardVisualScript>();
        for (int i = 0; i < HAND_SIZE; i++)
        {
            CardVisualScript CVS = Instantiate(_Card, _HandParent);
            _ActiveCards.Add(CVS);
            CVS.OnSelectAction = OnCardSelected;
            //At some point we will just have a dictionary of the users deck or something
            if(Random.Range(0,2) == 1)
            {
                CVS.SetCardInformation(DataManagerInstance.Instance.DataClass.GetRandomUnit(),this);
                CVS.CardType = CardVisualScript.CardTypes.UNIT;
            }
            else
            {
                CVS.SetCardInformation(DataManagerInstance.Instance.DataClass.GetRandomBank(),this);
                CVS.CardType = CardVisualScript.CardTypes.BANK;
            }
            
        }
    }

    public void AddCard()
    {
        if (_ActiveCards.Count < HAND_SIZE)
        {
            CardVisualScript CVS = Instantiate(_Card, _HandParent);
            _ActiveCards.Add(CVS);
            CVS.OnSelectAction = OnCardSelected;
            //At some point we will just have a dictionary of the users deck or something
            if (Random.Range(0, 2) == 1)
            {
                CVS.SetCardInformation(DataManagerInstance.Instance.DataClass.GetRandomUnit(),this);
                CVS.CardType = CardVisualScript.CardTypes.UNIT;
            }
            else
            {
                CVS.SetCardInformation(DataManagerInstance.Instance.DataClass.GetRandomBank(),this);
                CVS.CardType = CardVisualScript.CardTypes.BANK;
            }
        }
    }

    public void RemoveCard(CardVisualScript cvs)
    {
        _ActiveCards.Remove(cvs);
    }
}
