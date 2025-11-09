using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public static class ConnectionHelper
{
    public static void HandleRequestError(UnityWebRequest request, string operation, bool enableDebugLogs, UnityAction<string> onError)
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

        onError?.Invoke(errorMessage);
    }
}
