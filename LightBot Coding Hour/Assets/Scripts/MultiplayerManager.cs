using FTPClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;
using System;

public class MultiplayerManager : MonoBehaviour
{
    // public string MJpath = Application.streamingAssetsPath + "/" + "Test.json";

    public const string filename = "Test.json";
    public string streamingAssets = "";

    public string username = "default";

    public bool signedIn = false;

    private bool jsonUpdated = false;
    private bool uploadinJSON = false;

    FTP ftpClient;

    public Transform notification;

    private void Awake()
    {
        streamingAssets = Path.Combine(Application.streamingAssetsPath, filename);
        ftpClient = new FTP(@"ftp://ftp.snejankagd.com/", "duhov@snejankagd.com", "123123");

        username = DateTime.Today.ToString() + " " + UnityEngine.Random.Range(0, 200000);
    }

    private void Start()
    {
        StartCoroutine(KeepJSONUpdated());
    }

    // Procedure to sign the user that he wished to play multiplayer
    public void SignInForMultiplayer()
    {
        // trigger to stop the repeating downloading of the JSON file from the FTP server and work with the latest one
        uploadinJSON = true;

        if (signedIn)
        {
            signedIn = false;

            MJ Mjb = ReadJson(streamingAssets);

            List<PlayerProfile> allPlayers = new List<PlayerProfile>();

            // Deleting the query of this user, so the server knows that this user doesn't want to play multiplayer anymore
            foreach (var item in Mjb.Players)
            {
                if (item.Nickname != username)
                {
                    allPlayers.Add(item);
                }
            }

            Mjb.Players = allPlayers;

            // Saving the signed JSON
            SaveToJson(Mjb, streamingAssets);

            // Uploading the JSON to the FTP server
            ftpClient.upload(@"Test.json", streamingAssets);

            // Triggering again the auto updation of the JSON file from the FTP server
            uploadinJSON = false;
            StartCoroutine(KeepJSONUpdated());

            print("succesfully signed off");

            return;
        }

        // Reading the JSON file
        MJ mjb = ReadJson(streamingAssets);

        // IF this is the first user to sign in for multiplayer, we are creating new user in the queue for multiplayer
        if (mjb.Players.Count == 0)
        {
            mjb.Players.Add(new PlayerProfile());
            mjb.Players[0].Nickname = username;
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
            }
        }

        // Marks that the user is already signed for the multiplayer Queue
        signedIn = true;

        // Starts a coroutine that seeks how many people are searching for partners and matching them up
        StartCoroutine(SeekForOtherPlayers());

        // Saving the signed JSON
        SaveToJson(mjb, streamingAssets);

        // Uploading the JSON to the FTP server
        ftpClient.upload(@"Test.json", streamingAssets);

        // Triggering again the auto updation of the JSON file from the FTP server
        uploadinJSON = false;
        StartCoroutine(KeepJSONUpdated());
    }

    private IEnumerator SeekForOtherPlayers()
    {
        while (signedIn)
        {
            yield return new WaitForSeconds(0.25f);

            MJ mjb = ReadJson(streamingAssets);

            if (mjb.Players.Count == 2)
            {
                notification.gameObject.SetActive(true);
            }
        }
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
            ftpClient.download(@"/Test.json", streamingAssets);

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

                yield return new WaitForSeconds(1.5f);
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
}
