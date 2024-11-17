using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreditsLinks : MonoBehaviour, IPointerClickHandler
{
    private TMP_Text tmpText;

    private Dictionary<int, string> links = new();

    private void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(tmpText, eventData.position, null);

        Debug.Log(linkIndex);

        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = tmpText.textInfo.linkInfo[linkIndex];
            HandleLinkClick(linkInfo);
        }
    }

    private void HandleLinkClick(TMP_LinkInfo linkInfo)
    {
        switch (linkInfo.GetLinkID())
        {
            case "1":
                Application.OpenURL("https://www.xproff.ru");
                break;
            case "2":
                Application.OpenURL("https://t.me/necrodever");
                break;
            case "3":
                Application.OpenURL("https://t.me/matthewmercer01");
                break;
            default:
                Debug.Log("Link clicked: " + linkInfo.GetLinkID());
                break;
        }
    }
}
