using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class UserApiClient : MonoBehaviour
{
    public WebClient webClient;
    public static UserApiClient Singleton;

    //private void Awake()
    //{
    //    if (Singleton == null)
    //    {
    //        Singleton = this;
    //        DontDestroyOnLoad(this.gameObject);
    //    }
    //    else
    //    {
    //        Destroy(gameObject); // Prevent duplicate instances
    //        Debug.Log("Destroyed duplicate ExampleApp instance");
    //    }
    //}
    public async Awaitable<IWebRequestReponse> Register(User user)
    {
        string route = "/account/register";
        string data = JsonUtility.ToJson(user);

        return await webClient.SendPostRequest(route, data);
    }

    public async Awaitable<IWebRequestReponse> Login(User user)
    {
        string json = JsonUtility.ToJson(user);
        var response = await webClient.SendPostRequest("/account/login", json);

        //switch(response)
        //{
        //    case WebRequestData<string> data:
        //        Debug.Log("Response data raw: " + data.Data);
        //        string token = JsonHelper.ExtractToken(data.Data);
        //        webClient.SetToken(token);
        //        return new WebRequestData<string>("Succes");
        //    default:
        //        return response;
        //}


        if (response is WebRequestData<string> tokenResponse)
        {
            Debug.Log("Login successful, saving token: " + tokenResponse.Data);

            string token = JsonHelper.ExtractToken(tokenResponse.Data);
            webClient.SetToken(token);
            //PlayerPrefs.SetString("AuthToken", tokenResponse.Data.accessToken); // Optioneel: token opslaan
            //PlayerPrefs.Save();

            return new WebRequestData<string>("Success");
        }

        return response;
    }


    private IWebRequestReponse ProcessLoginResponse(IWebRequestReponse webRequestResponse)
    {
        switch (webRequestResponse)
        {
            case WebRequestData<string> data:
                Debug.Log("Response data raw: " + data.Data);
                string token = JsonHelper.ExtractToken(data.Data);
                webClient.SetToken(token);
                return new WebRequestData<string>("Succes");
            default:
                return webRequestResponse;
        }
    }

}

