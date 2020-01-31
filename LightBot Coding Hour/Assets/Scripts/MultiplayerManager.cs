using FTPClient;
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

    FTP ftpClient;

    public Transform matchmakingPanel;

    private int secs;
    private int mins;

    private string battleScene = "Level 6";

    public int bSecs = 90;

    public Text TimeText;
    public Text FoundGame;

    public GameObject resultPanel;

    public bool TestMultiplayer = false;

    private void Awake()
    {
        jsonPath = Path.Combine(Application.streamingAssetsPath, filename);
        ftpClient = new FTP(@"ftp://ftp.snejankagd.com/", "duhov@snejankagd.com", "123123");

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
    }

    // Procedure to sign the user that he wished to play multiplayer
    public void SignInForMultiplayer()
    {
        // trigger to stop the repeating downloading of the JSON file from the FTP server and work with the latest one
        uploadinJSON = true;

        if (signedIn)
        {
            signedIn = false;

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
            ftpClient.upload(@"Test.json", jsonPath);

            // Triggering again the auto updation of the JSON file from the FTP server
            uploadinJSON = false;
            StartCoroutine(KeepJSONUpdated());

            matchmakingPanel.gameObject.SetActive(false);

            print("succesfully signed off");

            return;
        }

        // Reading the JSON file
        MJ mjb = ReadJson(jsonPath);

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
        ftpClient.upload(@"Test.json", jsonPath);

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
            yield return new WaitForSeconds(0.25f);

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

            TimeText.text = DisplaySeconds(secs);
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
        SaveToJson(mjb, jsonPath);
        UploadInServer();
        showWinner = true;

        SceneManager.LoadScene("MultiplayerMenu");
    }

    public void ShowWinner()
    {
        var allMM = FindObjectsOfType<MultiplayerManager>();
        print(allMM.Length);

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
        int max = -1;

        switch (PlayerPrefs.GetString("diff"))
        {
            case "easy":
                foreach (var item in mjb.Players)
                {
                    if (item.AmountOfBlueTilesEnlightened > max)
                    {
                        max = item.AmountOfBlueTilesEnlightened;
                        winner = item.Nickname;
                    }
                }
                break;
            case "hard":
                foreach (var item in mjb.PlayersHard)
                {
                    if (item.AmountOfBlueTilesEnlightened > max)
                    {
                        max = item.AmountOfBlueTilesEnlightened;
                        winner = item.Nickname;
                    }
                }
                break;
            default:
                break;
        }

        PlayerProfile winnerProfile = new PlayerProfile();
        PlayerProfile secondPlaceProfile = new PlayerProfile();

        switch (PlayerPrefs.GetString("diff"))
        {
            case "easy":
                foreach (var item in mjb.Players)
                {
                    if (mjb.Players.Count == 2)
                    {
                        if (item.Nickname == winner)
                        {
                            winnerProfile = item;
                        }
                        else
                        {
                            secondPlaceProfile = item;
                        }
                    }
                }
                break;
            case "hard":
                foreach (var item in mjb.PlayersHard)
                {
                    if (mjb.PlayersHard.Count == 2)
                    {
                        if (item.Nickname == winner)
                        {
                            winnerProfile = item;
                        }
                        else
                        {
                            secondPlaceProfile = item;
                        }
                    }
                }
                break;
            default:
                break;
        }

        sm.winnerNameLabel.text = "Winner:/n" + winner;
        sm.place1.sprite = sm.ReturnAvatar(winnerProfile);
        sm.winnerData.text = string.Format("Amount of blue tiles enlighted: {0}\nAmount of orders used: {1}\nNeeded time to solve: {2} seconds", winnerProfile.AmountOfBlueTilesEnlightened, winnerProfile.AmountOfOrders, winnerProfile.timeInSeconds);

        sm.secondPlaceNameLabel.text = "Second place:\n" + secondPlaceProfile.Nickname;
        sm.place2.sprite = sm.ReturnAvatar(secondPlaceProfile);
        sm.secondPlaceData.text = string.Format("Amount of blue tiles enlighted: {0}\nAmount of orders used: {1}\nNeeded time to solve: {2} seconds", secondPlaceProfile.AmountOfBlueTilesEnlightened, secondPlaceProfile.AmountOfOrders, secondPlaceProfile.timeInSeconds);

        showWinner = false;
    }

    public void Ok()
    {
        resultPanel.gameObject.SetActive(false);
        MJ mjb = ReadJson(jsonPath);

        switch (PlayerPrefs.GetString("diff"))
        {
            case "easy":
                mjb.Players.Clear();
                mjb.EasyLevel = "";
                break;
            case "hard":
                mjb.PlayersHard.Clear();
                mjb.HardLevel = "";
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
            ftpClient.download(@"/Test.json", jsonPath);

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

                yield return new WaitForSeconds(1.25f);
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

    public void UploadInServer()
    {
        ftpClient.upload(@"Test.json", jsonPath);
    }

    public void ClearDataInTheServer()
    {
        uploadinJSON = true;

        MJ mjb = ReadJson(jsonPath);
        mjb.Players.Clear();
        mjb.PlayersHard.Clear();
        mjb.EasyLevel = "";
        mjb.HardLevel = "";

        SaveToJson(mjb, jsonPath);

        UploadInServer();

        uploadinJSON = false;
    }
}
