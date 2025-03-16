using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayfabManager : MonoBehaviour
{
    private static PlayfabManager instance;
    public TMP_InputField email_register, email_Login,email_passowrdRecovery, password_register, password_login;
    public TextMeshProUGUI MessageText;
    private static string tittleID = "4117F";
    public PlayFabSharedSettings settings;

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
        settings.TitleId = tittleID;
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
        MessageText.text = error.ErrorMessage;
    }

    private void OnSucessLogin(LoginResult result)
    {
        Debug.Log("LoginSuccessfull");
        MessageText.text = "Player logged in sucessfully";
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
}
