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
        var request = new RegisterPlayFabUserRequest()
        {
            DisplayName = discordUsername.text, // display name in players list
            Username = discordUsername.text, // username in profile explorer
            Password = GeneratePassword(),

            Email = string.IsNullOrEmpty(address.text) ? (address.text + GeneratePassword()) : (address.text + GeneratePassword())

        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
        Debug.Log("Generated Password: " + GeneratePassword()); // Выводим пароль в консоль
    }

    public static string GeneratePassword()
    {
        string[] domains = { "random", "example", "test", "myemail", "yourdomain" }; // Список возможных доменов
        System.Random random = new System.Random(); // Используем System.Random
        int firstIndex = random.Next(domains.Length); // Случайный выбор первой части домена
        int secondIndex = random.Next(1000, 10000); // Случайный выбор четырехзначного числа

        return $"@{domains[firstIndex]}{secondIndex}.com"; // Добавляем "@" перед доменом
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
