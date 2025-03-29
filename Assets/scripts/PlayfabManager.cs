using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using System;
using Facebook.Unity;
using PlayFab.PfEditor.EditorModels;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager instance;
    public TMP_InputField email_register, email_Login,email_passowrdRecovery, password_register, password_login;
    public TextMeshProUGUI MessageText;
    private static string tittleID = "4117F";
    public PlayFabSharedSettings settings;
    public Transform leaderboardContainer;
    public GameObject leaderboardEntryPrefab;
    public GameObject leaderboardPanel, Loginpanel, registerPanel;
    public string PlayerID = string.Empty;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    void Start()
    {
        //CustomIDLogin();
        //settings.TitleId = tittleID;

        if (FB.IsInitialized)
            return;
        FB.Init(() => FB.ActivateApp());
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

    private void OnError(PlayFab.PlayFabError error)
    {
        Debug.Log(error.ToString());
        MessageText.text = error.ErrorMessage;
    }

    private void OnSucessLogin(PlayFab.ClientModels.LoginResult result)
    {
        Debug.Log("LoginSuccessfull");
        MessageText.text = "Player logged in sucessfully";
        Loginpanel.SetActive(false);    
        UIHandler.Instance.MainMenuPanel.SetActive(true);
        PlayerID = result.PlayFabId;
    }

    public void RegisterViaMail()
    {
        if (password_register.text.Length < 6)
        {
            MessageText.text = "Password too short";
        }

        var request = new RegisterPlayFabUserRequest
        {
            Email = email_register.text,
            Password = password_register.text,
            RequireBothUsernameAndEmail = false,
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        MessageText.text = "Player Registered Successfully";
        registerPanel.SetActive(false);
        UIHandler.Instance.MainMenuPanel.SetActive(true);
    }

    public void LoginViaMail()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = email_Login.text,
            Password = password_login.text,
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnSucessLogin, OnError);
    }

    public void ResetPasswordButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = email_passowrdRecovery.text,
            TitleId = tittleID,
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnResetPasswordSuccess, OnError);
    }

    private void OnResetPasswordSuccess(SendAccountRecoveryEmailResult result)
    {
        MessageText.text = "Email Sent";
    }

    public void SendLeaderboard(int score) 
    {
        var request = new UpdatePlayerStatisticsRequest 
        {
            Statistics = new List<StatisticUpdate> 
            {
                new StatisticUpdate 
                {
                    StatisticName = "CoinsCollected",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successful leaderboard sent");
    }

    public void GetLeaderboard() {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "CoinsCollected",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
            // Clear previous entries
            foreach (Transform child in leaderboardContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (var item in result.Leaderboard)
            {
                // Instantiate the prefab
                GameObject entry = Instantiate(leaderboardEntryPrefab, leaderboardContainer);

                // Get TextMeshProUGUI components
                TextMeshProUGUI[] texts = entry.GetComponentsInChildren<TextMeshProUGUI>();

                // Assign values
                texts[0].text = (item.Position + 1).ToString();
                texts[1].text = item.PlayFabId;
                texts[2].text = item.StatValue.ToString();

                if(item.PlayFabId == PlayerID)
                {
                    entry.GetComponent<Image>().color = Color.red;
                }
            }

            UIHandler.Instance.MainMenuPanel.SetActive(false);
            leaderboardPanel.SetActive(true);
    }

    public void GetLeaderboardAroundPlayer()
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = "CoinsCollected",
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardGetAroundPlayer, OnError);
    }

    private void OnLeaderboardGetAroundPlayer(GetLeaderboardAroundPlayerResult result)
    {
        // Clear previous entries
        foreach (Transform child in leaderboardContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in result.Leaderboard)
        {
            Debug.Log(item);
            // Instantiate the prefab
            GameObject entry = Instantiate(leaderboardEntryPrefab, leaderboardContainer);

            // Get TextMeshProUGUI components
            TextMeshProUGUI[] texts = entry.GetComponentsInChildren<TextMeshProUGUI>();

            // Assign values
            texts[0].text = "Rank: " + (item.Position + 1);
            texts[1].text = "ID: " + item.PlayFabId;
            texts[2].text = "Score: " + item.StatValue;

            if (item.PlayFabId == PlayerID)
            {
                entry.GetComponent<Image>().color = Color.red;
            }

            UIHandler.Instance.MainMenuPanel.SetActive(false);
            leaderboardPanel.SetActive(true);
        }
    }

    // Player data
    public void GetAppearance()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataRecieved, OnError);
    }

    void OnDataRecieved(GetUserDataResult result)
    {
        Debug.Log("Recieved user data!");
        if (result.Data != null && result.Data.ContainsKey("Hat") && result.Data.ContainsKey("Skin") && result.Data.ContainsKey("Beard"))
        {
            //characterEditor.SetAppearance(result.Data["Hat"].Value, result.Data["Skin"].Value, result.Data["Beard"].Value);
        }
        else
        {
            Debug.Log("Player data not complete!");
        }
    }

    public void SaveAppearance()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string> {
            //{"Hat", characterEditor.Hat},
            //{"Skin", characterEditor.Skin},
            //{"Beard", characterEditor.Beard}
        }
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSend, OnError);
    }

    private void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("data sent succesfully");
    }

    public void LoginWithFacebook()
    {
        FB.LogInWithReadPermissions(new List<string> { "public_profile", "email" }, result =>
        {
            if (FB.IsLoggedIn)
            {
                var accessToken = AccessToken.CurrentAccessToken.TokenString;
                Debug.Log("Facebook Login Successful! Token: " + accessToken);
                LoginWithPlayfabFacebook(accessToken);
            }
            else
            {
                Debug.Log("Facebook Login Failed!");
            }
        });
    }
    public void LoginWithPlayfabFacebook(string accessToken)
    {

        var request = new LoginWithFacebookRequest
        {
            TitleId = tittleID,
            AccessToken = accessToken,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithFacebook(request, OnSucessLogin, OnError);
    }
}


