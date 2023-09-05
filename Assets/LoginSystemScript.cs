using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class LoginSystemScript : MonoBehaviour
{
    [Header("Login TMP InputFields")]
    [SerializeField] TMP_InputField discordUsername; // discord username
    [SerializeField] TMP_InputField address; // address
    [SerializeField] GameObject loginButton; // login button

    public void Register()
    {
        if (!string.IsNullOrEmpty(discordUsername.text)) // if discord username is not empty
        {
            var request = new RegisterPlayFabUserRequest() // make a request to register user
            {
                DisplayName = (discordUsername.text + GenerateUserNameNumbers()), // display name in players list
                Username = (discordUsername.text + GenerateUserNameNumbers()), // username in profile explorer
                Password = GeneratePassword(),

                Email = string.IsNullOrEmpty(address.text) ? (address.text + GeneratePassword()) : (address.text + GeneratePassword())

            };

            PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
            Debug.Log("Generated Password: " + GeneratePassword());
        }
    }

    public static string GeneratePassword() {
        string[] domains = { "gmail", "mail"};
        System.Random random = new System.Random();
        int firstIndex = random.Next(domains.Length);
        int secondIndex = random.Next(1000, 10000);

        return $"fix@{domains[firstIndex]}{secondIndex}.com";
    }

    public static string GenerateUserNameNumbers() {
        System.Random random = new System.Random();
        int usernameNumbers = random.Next(1000, 10000);

        return usernameNumbers.ToString();
    }

    public void OnRegisterSuccess(RegisterPlayFabUserResult result) {
        Debug.Log("Success registering user: " + result.PlayFabId);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnError(PlayFabError error) {
        Debug.Log("Error registering user: " + error.GenerateErrorReport());
        Text buttonText = loginButton.GetComponentInChildren<Text>();
        buttonText.text = "Discord Error";
    }
}
