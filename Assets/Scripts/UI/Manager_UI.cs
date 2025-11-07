using System.Collections.Generic;
using UnityEngine;

public class Manager_UI : MonoBehaviour
{
    private static Manager_UI _instance;

    public static Manager_UI Instance { get { return _instance; } }

    [SerializeField] private Transform _contentParent;
    [SerializeField] private GameObject _quizPrefab;
    [SerializeField] private Manager_CRUD _managerCrud;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        _managerCrud.OnQuizItemsLoaded += OnQuizItemsLoaded;
    }

    private void OnDisable()
    {
        _managerCrud.OnQuizItemsLoaded -= OnQuizItemsLoaded;
    }

    private void OnQuizItemsLoaded(List<QuizItem> quizItems)
    {
        Debug.Log($"UI: ricevute {quizItems.Count} quiz items dal server");

        foreach (Transform child in _contentParent)
            Destroy(child.gameObject);

        foreach (QuizItem quizItem in quizItems)
        {
            GameObject newQuizItemObj = Instantiate(_quizPrefab, _contentParent);
            QuizItem_UI quizItemsUI = newQuizItemObj.GetComponent<QuizItem_UI>();
            quizItemsUI.Setup(quizItem);
        }
    }
}
