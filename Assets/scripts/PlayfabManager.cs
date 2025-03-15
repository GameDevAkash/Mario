using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
    private static PlayfabManager instance;
    // Start is called before the first frame update

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    void Start()
    {
        CustomIDLogin();
    }

    public void CustomIDLogin()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnSucessLogin, OnError);
    }

    private void OnError(PlayFabError error)
    {
        Debug.Log(error.ToString());
    }

    private void OnSucessLogin(LoginResult result)
    {
        Debug.Log("LoginSuccessfull");
    }
}
