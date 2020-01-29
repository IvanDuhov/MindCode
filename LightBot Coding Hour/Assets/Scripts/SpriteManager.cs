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

    public Text winnerNameLabel;
    public Text secondPlaceNameLabel;

    public Text winnerData;
    public Text secondPlaceData;

    public Transform optionsMenu;
    public Image bgImage;
    public Image engImage;

    public Sprite bulgarian;
    public Sprite bulgarianSelected;

    public Sprite english;
    public Sprite englishSelected;

    public Scrollbar musicVolume;

    private void Start()
    {
        // If it is in the main menu
        if (place1 != null && place3 != null && bgImage != null)
        {

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
        if (nickname.text == null || nickname.text == "")
        {
            if (PlayerPrefs.GetString("nickname") == null || PlayerPrefs.GetString("nickname") == "")
            {
                PlayerPrefs.SetString("nickname", "default-" + UnityEngine.Random.Range(1, 20000000));
                print("Should be set system nickname");
            }
            else
            {
                print("something not okay");
                PlayerPrefs.SetString("nickname", PlayerPrefs.GetString("nickname"));
            }
        }
        else
        {
            PlayerPrefs.SetString("nickname", nickname.text);
        }

        profileMenu.gameObject.SetActive(false);
    }

    public void OpenProfileSettings()
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

        placeHolder.text = PlayerPrefs.GetString("nickname");
        nickname.text = PlayerPrefs.GetString("nickname");

        profileMenu.gameObject.SetActive(true);
    }

    public void SelectEnglish()
    {
        PlayerPrefs.SetString("English", "true");

        bgImage.sprite = bulgarian;
        engImage.sprite = englishSelected;
    }

    public void SelectBulgarian()
    {
        PlayerPrefs.SetString("English", "false");

        bgImage.sprite = bulgarianSelected;
        engImage.sprite = english;
    }

    public void OpenOptions()
    {
        string lng = PlayerPrefs.GetString("English");

        if (lng == "true")
        {
            SelectEnglish();
        }
        else
        {
            SelectBulgarian();
        }

        // Make sure it doesn't set the volume to null when the game is started for the first time
        if (PlayerPrefs.GetFloat("volume") != null)
        {
            musicVolume.value = PlayerPrefs.GetFloat("volume");
        }

        optionsMenu.gameObject.SetActive(true);
    }

    public void SaveOptions()
    {
        optionsMenu.gameObject.SetActive(false);

        PlayerPrefs.SetFloat("volume", musicVolume.value);

        AudioSource ass = FindObjectOfType<AudioSource>();
        ass.volume = musicVolume.value;
    }

    public Sprite ReturnAvatar(PlayerProfile player)
    {
        switch (player.avatar)
        {
            case 1:
                return avatar1;
            case 2:
                return avatar2;
            case 3:
                return avatar3;
            default:
                return avatar1;
        }
    }


}
