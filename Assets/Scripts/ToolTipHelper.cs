using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTipHelper :MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{ 
    public enum HelpType
    {
        ATTACK,
        DEFENSE,
        MOVEMENT,
        SPAWNCOST,
        MONEYAMT
    }

    public HelpType CurrentHelpType;

    const float SHOWTIME = .5f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        Invoke("ShowText", SHOWTIME);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CancelInvoke("ShowText");
        TileAssistant.ToolTip.HideText();
    }

    public void ShowText()
    {
        switch(CurrentHelpType)
        {
            case HelpType.ATTACK:
                TileAssistant.ToolTip.ShowText("This is the attack amount.");
                break;
            case HelpType.DEFENSE:
                TileAssistant.ToolTip.ShowText("This is the defense amount.");
                break;
            case HelpType.MOVEMENT:
                TileAssistant.ToolTip.ShowText("This is the movement amount in tiles.");
                break;
            case HelpType.SPAWNCOST:
                TileAssistant.ToolTip.ShowText("This is the how much this unit costs.");
                break;
            case HelpType.MONEYAMT:
                TileAssistant.ToolTip.ShowText("This is how much money is generated per turn");
                break;
        }
    }

}


