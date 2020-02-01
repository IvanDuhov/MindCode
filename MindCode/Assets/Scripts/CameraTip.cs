using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Transform hint;

    public Text hintText;

    private void Start()
    {
        if (PlayerPrefs.GetString("language") == "true")
        {
            hintText.text = "You can change the camera views by pressing the Tab button.";
        }
        else
        {
            hintText.text = "Можете да сменяте режима на камерата като натиснете Tab.";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hint.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hint.gameObject.SetActive(false);
    }
}
