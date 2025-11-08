using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[Serializable]
public class User
{
    public int id;
    public string nickname;
    public string password;
    public Certificates certificates;

    public User(string nickname, string password)
    {
        this.nickname = nickname;
        this.password = password;
    }

    public User(string nickname, string password, Certificates certificates)
    {
        this.nickname = nickname;
        this.password = password;
        this.certificates = certificates;
    }
}

[Serializable]
public class UserResponse
{
    public string error;
    public string message;
    public string details;
}

//Wrapper

public class Manager_User : MonoBehaviour
{
    [Header("Server Configuration")]
    public string baseUrl = "http://localhost:8080/api/users";

    [Header("Events")]
    public UnityAction<User> OnUserLoaded;
    public UnityAction<string> OnError;

    [Header("Debug")]
    public bool enableDebugLogs = true;

    private void Start()
    {
        StartCoroutine(TestConnection());
    }

    #region Public Methods

    [ContextMenu("Test Connection")]
    public void TestConnectionManual()
    {
        StartCoroutine(TestConnection());
    }

    public void LoadUserByNicknameAndPassword(string nickname, string password)
    {
        User user = new User(nickname, password);
        StartCoroutine(GetUserByNicknameAndPasswordCoroutine(user));
    }

    #endregion

    #region Coroutines

    private IEnumerator TestConnection()
    {
        if (enableDebugLogs)
            Debug.Log("Testing connection to server from Manager_Users");

        using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/test"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                if (enableDebugLogs)
                    Debug.Log("Connection to server successful!");
            }
            else
            {
                string error = $"Connection failed: {request.error} on {baseUrl}";
                if (enableDebugLogs)
                    Debug.LogError(error);
                OnError?.Invoke(error);
            }
        }
    }

    private IEnumerator GetUserByNicknameAndPasswordCoroutine(User user)
    {
        if (enableDebugLogs)
            Debug.Log($"Getting user with nickname: {user.nickname}");

        string jsonBody = JsonUtility.ToJson(user);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);

        using (UnityWebRequest request = new UnityWebRequest($"{baseUrl}/login", "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);  //needed to send the JSON body in a POST request
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    string jsonResponse = request.downloadHandler.text;
                    User userFound = JsonUtility.FromJson<User>(jsonResponse);

                    if (enableDebugLogs)
                        Debug.Log($"Loaded user: {userFound.nickname}");

                    // Per una singola task, la passiamo in una lista
                    OnUserLoaded?.Invoke(new User(userFound.nickname, userFound.password, userFound.certificates));
                }
                catch (Exception e)
                {
                    string error = $"Error parsing user: {e.Message}";
                    if (enableDebugLogs)
                        Debug.LogError(error);
                    OnError?.Invoke(error);
                }
            }
            else
            {
                ConnectionHelper.HandleRequestError(request, $"Loading user with nickname: {user.nickname}", enableDebugLogs, OnError);
            }
        }
    }

    #endregion
}