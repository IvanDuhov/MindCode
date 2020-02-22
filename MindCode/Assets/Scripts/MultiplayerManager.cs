using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Net;
using System.Threading.Tasks;

public class MultiplayerManager : MonoBehaviour
{
    // public string MJpath = Application.streamingAssetsPath + "/" + "Test.json";

    public const string filename = "Test.json";
    public string jsonPath = "";

    public string username = "default";

    public bool signedIn = false;
    public bool showWinner = false;

    public bool jsonUpdated = false;
    public bool uploadinJSON = false;

    public Transform matchmakingPanel;

    private int secs;

    private string battleScene = "Level 6";

    public int bSecs = 90;

    public Text TimeText;
    public Text FoundGame;
    public Text resultsLabel;
    public Text resultsOk;

    public Text playLabel;
    public Text easyLabel;
    public Text hardLabel;
    public Text cancelLabel;

    public GameObject resultPanel;

    public bool TestMultiplayer = false;

    private PlayerProfile myProfile;

    public Transform sorryBusyServer;
    public Text busyServerText;

    public static string localDBPath = Path.Combine(Application.streamingAssetsPath, "LocalDB.json");

    private void Awake()
    {
        jsonPath = Path.Combine(Application.streamingAssetsPath, filename);

        username = PlayerPrefs.GetString("nickname");

        var mmSecondInstance = FindObjectOfType<MultiplayerManager>();

        if (mmSecondInstance != this.gameObject)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(KeepJSONUpdated());

        var allMM = FindObjectsOfType<MultiplayerManager>();

        if (allMM.Length >= 2)
        {
            foreach (var item in allMM)
            {
                if (item != this.gameObject && item.showWinner)
                {
                    ShowWinner();
                    Destroy(item.gameObject);
                }
            }
        }

        if (PlayerPrefs.GetString("English") == "true")
        {
            playLabel.text = "Play";
            easyLabel.text = "Easy";
            hardLabel.text = "Hard";
            cancelLabel.text = "Cancel";
            busyServerText.text = "Our servers are full, please wait or try to select different difficulty.";
        }
        else
        {
            playLabel.text = "Играй";
            easyLabel.text = "Лесно";
            hardLabel.text = "Трудно";
            cancelLabel.text = "Спри";
            busyServerText.text = "Сървърите ни са пълни, моля изчакайте или изберете друга трудност.";
        }

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 300;
    }

    public void BusyOk()
    {
        sorryBusyServer.gameObject.SetActive(false);
    }

    // Procedure to sign the user that he wished to play multiplayer
    public void SignInForMultiplayer()
    {
        // trigger to stop the repeating downloading of the JSON file from the FTP server and work with the latest one
        uploadinJSON = true;

        // Unsigning if the user doesn;t want to play multiplayer anymore
        if (signedIn)
        {
            signedIn = false;

            secs = 0;

            MJ Mjb = ReadJson(jsonPath);

            List<PlayerProfile> allPlayers = new List<PlayerProfile>();

            switch (PlayerPrefs.GetString("diff"))
            {
                case "easy":
                    // Deleting the query of this user, so the server knows that this user doesn't want to play multiplayer anymore
                    foreach (var item in Mjb.Players)
                    {
                        if (item.Nickname != username)
                        {
                            allPlayers.Add(item);
                        }
                    }

                    Mjb.Players = allPlayers;

                    break;
                case "hard":
                    // Deleting the query of this user, so the server knows that this user doesn't want to play multiplayer anymore
                    foreach (var item in Mjb.PlayersHard)
                    {
                        if (item.Nickname != username)
                        {
                            allPlayers.Add(item);
                        }
                    }

                    Mjb.PlayersHard = allPlayers;
                    break;
                default:
                    // Deleting the query of this user, so the server knows that this user doesn't want to play multiplayer anymore
                    foreach (var item in Mjb.Players)
                    {
                        if (item.Nickname != username)
                        {
                            allPlayers.Add(item);
                        }
                    }

                    Mjb.Players = allPlayers;
                    break;
            }

            // Saving the signed JSON
            SaveToJson(Mjb, jsonPath);

            // Uploading the JSON to the FTP server
            //ftpClient.upload(@"Test.json", jsonPath);
            UploadInServer();

            // Triggering again the auto updation of the JSON file from the FTP server
            uploadinJSON = false;
            StartCoroutine(KeepJSONUpdated());

            matchmakingPanel.gameObject.SetActive(false);

            print("succesfully signed off");

            return;
        }

        // Reading the JSON file
        MJ mjb = ReadJson(jsonPath);

        if (PlayerPrefs.GetString("diff") == "easy")
        {
            if (mjb.Players.Count >= 2)
            {
                sorryBusyServer.gameObject.SetActive(true);
                return;
            }
        }
        else
        {
            if (mjb.PlayersHard.Count >= 2)
            {
                sorryBusyServer.gameObject.SetActive(true);
                return;
            }
        }

        // Reading the JSON file
        mjb = ReadJson(jsonPath);

        // Making active the matchamkiing meny, where the user can see for how long he is waiting and how many players there are at the queue
        matchmakingPanel.gameObject.SetActive(true);

        switch (PlayerPrefs.GetString("diff"))
        {
            case "easy":
                // IF this is the first user to sign in for multiplayer, we are creating new user in the queue for multiplayer
                if (mjb.Players.Count == 0)
                {
                    mjb.Players.Add(new PlayerProfile());
                    mjb.Players[0].Nickname = username;
                    mjb.Players[0].avatar = (byte)PlayerPrefs.GetInt("avatar");

                    if (mjb.EasyLevel == "" || mjb.EasyLevel == null)
                    {
                        mjb.EasyLevel = SelectRandomEasyLevel();
                    }
                }
                else // If not, we are checking if there is an empty record in the queue
                {
                    // trigger to mark if an empty record was found in the queue
                    bool saved = false;

                    for (int i = 0; i < mjb.Players.Count; i++)
                    {
                        // if there is an empty record, we are filling it and signing this user at that record
                        if (mjb.Players[i].Nickname == null)
                        {
                            mjb.Players[i].Nickname = username;
                            mjb.Players[i].avatar = (byte)PlayerPrefs.GetInt("avatar");
                            saved = true;
                            break;
                        }
                    }

                    // If there wasn't an empty record to fill, we are creating a brand new one
                    if (!saved)
                    {
                        print("Player added");
                        mjb.Players.Add(new PlayerProfile());
                        mjb.Players[mjb.Players.Count - 1].Nickname = username;
                        mjb.Players[mjb.Players.Count - 1].avatar = (byte)PlayerPrefs.GetInt("avatar");
                    }
                }
                break;
            case "hard":
                // IF this is the first user to sign in for multiplayer, we are creating new user in the queue for multiplayer
                if (mjb.PlayersHard.Count == 0)
                {
                    mjb.PlayersHard.Add(new PlayerProfile());
                    mjb.PlayersHard[0].Nickname = username;
                    mjb.PlayersHard[0].avatar = (byte)PlayerPrefs.GetInt("avatar");

                    if (mjb.HardLevel == "" || mjb.HardLevel == null)
                    {
                        mjb.HardLevel = SelectRandomHardLevel();
                    }
                }
                else // If not, we are checking if there is an empty record in the queue
                {
                    // trigger to mark if an empty record was found in the queue
                    bool saved = false;

                    for (int i = 0; i < mjb.PlayersHard.Count; i++)
                    {
                        // if there is an empty record, we are filling it and signing this user at that record
                        if (mjb.PlayersHard[i].Nickname == null)
                        {
                            mjb.PlayersHard[i].Nickname = username;
                            mjb.PlayersHard[i].avatar = (byte)PlayerPrefs.GetInt("avatar");
                            saved = true;
                            break;
                        }
                    }

                    // If there wasn't an empty record to fill, we are creating a brand new one
                    if (!saved)
                    {
                        print("Player added");
                        mjb.PlayersHard.Add(new PlayerProfile());
                        mjb.PlayersHard[mjb.PlayersHard.Count - 1].Nickname = username;
                        mjb.PlayersHard[mjb.PlayersHard.Count - 1].avatar = (byte)PlayerPrefs.GetInt("avatar");
                    }
                }
                break;
            default:
                break;
        }

        // Marks that the user is already signed for the multiplayer Queue
        signedIn = true;

        // Starts a coroutine that seeks how many people are searching for partners and matching them up
        StartCoroutine(SeekForOtherPlayers());

        // Saving the signed JSON
        SaveToJson(mjb, jsonPath);

        // Uploading the JSON to the FTP server
        //ftpClient.upload(@"Test.json", jsonPath);
        UploadInServer();

        // Triggering again the auto updation of the JSON file from the FTP server
        uploadinJSON = false;
        StartCoroutine(KeepJSONUpdated());
    }

    // Checking the FTP if there are enough signed in players to play and if there are, it notifies the users and starts the game
    private IEnumerator SeekForOtherPlayers()
    {
        StartCoroutine(Timer());

        while (signedIn)
        {
            // Check the ftp file for data every 1/4 sec
            yield return new WaitForSeconds(1f);

            MJ mjb = ReadJson(jsonPath);

            string gameMode = PlayerPrefs.GetString("diff");

            switch (gameMode)
            {
                case "easy":
                    // if there are enough players do that:
                    if (mjb.Players.Count == 2 || TestMultiplayer)
                    {
                        // Instead of this make text says taht is has found enoguh players to play
                        FoundGame.text = "Game found for " + DisplaySeconds(secs);
                        StopCoroutine(SeekForOtherPlayers());
                        StopCoroutine(Timer());
                        signedIn = false;

                        mjb.CurrentlyPlaying = true;
                        SaveToJson(mjb, jsonPath);
                        UploadInServer();

                        SceneManager.LoadScene(mjb.EasyLevel);
                    }
                    break;
                case "hard":
                    // if there are enough players do that:
                    if (mjb.PlayersHard.Count == 2 || TestMultiplayer)
                    {
                        // Instead of this make text says taht is has found enoguh players to play
                        FoundGame.text = "Game found for " + DisplaySeconds(secs);
                        StopCoroutine(SeekForOtherPlayers());
                        StopCoroutine(Timer());
                        signedIn = false;

                        mjb.CurrentlyPlaying = true;
                        SaveToJson(mjb, jsonPath);
                        UploadInServer();

                        SceneManager.LoadScene(mjb.HardLevel);
                    }
                    break;
                default:
                    if (mjb.Players.Count == 2 || TestMultiplayer)
                    {
                        // Instead of this make text says taht is has found enoguh players to play
                        FoundGame.text = "Game found for " + DisplaySeconds(secs);
                        StopCoroutine(SeekForOtherPlayers());
                        StopCoroutine(Timer());
                        signedIn = false;

                        mjb.CurrentlyPlaying = true;
                        SaveToJson(mjb, jsonPath);
                        UploadInServer();

                        SceneManager.LoadScene("Level 4");
                    }
                    break;
            }
        }
    }

    private string SelectRandomEasyLevel()
    {
        switch (UnityEngine.Random.Range(4, 8))
        {
            case 4:
                return "Level 4";
            case 5:
                return "Level 5";
            case 6:
                return "Level 6";
            case 7:
                return "Level 7";
            case 8:
                return "Level 8";

            default:
                return "Level 6";
        }
    }

    private string SelectRandomHardLevel()
    {
        switch (UnityEngine.Random.Range(2, 6))
        {
            case 2:
                return "Level P2";
            case 3:
                return "Level P3";
            case 5:
                return "Level P5";
            case 6:
                return "Level P6";

            default:
                return "Level P6";
        }
    }

    IEnumerator Timer()
    {
        while (signedIn)
        {
            yield return new WaitForSeconds(1f);

            secs++;

            if (PlayerPrefs.GetString("English") == "true")
            {
                TimeText.text = "Time searching: " + DisplaySeconds(secs);
            }
            else
            {
                TimeText.text = "Изминало време в търсене: " + DisplaySeconds(secs);
            }
        }
    }

    public string DisplaySeconds(int secs)
    {
        if (secs < 10)
        {
            return "0:0" + secs;
        }
        else if ((secs % 60) < 10)
        {
            return (secs / 60) + ":0" + (secs % 60);
        }

        return (secs / 60) + ":" + (secs % 60);
    }

    public IEnumerator BattleTimer(Text timerText)
    {
        while (bSecs != 0)
        {
            bSecs--;
            timerText.text = DisplaySeconds(bSecs);
            yield return new WaitForSeconds(1f);
        }

        MJ mjb = ReadJson(jsonPath);
        mjb.CurrentlyPlaying = false;

        switch (PlayerPrefs.GetString("diff"))
        {
            case "easy":
                foreach (var item in mjb.Players)
                {
                    if (item.Nickname == username)
                    {
                        item.showed = true;
                        break;
                    }
                }
                break;
            case "hard":
                foreach (var item in mjb.PlayersHard)
                {
                    if (item.Nickname == username)
                    {
                        item.showed = true;
                        break;
                    }
                }
                break;
            default:
                break;
        }

        SaveToJson(mjb, jsonPath);
        showWinner = true;
        UploadInServer();

        bool waitingForOpponed = true;

        while (waitingForOpponed)
        {
            waitingForOpponed = false;

            mjb = ReadJson(jsonPath);

            // Checking if all players are ready for the results to be shown
            switch (PlayerPrefs.GetString("diff"))
            {
                case "easy":
                    foreach (var item in mjb.Players)
                    {
                        if (!item.showed)
                        {
                            waitingForOpponed = true;
                            break;
                        }
                    }
                    break;
                case "hard":
                    foreach (var item in mjb.PlayersHard)
                    {
                        if (!item.showed)
                        {
                            waitingForOpponed = true;
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }

            // Hitting the database that this user is ready to see the data
            switch (PlayerPrefs.GetString("diff"))
            {
                case "easy":
                    foreach (var item in mjb.Players)
                    {
                        if (item.Nickname == username)
                        {
                            item.showed = true;
                            break;
                        }
                    }
                    break;
                case "hard":
                    foreach (var item in mjb.PlayersHard)
                    {
                        if (item.Nickname == username)
                        {
                            item.showed = true;
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
            print("This user should be shown.");

            SaveToJson(mjb, jsonPath);

            // Setting up the battle timer
            GameObject mmTimer = GameObject.Find("MMTimer");
            GameObject playButton = GameObject.Find("Start");

            playButton.GetComponent<Button>().enabled = false;
            print("Play button should be disabled");

            if (PlayerPrefs.GetString("English") == "true")
            {
                mmTimer.GetComponentInChildren<Text>().text = "Waiting...";
            }
            else
            {
                mmTimer.GetComponentInChildren<Text>().text = "Изчакване...";
            }

            UploadInServer();

            yield return new WaitForSeconds(1f);
        }

        SceneManager.LoadScene("MultiplayerMenu");
    }

    public void ShowWinner()
    {
        var allMM = FindObjectsOfType<MultiplayerManager>();
        print(allMM.Length);

        StartCoroutine(DownloadDataFromTheSerer());

        foreach (var item in allMM)
        {
            if (item.resultPanel != null)
            {
                resultPanel = item.resultPanel;
            }
        }

        resultPanel.SetActive(true);

        SpriteManager sm = FindObjectOfType<SpriteManager>();

        MJ mjb = ReadJson(jsonPath);

        string winner = "";
        int max = 0;

        switch (PlayerPrefs.GetString("diff"))
        {
            case "easy":
                foreach (var item in mjb.Players)
                {
                    if (item.AmountOfBlueTilesEnlightened >= max)
                    {
                        max = item.AmountOfBlueTilesEnlightened;
                        winner = item.Nickname;
                    }
                }
                foreach (var item in mjb.Players)
                {
                    if (item.Nickname == username)
                    {
                        item.showed = true;
                        break;
                    }
                }
                break;
            case "hard":
                foreach (var item in mjb.PlayersHard)
                {
                    if (item.AmountOfBlueTilesEnlightened >= max)
                    {
                        max = item.AmountOfBlueTilesEnlightened;
                        winner = item.Nickname;
                    }
                }
                foreach (var item in mjb.PlayersHard)
                {
                    if (item.Nickname == username)
                    {
                        item.showed = true;
                        break;
                    }
                }
                break;
            default:
                break;
        }

        SaveToJson(mjb, jsonPath);

        PlayerProfile winnerProfile = new PlayerProfile();
        PlayerProfile secondPlaceProfile = new PlayerProfile();

        switch (PlayerPrefs.GetString("diff"))
        {
            case "easy":
                if (mjb.Players.Count == 2)
                {
                    if (mjb.Players[0].Nickname == winner)
                    {
                        if (mjb.Players[0].AmountOfBlueTilesEnlightened == mjb.Players[1].AmountOfBlueTilesEnlightened)
                        {
                            if (mjb.Players[0].AmountOfOrders > mjb.Players[1].AmountOfOrders)
                            {
                                winnerProfile = mjb.Players[1];
                                secondPlaceProfile = mjb.Players[0];
                            }
                            else if (mjb.Players[0].AmountOfOrders == mjb.Players[1].AmountOfOrders)
                            {
                                if (mjb.Players[0].timeInSeconds > mjb.Players[1].timeInSeconds)
                                {
                                    winnerProfile = mjb.Players[1];
                                    secondPlaceProfile = mjb.Players[0];
                                }
                                else
                                {
                                    winnerProfile = mjb.Players[0];
                                    secondPlaceProfile = mjb.Players[1];
                                }
                            }
                            else
                            {
                                winnerProfile = mjb.Players[0];
                                secondPlaceProfile = mjb.Players[1];
                            }
                        }
                        else
                        {
                            winnerProfile = mjb.Players[0];
                            secondPlaceProfile = mjb.Players[1];
                        }
                    }
                    else if (mjb.Players[1].Nickname == winner)
                    {
                        if (mjb.Players[1].AmountOfBlueTilesEnlightened == mjb.Players[0].AmountOfBlueTilesEnlightened)
                        {
                            if (mjb.Players[1].AmountOfOrders > mjb.Players[0].AmountOfOrders)
                            {
                                winnerProfile = mjb.Players[0];
                                secondPlaceProfile = mjb.Players[1];
                            }
                            else if (mjb.Players[1].AmountOfOrders == mjb.Players[0].AmountOfOrders)
                            {
                                if (mjb.Players[1].timeInSeconds > mjb.Players[0].timeInSeconds)
                                {
                                    winnerProfile = mjb.Players[0];
                                    secondPlaceProfile = mjb.Players[1];
                                }
                                else
                                {
                                    winnerProfile = mjb.Players[1];
                                    secondPlaceProfile = mjb.Players[0];
                                }
                            }
                            else
                            {
                                winnerProfile = mjb.Players[1];
                                secondPlaceProfile = mjb.Players[0];
                            }
                        }
                        else
                        {
                            winnerProfile = mjb.Players[1];
                            secondPlaceProfile = mjb.Players[0];
                        }
                    }
                }
                break;
            case "hard":
                if (mjb.PlayersHard[0].Nickname == winner)
                {
                    if (mjb.PlayersHard[0].Nickname == winner)
                    {
                        if (mjb.PlayersHard[0].AmountOfBlueTilesEnlightened == mjb.PlayersHard[1].AmountOfBlueTilesEnlightened)
                        {
                            if (mjb.PlayersHard[0].AmountOfOrders > mjb.PlayersHard[1].AmountOfOrders)
                            {
                                winnerProfile = mjb.PlayersHard[1];
                                secondPlaceProfile = mjb.PlayersHard[0];
                            }
                            else if (mjb.PlayersHard[0].AmountOfOrders == mjb.PlayersHard[1].AmountOfOrders)
                            {
                                if (mjb.PlayersHard[0].timeInSeconds > mjb.PlayersHard[1].timeInSeconds)
                                {
                                    winnerProfile = mjb.PlayersHard[1];
                                    secondPlaceProfile = mjb.PlayersHard[0];
                                }
                                else
                                {
                                    winnerProfile = mjb.PlayersHard[0];
                                    secondPlaceProfile = mjb.PlayersHard[1];
                                }
                            }
                            else
                            {
                                winnerProfile = mjb.PlayersHard[0];
                                secondPlaceProfile = mjb.PlayersHard[1];
                            }
                        }
                        else
                        {
                            winnerProfile = mjb.PlayersHard[0];
                            secondPlaceProfile = mjb.PlayersHard[1];
                        }
                    }
                }
                else if (mjb.PlayersHard[1].Nickname == winner)
                {
                    if (mjb.PlayersHard[1].AmountOfBlueTilesEnlightened == mjb.PlayersHard[0].AmountOfBlueTilesEnlightened)
                    {
                        if (mjb.PlayersHard[1].AmountOfOrders > mjb.PlayersHard[0].AmountOfOrders)
                        {
                            winnerProfile = mjb.PlayersHard[0];
                            secondPlaceProfile = mjb.PlayersHard[1];
                        }
                        else if (mjb.PlayersHard[1].AmountOfOrders == mjb.PlayersHard[0].AmountOfOrders)
                        {
                            if (mjb.PlayersHard[1].timeInSeconds > mjb.PlayersHard[0].timeInSeconds)
                            {
                                winnerProfile = mjb.PlayersHard[0];
                                secondPlaceProfile = mjb.PlayersHard[1];
                            }
                            else
                            {
                                winnerProfile = mjb.PlayersHard[1];
                                secondPlaceProfile = mjb.PlayersHard[0];
                            }
                        }
                        else
                        {
                            winnerProfile = mjb.PlayersHard[1];
                            secondPlaceProfile = mjb.PlayersHard[0];
                        }
                    }
                    else
                    {
                        winnerProfile = mjb.PlayersHard[1];
                        secondPlaceProfile = mjb.PlayersHard[0];
                    }
                }

                break;
        }

        if (PlayerPrefs.GetString("English") == "true")
        {
            resultsLabel.text = "Results";
            resultsOk.text = "Ok";

            sm.winnerNameLabel.text = "Winner:\n" + winnerProfile.Nickname;
            sm.winnerData.text = string.Format("Amount of blue tiles enlighted: {0}\nAmount of orders used: {1}\nNeeded time to solve: {2} seconds", winnerProfile.AmountOfBlueTilesEnlightened, winnerProfile.AmountOfOrders, winnerProfile.timeInSeconds);

            sm.secondPlaceNameLabel.text = "Second place:\n" + secondPlaceProfile.Nickname;
            sm.secondPlaceData.text = string.Format("Amount of blue tiles enlighted: {0}\nAmount of orders used: {1}\nNeeded time to solve: {2} seconds", secondPlaceProfile.AmountOfBlueTilesEnlightened, secondPlaceProfile.AmountOfOrders, secondPlaceProfile.timeInSeconds);
        }
        else
        {
            resultsLabel.text = "Резултати";
            resultsOk.text = "Окей";

            sm.winnerNameLabel.text = "Победител:\n" + winnerProfile.Nickname;
            sm.winnerData.text = string.Format("Осветени полета: {0}\nБрой използвани команди: {1}\nНужно време за решаване: {2} секунди", winnerProfile.AmountOfBlueTilesEnlightened, winnerProfile.AmountOfOrders, winnerProfile.timeInSeconds);

            sm.secondPlaceNameLabel.text = "Губещ:\n" + secondPlaceProfile.Nickname;
            sm.secondPlaceData.text = string.Format("Осветени полета: {0}\nБрой използвани команди: {1}\nНужно време за решаване: {2} секунди", secondPlaceProfile.AmountOfBlueTilesEnlightened, secondPlaceProfile.AmountOfOrders, secondPlaceProfile.timeInSeconds);

        }

        // Gathering the data for the local Database
        string localDB = Path.Combine(Application.streamingAssetsPath, "LocalDB.json");
        StatObject so = ReadLocalDBJson(localDB);
        so.totalGames++;

        // Adding data for the statistics
        if (winnerProfile.Nickname == username)
        {
            so.wonGames++;
            so.totalSeconds = winnerProfile.timeInSeconds;
            so.totalOrders = winnerProfile.AmountOfOrders;
        }

        // Saving the data in the local db json file
        SaveToJson(so, localDB);

        print(winnerProfile.Nickname + " is the fuckign winner  with " + winnerProfile.AmountOfBlueTilesEnlightened);

        sm.place1.sprite = sm.ReturnAvatar(winnerProfile);
        sm.place2.sprite = sm.ReturnAvatar(secondPlaceProfile);

        showWinner = false;

        UploadInServer();
    }

    public void Ok()
    {
        resultPanel.gameObject.SetActive(false);
        MJ mjb = ReadJson(jsonPath);

        switch (PlayerPrefs.GetString("diff"))
        {
            case "easy":
                /*
                foreach (var item in mjb.Players)
                {
                    if (item.Nickname == username)
                    {
                        mjb.Players.Remove(item);
                    }
                }
                */

                bool deleteData = true;

                foreach (var item in mjb.Players)
                {
                    if (item.showed == false)
                    {
                        deleteData = false;
                        break;
                    }
                }

                if (deleteData)
                {
                    mjb.Players.Clear();
                    mjb.EasyLevel = "";
                }

                break;
            case "hard":
                /*
                foreach (var item in mjb.PlayersHard)
                {
                    if (item.Nickname == username)
                    {
                        mjb.PlayersHard.Remove(item);
                    }
                }
                */

                deleteData = true;

                foreach (var item in mjb.PlayersHard)
                {
                    if (item.showed == false)
                    {
                        deleteData = false;
                        break;
                    }
                }

                if (deleteData)
                {
                    mjb.PlayersHard.Clear();
                    mjb.HardLevel = "";
                }

                break;
            default:
                mjb.Players.Clear();
                break;
        }

        SaveToJson(mjb, jsonPath);
        UploadInServer();
    }

    // Procedure starting the coroutine for downloading the JSON file from the FTP server
    public void StartDownload()
    {
        StartCoroutine(DownloadJSON());
    }

    // Downloads the JSON file from the FTP server
    private IEnumerator DownloadJSON()
    {
        // Checking for internet connection
        UnityWebRequest www = UnityWebRequest.Get("http://google.com");
        yield return www.SendWebRequest();

        bool hasInternet = false;

        if (www.isNetworkError || www.isHttpError || www == null)
        {
            print("No Internet connection!");
        }
        else
        {
            hasInternet = true;
        }

        if (hasInternet)
        {
            // ftpClient.download(@"/Test.json", jsonPath);
            //downloadWithFTP("ftp://ftp.snejankagd.com//Test.json", jsonPath, "duhov@snejankagd.com", "123123");

            StartCoroutine(DownloadDataFromTheSerer());

            jsonUpdated = true;
        }
    }

    // If at the moment the JSON file isn't being updated and uploading to the FTP server, this coroutine is downloading the JSON from the FTP every 1.5 secs
    private IEnumerator KeepJSONUpdated()
    {
        while (!uploadinJSON)
        {
            while (!jsonUpdated)
            {
                StartCoroutine(DownloadJSON());

                if (uploadinJSON)
                {
                    break;
                }

                yield return new WaitForSeconds(1.75f);
            }

            jsonUpdated = false;
            print("Json Updated");
        }
    }

    public static MJ ReadJson(string path)
    {
        var setting = new JsonSerializerSettings();

        setting.Formatting = Newtonsoft.Json.Formatting.Indented;
        setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        setting.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;


        var fileContent = File.ReadAllText(path);
        MJ mj = JsonConvert.DeserializeObject<MJ>(fileContent);

        return mj;
    }

    public static StatObject ReadLocalDBJson(string path)
    {
        var setting = new JsonSerializerSettings();

        setting.Formatting = Newtonsoft.Json.Formatting.Indented;
        setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        setting.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;

        var fileContent = File.ReadAllText(path);
        StatObject mj = JsonConvert.DeserializeObject<StatObject>(fileContent);

        return mj;
    }

    public static void SaveToJson(MJ mjb, string path)
    {
        JsonSerializer serializer = new JsonSerializer();
        serializer.Converters.Add(new JavaScriptDateTimeConverter());
        serializer.NullValueHandling = NullValueHandling.Ignore;

        using (StreamWriter sw = new StreamWriter(path))
        using (JsonWriter writer = new JsonTextWriter(sw))
        {
            serializer.Serialize(writer, mjb);
            // {"ExpiryDate":new Date(1230375600000),"Price":0}
        }
    }

    public static void SaveToJson(StatObject mjb, string path)
    {
        JsonSerializer serializer = new JsonSerializer();
        serializer.Converters.Add(new JavaScriptDateTimeConverter());
        serializer.NullValueHandling = NullValueHandling.Ignore;

        using (StreamWriter sw = new StreamWriter(path))
        using (JsonWriter writer = new JsonTextWriter(sw))
        {
            serializer.Serialize(writer, mjb);
            // {"ExpiryDate":new Date(1230375600000),"Price":0}
        }
    }

    public void UploadInServer()
    {
        uploadinJSON = true;
        StartCoroutine(UploadDataInTheServer());
        uploadinJSON = false;
    }

    public void ClearDataInTheServer()
    {
        uploadinJSON = true;

        MJ mjb = ReadJson(jsonPath);
        mjb.Players.Clear();
        mjb.PlayersHard.Clear();
        mjb.EasyLevel = "";
        mjb.HardLevel = "";
        mjb.CurrentlyPlaying = false;

        SaveToJson(mjb, jsonPath);

        UploadInServer();
        print("Done?");

        uploadinJSON = false;
    }

    private async void downloadWithFTP(string ftpUrl, string savePath = "", string userName = "", string password = "")
    {

        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(ftpUrl));
        //request.Proxy = null;

        request.UsePassive = true;
        request.UseBinary = true;
        request.KeepAlive = true;

        //If username or password is NOT null then use Credential
        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
        {
            request.Credentials = new NetworkCredential(userName, password);
        }

        request.Method = WebRequestMethods.Ftp.DownloadFile;

        //If savePath is NOT null, we want to save the file to path
        //If path is null, we just want to return the file as array
        if (!string.IsNullOrEmpty(savePath))
        {
            downloadAndSave(request.GetResponse(), savePath);
        }
        else
        {
            downloadAsbyteArray(request.GetResponse());
        }
    }

    byte[] downloadAsbyteArray(WebResponse request)
    {
        using (Stream input = request.GetResponseStream())
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while (input.CanRead && (read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }

    void downloadAndSave(WebResponse request, string savePath)
    {
        Stream reader = request.GetResponseStream();

        //Create Directory if it does not exist
        if (!Directory.Exists(Path.GetDirectoryName(savePath)))
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
        }

        FileStream fileStream = new FileStream(savePath, FileMode.Create);


        int bytesRead = 0;
        byte[] buffer = new byte[2048];

        while (true)
        {
            bytesRead = reader.Read(buffer, 0, buffer.Length);

            if (bytesRead == 0)
                break;

            fileStream.Write(buffer, 0, bytesRead);
        }
        fileStream.Close();
    }

    private async void UpdateJSONUsedInTask()
    {
        try
        {
            string filename = Path.GetFileName(jsonPath);

            FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create("ftp://ftp.snejankagd.com//Test.json");
            ftp.Credentials = new NetworkCredential("duhov@snejankagd.com", "123123");

            ftp.KeepAlive = true;
            ftp.UseBinary = true;
            ftp.Method = WebRequestMethods.Ftp.UploadFile;

            FileStream fs = File.OpenRead(jsonPath);
            byte[] buffer = new byte[fs.Length];
            await fs.ReadAsync(buffer, 0, buffer.Length);
            await fs.FlushAsync();
            fs.Close();

            Stream ftpstream = ftp.GetRequestStream();
            await ftpstream.WriteAsync(buffer, 0, buffer.Length);
            await ftpstream.FlushAsync();
            ftpstream.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public IEnumerator DownloadDataFromTheSerer()
    {
        Task t = Task.Run(() => downloadWithFTP("ftp://ftp.snejankagd.com//Test.json", jsonPath, "duhov@snejankagd.com", "123123"));

        while (!t.IsCompleted)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator UploadDataInTheServer()
    {
        Task t = Task.Run(() => UpdateJSONUsedInTask());

        while (!t.IsCompleted)
        {
            yield return new WaitForEndOfFrame();
        }
    }

}
