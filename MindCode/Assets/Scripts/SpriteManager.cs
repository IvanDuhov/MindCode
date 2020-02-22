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

    public Text singleplayerText;
    public Text multiplayerText;
    public Text profileText;
    public Text optionsText;
    public Text exitText;

    public Text profileSettings;
    public Text profileAvatar;
    public Text profileEnterUsername;
    public Text profileUsernamePlaceholer;
    public Text profileSave;

    public Text options;
    public Text optionsLanguage;
    public Text optionsVolume;
    public Text optionsSave;

    public Sprite english;
    public Sprite englishSelected;

    public Scrollbar musicVolume;

    public Button easy;
    public Button hard;
    public Button play;

    public Sprite diffPanel;
    public Sprite diffPanelSelected;

    public Transform multiHelp;
    public Text multiHelpText;
    public Text statistics;

    private MultiplayerManager mm;

    private void Start()
    {
        mm = FindObjectOfType<MultiplayerManager>();

        if (SceneManager.GetActiveScene().name == "Menu")
        {
            Translate();

            if (PlayerPrefs.GetString("nickname") == null || PlayerPrefs.GetString("nickname") == "")
            {
                PlayerPrefs.SetString("nickname", "default-" + UnityEngine.Random.Range(1, 1000000));
            }
        }

        if (SceneManager.GetActiveScene().name == "MultiplayerMenu")
        {
            if (PlayerPrefs.GetString("English") == "true")
            {
                multiHelpText.text = "The multiplayer is an actual battle between two players. Depending on the difficulty a map level is being randomly selected and the players need to provide the best solution they can come up with. They have 90 seconds to provide their solution by clicking on the green play button. The game rules are the same as in the single-player. The winner is the one who has lighten up most blue tiles if the enlighten blue tiles are even, then the number of orders used is compared, if it is even, too, then the time need to the players to provide the solution. Only the best-provided solution is taken into account, which means if you first provide a solution enlighting two blue tiles and then try a solution and it enlights only two tiles, that solution wouldn't be uploaded and used to determine the winner of the battle.";
            }
            else
            {
                multiHelpText.text = "Мултиплеърът е битка между двама играчи. В зависимост от трудността, нивото, на което ще се съревновават двамата играчи, се избира на случаен принцип. Те разполагат с 90 секунди, за да осигурят своето възможно най-добро решение. Правилата на играта са същите като в самостоятелната игра. Победител е този, който е светнал повечето сини полета, ако осветените сини полета са равен брой, тогава се сравнява броят на използваните команди, ако той също е равен, тогава се сравнява времето, за което играчите са измислили решенията си. Взема се предвид само най-доброто предоставено решение, което означава че, ако първото предоставено решение е осветило две сини полета, а след това играчът изпробва друго решение решение и то освети само две сини полета, това решение няма да бъде качено и използвано за определяне на победителя в двубоя.";
            }

            switch (PlayerPrefs.GetString("diff"))
            {
                case "easy":
                    SelectEasyDiff();
                    break;
                case "hard":
                    SelectHardDiff();
                    break;
            }
        }
    }

    public void MultiplayerHelpMenu()
    {
        multiHelp.gameObject.SetActive(true);
    }

    public void MultiplayerHelpOk()
    {
        multiHelp.gameObject.SetActive(false);
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

        Translate();

        bgImage.sprite = bulgarian;
        engImage.sprite = englishSelected;
    }

    public void SelectBulgarian()
    {
        PlayerPrefs.SetString("English", "false");

        Translate();

        bgImage.sprite = bulgarianSelected;
        engImage.sprite = english;
    }

    public void Translate()
    {
        if (PlayerPrefs.GetString("English") == "true")
        {
            singleplayerText.text = "Singleplayer";
            multiplayerText.text = "Multiplayer";
            profileText.text = "Profile";
            optionsText.text = "Options";
            exitText.text = "Exit";

            profileSettings.text = "Profile settings";
            profileAvatar.text = "Select avatar";
            profileEnterUsername.text = "Enter your username";
            profileUsernamePlaceholer.text = "Username ...";
            profileSave.text = "Save";

            options.text = "Options";
            optionsLanguage.text = "Select language";
            optionsVolume.text = "Control your music volume";
            optionsSave.text = "Save";
            statistics.text = "Statistics";
        }
        else
        {
            singleplayerText.text = "Самостоятелна игра";
            multiplayerText.text = "Групова игра";
            profileText.text = "Профил";
            optionsText.text = "Настройки";
            exitText.text = "Изход";

            profileSettings.text = "Настройки на профила";
            profileAvatar.text = "Избери си аватар";
            profileEnterUsername.text = "Въведи потребителско име";
            profileUsernamePlaceholer.text = "Потребителско име ...";
            profileSave.text = "Запази";

            options.text = "Настройки";
            optionsLanguage.text = "Избери си език";
            optionsVolume.text = "Контролирай силата на звука";
            optionsSave.text = "Запази";
            statistics.text = "Статистики";
        }
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

    public void SelectEasyDiff()
    {
        easy.GetComponent<Image>().sprite = diffPanelSelected;

        PlayerPrefs.SetString("diff", "easy");

        hard.GetComponent<Image>().sprite = diffPanel;
    }

    public void SelectHardDiff()
    {
        hard.GetComponent<Image>().sprite = diffPanelSelected;

        PlayerPrefs.SetString("diff", "hard");

        easy.GetComponent<Image>().sprite = diffPanel;
    }
}
