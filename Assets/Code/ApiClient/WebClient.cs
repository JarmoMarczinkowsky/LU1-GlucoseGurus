using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebClient : MonoBehaviour
{
    public string baseUrl;
    private string token;

    //public static WebClient Singleton;
    public static WebClient instance;

    public void Start()
    {
        Debug.Log("Nieuwe WebClent aangemaakt");
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);


            // Haal token op uit PlayerPrefs
            //if (PlayerPrefs.HasKey("AuthToken"))
            //{
            //    string savedToken = PlayerPrefs.GetString("AuthToken");
            //    SetToken(savedToken);
            //    Debug.Log("Loaded saved token.:" + savedToken);
            //}
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetToken(string token)
    {
        Debug.Log("Received token: " + token);

        this.token = token;
        Debug.Log("Set token to: " + this.token);
    }

    public async Awaitable<IWebRequestReponse> SendGetRequest(string route)
    {
        UnityWebRequest webRequest = CreateWebRequest("GET", route, "");
        return await SendWebRequest(webRequest);
    }
    public async Awaitable<IWebRequestReponse> SendGetRequest(string route, string data)
    {
        UnityWebRequest webRequest = CreateWebRequest("GET", route, data);
        return await SendWebRequest(webRequest);
    }

    public async Awaitable<IWebRequestReponse> SendPostRequest(string route, string data)
    {
        UnityWebRequest webRequest = CreateWebRequest("POST", route, data);
        IWebRequestReponse response = await SendWebRequest(webRequest);
        return response;
    }

    public async Awaitable<IWebRequestReponse> SendPutRequest(string route, string data)
    {
        UnityWebRequest webRequest = CreateWebRequest("PUT", route, data);
        return await SendWebRequest(webRequest);
    }

    public async Awaitable<IWebRequestReponse> SendDeleteRequest(string route)
    {
        UnityWebRequest webRequest = CreateWebRequest("DELETE", route, "");
        return await SendWebRequest(webRequest);
    }

    private UnityWebRequest CreateWebRequest(string type, string route, string data)
    {
        string url = baseUrl + route;
        Debug.Log("Creating " + type + " request to " + url + " with data: " + data);

        data = RemoveIdFromJson(data); // Backend throws error if it receiving empty strings as a GUID value.
        var webRequest = new UnityWebRequest(url, type);
        byte[] dataInBytes = new UTF8Encoding().GetBytes(data);
        webRequest.uploadHandler = new UploadHandlerRaw(dataInBytes);
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        webRequest.SetRequestHeader("Content-Type", "application/json");

        AddToken(webRequest);


        return webRequest;
    }

    private async Awaitable<IWebRequestReponse> SendWebRequest(UnityWebRequest webRequest)
    {
        await webRequest.SendWebRequest();

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.Success:
                string responseData = webRequest.downloadHandler.text;
                Debug.Log("Response: " + responseData);

                //// Converteer JSON naar een object als de request een token bevat
                //if (responseData.Contains("accessToken")) // Simpele check om te zien of het een login-response is
                //{
                //    Debug.Log(responseData);
                //    Token tokenData = JsonUtility.FromJson<Token>(responseData);
                //    return new WebRequestData<Token>(tokenData);
                //}
                return new WebRequestData<string>(responseData);

            default:
                return new WebRequestError(webRequest.error);
        }
    }


    private void AddToken(UnityWebRequest webRequest)
    {
        Debug.Log("Token: " + token);
        webRequest.SetRequestHeader("Authorization", "Bearer " + token);
        Debug.Log("Added token to request: " + token);
    }

    private string RemoveIdFromJson(string json)
    {
        return json.Replace("\"id\":\"\",", "");
    }

}

[Serializable]
public class Token
{
    public string tokenType;
    public string accessToken;
}
