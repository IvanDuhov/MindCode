using FTPClient;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;

public class MultiplayerManager : MonoBehaviour
{
    // public string MJpath = Application.streamingAssetsPath + "/" + "Test.json";

    public const string filename = "Test2.json";
    public string streamingAssets = "";

    private void Awake()
    {
        streamingAssets = Path.Combine(Application.streamingAssetsPath, filename);
    }

    private void Start()
    {
        StartDownload();
    }

    public void ReadJson()
    {
        MJ mjb = ReadFromDataPersistentPath(streamingAssets);
        mjb.Players[0].Nickname = "Pesho";
        mjb.Players[0].AmountOfBlueTilesEnlightened = 10;
        mjb.Players[0].AmountOfOrders = 12;

        SaveToJson(mjb, streamingAssets);
        print(mjb.NumberOfPeopleWillingToPlay);
    }

    public void StartDownload()
    {
        StartCoroutine(DownloadGlobalDataBase());
    }

    private IEnumerator DownloadGlobalDataBase()
    {
        // Checking for internet connection
        UnityWebRequest www = UnityWebRequest.Get("http://google.com");
        yield return www.SendWebRequest();

        bool hasInternet = false;

        if (www.isNetworkError || www.isHttpError || www == null)
        {
        }
        else
        {
            hasInternet = true;
        }

        if (hasInternet)
        {
            FTP ftpClient = new FTP(@"ftp://ftp.snejankagd.com/", "duhov@snejankagd.com", "123123");
            ftpClient.download(@"/Test.json", streamingAssets);
            print("GData downloaded!");
        }
    }

    public static MJ ReadFromDataPersistentPath(string path)
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




        /*
        var settings = new JsonSerializerSettings();

        setting.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;

        string dataAsJson = JsonUtility.ToJson(mjb, true); //true=pretty
        var asd = JsonSerializer.Create(settings);

            
        File.WriteAllText(path, dataAsJson);

        print("Data Saved to Json");*/
    }
}
