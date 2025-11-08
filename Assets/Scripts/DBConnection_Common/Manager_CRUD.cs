using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

[Serializable]
public class QuizItem
{
    public int quiz_item_id;
    public int category_id;
    public string question_text;
    public string correct_answer;
    public string answer_2;
    public string answer_3;
    public string explanation;

    public QuizItem(int id, int category, string question, string correctAnswer, string answer2, string answer3, string explanation)
    {
        this.quiz_item_id = id;
        this.category_id = category;
        this.question_text = question;
        this.correct_answer = correctAnswer;
        this.answer_2 = answer2;
        this.answer_3 = answer3;
        this.explanation = explanation;
    }
}

[Serializable]
public class QuizItemResponse
{
    public string error;
    public string message;
    public string details;
}

[Serializable]
public class QuizItemListWrapper
{
    public QuizItem[] quizItems;

    public QuizItemListWrapper(QuizItem[] quizItems)
    {
        this.quizItems = quizItems;
    }

    public List<QuizItem> ToList()
    {
        if (quizItems == null)
            return new List<QuizItem>();
        return new List<QuizItem>(quizItems);
    }
}

public class Manager_CRUD : MonoBehaviour
{
    [Header("Server Configuration")]
    public string baseUrl = "http://localhost:8080/api/quiz_items";

    [Header("Events")]
    public UnityAction<List<QuizItem>> OnQuizItemsLoaded;
    public UnityAction<QuizItem> OnQuizItemsCreated;
    public UnityAction<QuizItem> OnQuizItemsUpdated;
    public UnityAction<string> OnQuizItemsDeleted;
    public UnityAction<string> OnError;

    [Header("Debug")]
    public bool enableDebugLogs = true;

    private void Start()
    {
        // Test connection on start
        StartCoroutine(TestConnection());
    }

    #region Public Methods

    /// <summary>
    /// Carica tutti i quiz item specifici di una categoria per ID
    /// </summary>
    public void LoadQuizItemsByCategory(int categoryId)
    {
        StartCoroutine(GetQuizItemsByCategoryCoroutine(categoryId));
    }

    /// <summary>
    /// Test di connessione manuale
    /// </summary>
    [ContextMenu("Test Connection")]
    public void TestConnectionManual()
    {
        StartCoroutine(TestConnection());
    }

    #endregion

    #region Coroutines

    private IEnumerator TestConnection()
    {
        if (enableDebugLogs)
            Debug.Log("Testing connection to server...");

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

    private IEnumerator GetQuizItemsByCategoryCoroutine(int categoryId)
    {
        if (enableDebugLogs)
            Debug.Log($"Loading quiz items for category id: {categoryId}...");

        using (UnityWebRequest request = UnityWebRequest.Get($"{baseUrl}/{categoryId}"))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            print("entro in categoryquizitemscouroutine");
            if (request.result == UnityWebRequest.Result.Success)
            {
                try
                {
                    string jsonResponse = request.downloadHandler.text;

                    if (enableDebugLogs)
                        Debug.Log($"Raw JSON response: {jsonResponse}");

                    // Controlla se la risposta � vuota o null
                    if (string.IsNullOrEmpty(jsonResponse) || jsonResponse.Trim() == "null")
                    {
                        if (enableDebugLogs)
                            Debug.Log("Empty response, returning empty quiz item list");
                        OnQuizItemsLoaded?.Invoke(new List<QuizItem>());
                        yield break;
                    }

                    // Verifica che sia un array JSON valido
                    if (!jsonResponse.Trim().StartsWith("["))
                    {
                        if (enableDebugLogs)
                            Debug.LogWarning($"Response is not an array: {jsonResponse}");
                        OnQuizItemsLoaded?.Invoke(new List<QuizItem>());
                        yield break;
                    }

                    // JsonUtility non supporta direttamente array/liste, quindi wrappiamo
                    string wrappedJson = "{\"quizItems\":" + jsonResponse + "}";
                    QuizItemListWrapper wrapper = JsonUtility.FromJson<QuizItemListWrapper>(wrappedJson);

                    // Verifica che il wrapper e l'array non siano null
                    List<QuizItem> quizItems = new List<QuizItem>();
                    if (wrapper != null && wrapper.quizItems != null)
                    {
                        quizItems = wrapper.ToList();
                    }

                    if (enableDebugLogs)
                    {
                        Debug.Log($"Loaded {quizItems.Count} quiz items for category id: {categoryId}");
                        if (quizItems.Count == 0 && enableDebugLogs)
                            Debug.Log($"No quiz items found for category {categoryId}");
                    }

                    OnQuizItemsLoaded?.Invoke(quizItems);
                }
                catch (Exception e)
                {
                    string error = $"Error parsing quiz items: {e.Message}";
                    if (enableDebugLogs)
                        Debug.LogError(error);
                    OnError?.Invoke(error);
                }
            }
            else
            {
                HandleRequestError(request, $"loading quiz items for category id: {categoryId}");
            }
        }
    }

    #endregion

    #region Helper Methods

    private void HandleRequestError(UnityWebRequest request, string operation)
    {
        string errorMessage = $"Error {request.responseCode}: {request.error}";

        if (request.downloadHandler != null && !string.IsNullOrEmpty(request.downloadHandler.text))
        {
            try
            {
                QuizItemResponse errorResponse = JsonUtility.FromJson<QuizItemResponse>(request.downloadHandler.text);
                if (!string.IsNullOrEmpty(errorResponse.error))
                {
                    errorMessage += $" - {errorResponse.error}";
                }
            }
            catch
            {
                errorMessage += $" - Raw response: {request.downloadHandler.text}";
            }
        }

        if (enableDebugLogs)
            Debug.LogError($"{errorMessage}");

        OnError?.Invoke(errorMessage);
    }

   internal void LoadCategoryQuizItems()
   {
      throw new NotImplementedException();
   }

   #endregion
}