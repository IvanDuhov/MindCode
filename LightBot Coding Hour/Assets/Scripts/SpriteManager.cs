using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpriteManager : MonoBehaviour
{
    public Sprite avatar1;
    public Sprite avatar1Selected;

    public Sprite avatar2;
    public Sprite avatar2Selected;

    public Sprite avatar3;
    public Sprite avatar3Selected;

    public Image place1;
    public Image place2;
    public Image place3;

    public Text nickname;
    public Text placeHolder;

    public Transform profileMenu;

    private void Start()
    {
        // If it is in the main menu
        if (place1 != null)
        {
            int avatar = PlayerPrefs.GetInt("avatar");

            switch (avatar)
            {
                case 1:
                    SelectAvatar1();
                    break;
                case 2:
                    SelectAvatar2();
                    break;
                case 3:
                    SelectAvatar3();
                    break;
            }
        }
    }

    public void SelectAvatar1()
    {
        place1.sprite = avatar1Selected;

        place2.sprite = avatar2;
        place3.sprite = avatar3;

        PlayerPrefs.SetInt("avatar", 1);
    }

    public void SelectAvatar2()
    {
        place2.sprite = avatar2Selected;

        place1.sprite = avatar1;
        place3.sprite = avatar3;

        PlayerPrefs.SetInt("avatar", 2);
    }

    public void SelectAvatar3()
    {
        place3.sprite = avatar3Selected;

        place2.sprite = avatar2;
        place1.sprite = avatar1;

        PlayerPrefs.SetInt("avatar", 3);
    }

    public void SaveProfile()
    {
        PlayerPrefs.SetString("nickname", nickname.text);
        profileMenu.gameObject.SetActive(false);
    }

    public void OpenProfileSettings()
    {
        placeHolder.text = PlayerPrefs.GetString("nickname");
        print(nickname.text);
        profileMenu.gameObject.SetActive(true);
    }


}
