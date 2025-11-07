using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager_Progression : MonoBehaviour
{
    private static Manager_Progression _instance;
    public static Manager_Progression Instance { get { return _instance; } }

    [SerializeField] private Manager_CRUD _managerCrud;
    [SerializeField] private Button _answer1Button, _answer2Button, _answer3Button;
    [SerializeField] private TextMeshProUGUI _questionText, _questionCounterText, _explanationText;

    [SerializeField] private PanelManager _panelManager;
    [Header("UI")]
    [SerializeField] private GameObject finestraWrong, questionsPanel;

    private int _category_id;
    private List<QuizItem> _quizItems = new();
    private int _questionCounter = 1;
    private QuizItem _currentQuizItem;
    private bool isCorrect = true;

    [Header("Debug")]
    public bool enableDebugLogs = true;

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

    /// <summary>
    /// Gets the category_id from the scene
    /// </summary>
    private void Start()
    {
        _panelManager.ShowQuestionPanel();

        var currentSceneName = SceneManager.GetActiveScene().name;
        if (Enum.TryParse(currentSceneName, out ChosenScene chosenScene))
        {
            _category_id = (int)chosenScene;

            if (enableDebugLogs)
                Debug.Log($"Scene: {currentSceneName}, Category ID: {_category_id}");

            _managerCrud.LoadQuizItemsByCategory(_category_id);     //calls OnQuizItemsLoaded event

            _questionCounterText.text = $"Domanda {_questionCounter}/ {_quizItems.Count}";
        }
        else
        {
            if (enableDebugLogs)
                Debug.LogWarning($"The scene '{currentSceneName}' does not exists");
        }
    }

    /// <summary>
    /// Called on OnQuizItemsLoaded event. Populate the _quizItems list and calls RandomizeQuestion()
    /// </summary>
    /// <param name="quizItems">List of all the QuizItems of the chosen category</param>
    private void OnQuizItemsLoaded(List<QuizItem> quizItems)
    {
        if (quizItems == null || quizItems.Count == 0)
        {
            if (enableDebugLogs)
                Debug.LogWarning($"No quiz items found for category {_category_id}");
            return;
        }

        _quizItems = quizItems;
        if (enableDebugLogs)
            Debug.Log($"Loaded {_quizItems.Count} quiz items.");

        RandomizeQuestion();
    }

    /// <summary>
    /// Randomize the question among all the question of the category
    /// </summary>
    private void RandomizeQuestion()
    {
        if (_quizItems.Count == 0)
        {
            if (enableDebugLogs)
                Debug.Log("No more quiz items left");
            return;
        }

        int quizItemIndex = UnityEngine.Random.Range(0, _quizItems.Count);
        QuizItem chosenQuizItem = _quizItems[quizItemIndex];
        _quizItems.RemoveAt(quizItemIndex);
        if (enableDebugLogs)
            Debug.Log($"Quiz item randomly selected => ID: {chosenQuizItem.quiz_item_id}, Correct Answer: {chosenQuizItem.answer_1}");

        _currentQuizItem = chosenQuizItem; // we get the current question to then get the explanation in case the answer is wrong
        _questionText.text = chosenQuizItem.question_text;

        AssingAnswersToButtons(chosenQuizItem);
    }

    /// <summary>
    /// Assigns the answers randomly among the buttons
    /// </summary>
    /// <param name="quizItem"></param>
    private void AssingAnswersToButtons(QuizItem quizItem)
    {
        _answer1Button.onClick.RemoveAllListeners();
        _answer2Button.onClick.RemoveAllListeners();
        _answer3Button.onClick.RemoveAllListeners();

        var randomizedAnswers = RandomizeAnswer(quizItem);
        if (enableDebugLogs)
            Debug.Log($"Answer 1: {randomizedAnswers[0]}, Answer 2: {randomizedAnswers[1]}, Answer 3: {randomizedAnswers[2]}");

        _answer1Button.GetComponentInChildren<TextMeshProUGUI>().text = randomizedAnswers[0];
        _answer2Button.GetComponentInChildren<TextMeshProUGUI>().text = randomizedAnswers[1];
        _answer3Button.GetComponentInChildren<TextMeshProUGUI>().text = randomizedAnswers[2];

        _answer1Button.onClick.AddListener(() => OnAnswerSelected(quizItem, randomizedAnswers[0]));
        _answer2Button.onClick.AddListener(() => OnAnswerSelected(quizItem, randomizedAnswers[1]));
        _answer3Button.onClick.AddListener(() => OnAnswerSelected(quizItem, randomizedAnswers[2]));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="quizItem"></param>
    /// <returns>A list of strings containing the three answers in a random order</returns>
    private List<string> RandomizeAnswer(QuizItem quizItem)
    {
        List<string> answers = new List<string>
        {
            quizItem.answer_1,
            quizItem.answer_2,
            quizItem.answer_3
        }.OrderBy(a => UnityEngine.Random.value).ToList();

        return answers;
    }

    /// <summary>
    /// Compares the selected answer with QuizItem.correct_answers to determine if it is correct
    /// </summary>
    /// <param name="quizItem">The quiz item containing the correct answer</param>
    /// <param name="chosenAnswer">The answer selected by the user</param>
    private void OnAnswerSelected(QuizItem quizItem, string chosenAnswer)
    {

        if (chosenAnswer == quizItem.answer_1)          //change column to 'correct_answer'
        {
            //Add button style in case of wrong answer
            OnCorrectAnswerClicked();
            if (enableDebugLogs)
                Debug.Log($"Correct answer. chosenAnswer: {chosenAnswer}, correct_answer: {quizItem.answer_1}");
        }
        else
        {
            //Add button style in case of wrong answer
            OnWrongAnswerClicked();
            if (enableDebugLogs)
                Debug.Log($"Wrong answer. chosenAnswer: {chosenAnswer}, correct_answer: {quizItem.answer_1}");
        }
        
        if (SoundManager.instance != null)
        {
            if (isCorrect)
            {
                SoundManager.instance.PlayCorrectAnswer();
            }
            else
            {
                SoundManager.instance.PlayWrongAnswer();
            }
        }
    }

    private IEnumerator NewQuestionDelay()
    {
        yield return new WaitForSeconds(.9f);

        ResetButtons();
        RandomizeQuestion();
    }

    private void ResetButtons()
    {
        _answer1Button.interactable = true;
        _answer2Button.interactable = true;
        _answer3Button.interactable = true;

        _answer1Button.GetComponent<ButtonAnimator>().enabled = true;
        _answer2Button.GetComponent<ButtonAnimator>().enabled = true;
        _answer3Button.GetComponent<ButtonAnimator>().enabled = true;
    }

    private void OnCorrectAnswerClicked()
    {
        _questionCounter++;
        _questionCounterText.text = $"Domanda {_questionCounter}/ {_quizItems.Count}";
        _answer1Button.image.color = Color.green;
        _answer2Button.image.color = Color.softRed;
        _answer3Button.image.color = Color.softRed;
        _answer1Button.interactable = false;
        _answer2Button.interactable = false;
        _answer3Button.interactable = false;
        _answer1Button.GetComponent<ButtonAnimator>().enabled = false;
        _answer2Button.GetComponent<ButtonAnimator>().enabled = false;
        _answer3Button.GetComponent<ButtonAnimator>().enabled = false;
        StartCoroutine(NewQuestionDelay());
    }

    private void OnWrongAnswerClicked()
    {
        _explanationText.text = _currentQuizItem.explanation;
        _panelManager.ShowWrongAnswerPanel();
    }
}
