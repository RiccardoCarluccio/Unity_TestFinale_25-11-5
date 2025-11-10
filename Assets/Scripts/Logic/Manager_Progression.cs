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
    [SerializeField] private Manager_User _userManager;
    [SerializeField] private Button _answer1Button, _answer2Button, _answer3Button;
    [SerializeField] private TextMeshProUGUI _questionText, _questionCounterText, _explanationText;
    [SerializeField] private PanelManager _panelManager;

    private int _category_id;
    private List<QuizItem> _quizItems = new();
    private int _questionCounter = 1;
    private QuizItem _currentQuizItem;
    private int _totalQuestions;

    [Header("Debug")]
    public bool enableDebugLogs = true;
    public bool IsTesting = true;
    public int numOfQuestions = 1;

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
        var currentSceneName = SceneManager.GetActiveScene().name;
        if (Enum.TryParse(currentSceneName, out ChosenScene chosenScene))
        {
            _category_id = (int)chosenScene;

            if (enableDebugLogs)
                Debug.Log($"Scene: {currentSceneName}, Category ID: {_category_id}");

            _managerCrud.LoadQuizItemsByCategory(_category_id);     //calls OnQuizItemsLoaded event

            if (enableDebugLogs)
                Debug.Log($"total questions: {_quizItems.Count}");
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

        _quizItems = new List<QuizItem>(quizItems);
        if (IsTesting)
            _totalQuestions = numOfQuestions;
        else
            _totalQuestions = _quizItems.Count;
        _questionCounter = 1;

        if (enableDebugLogs)
            Debug.Log($"Loaded {_quizItems.Count} quiz items.");

        _questionCounterText.text = $"Domanda: {_questionCounter}/ {_totalQuestions}";

        RandomizeQuestion();
    }

    /// <summary>
    /// Randomize the question among all the question of the category
    /// </summary>
    private void RandomizeQuestion()
    {
        if (_questionCounter == 0)
        {
            _panelManager.ShowEndPanel();
            return;
        }

        int quizItemIndex = UnityEngine.Random.Range(0, _quizItems.Count);
        _currentQuizItem = _quizItems[quizItemIndex];
        _quizItems.RemoveAt(quizItemIndex);
        _questionText.text = _currentQuizItem.question_text;
        if (enableDebugLogs)
            Debug.Log($"Quiz item randomly selected => ID: {_currentQuizItem.quiz_item_id}, Correct Answer: {_currentQuizItem.correct_answer}, Answer #2: {_currentQuizItem.answer_2}");

        AssingAnswersToButtons(_currentQuizItem);
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

        _answer1Button.onClick.AddListener(() => OnAnswerSelected(_answer1Button, quizItem, randomizedAnswers[0]));
        _answer2Button.onClick.AddListener(() => OnAnswerSelected(_answer2Button, quizItem, randomizedAnswers[1]));
        _answer3Button.onClick.AddListener(() => OnAnswerSelected(_answer3Button, quizItem, randomizedAnswers[2]));
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
            quizItem.correct_answer,
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
    private void OnAnswerSelected(Button clickedButton, QuizItem quizItem, string chosenAnswer)
    {
        if (chosenAnswer == quizItem.correct_answer)          //change column to 'correct_answer'
        {
            //Add button style in case of correct answer
            OnCorrectAnswerClicked(clickedButton);
            if (enableDebugLogs)
                Debug.Log($"Correct answer. chosenAnswer: {chosenAnswer}, correct_answer: {quizItem.correct_answer}");
        }
        else
        {
            //Add button style in case of wrong answer
            OnWrongAnswerClicked();
            if (enableDebugLogs)
                Debug.Log($"Wrong answer. chosenAnswer: {chosenAnswer}, correct_answer: {quizItem.correct_answer}");
        }
    }



    /// <summary>
    /// Resets buttons after new questions, setting their interactability to true and resetting their color and animator
    /// </summary>

    private void ResetButton()
    {
        foreach (var button in new[] { _answer1Button, _answer2Button, _answer3Button })
        {
            button.interactable = true;
            button.image.color = new Color32(255, 255, 255, 0);
            button.GetComponent<ButtonAnimator>().enabled = true;
        }
    }

    /// <summary>
    /// Called when the correct answer button is clicked
    /// </summary>
    /// <param name="correctButton">The correct answer button</param>
    /// <remarks>
    /// Plays the correct answer sound, increments the question counter,
    /// sets the question counter text, sets the correct answer button color to green,
    /// sets the other answer buttons color to soft red, disables the answer buttons,
    /// disables the button animator, shows the question panel and waits for a new question
    /// </remarks>

    private void OnCorrectAnswerClicked(Button correctButton)
    {
        if (SoundManager.instance != null)
            SoundManager.instance.PlayCorrectAnswer();

        correctButton.image.color = Color.green;

        foreach (var button in new[] { _answer1Button, _answer2Button, _answer3Button })
        {
            if (button != correctButton)
                button.image.color = Color.softRed;

            button.interactable = false;
            button.GetComponent<ButtonAnimator>().enabled = false;
        }

        _panelManager.ShowQuestionPanel();
        StartCoroutine(NewQuestionDelay());

    }

    /// <summary>
    /// Delay between questions, waits for 1 second and then resets buttons, randomizes a new question and shows the end panel if the total questions have been answered
    /// </summary>

    private IEnumerator NewQuestionDelay()
    {
        yield return new WaitForSeconds(1f);

        _questionCounter++;
        // _currentQuizItemIndex++;

        if (_questionCounter > _totalQuestions)
        {
            _questionCounter = _totalQuestions;
            _questionCounterText.text = $"Domanda: {_questionCounter}/{_totalQuestions}";

            if (LoggedUser.Instance == null || LoggedUser.Instance.User == null)
            {
                if (enableDebugLogs)
                    Debug.LogError("LoggedUser.Instance or User is null!");
                _panelManager.ShowEndPanel();
                yield break;
            }

            var currentSceneName = SceneManager.GetActiveScene().name;
            if (Enum.TryParse(currentSceneName, out ChosenScene chosenScene))
            {
                var categoryName = chosenScene.ToString().Replace("Scene_", "");

                if (Enum.TryParse(categoryName, true, out Certificates certificate))        //"true" makes the method case insensitive
                {
                    _userManager.UpdateCertificates(LoggedUser.Instance.User.nickname, certificate);
                }
                else
                {
                    if (enableDebugLogs)
                        Debug.LogWarning($"No certificate found for category name: {categoryName}");
                }
            }

            _panelManager.ShowEndPanel();
            yield break;
        }

        
        _questionCounterText.text = $"Domanda: {_questionCounter}/{_totalQuestions}";
        ResetButton();
        RandomizeQuestion();
    }

    private void OnWrongAnswerClicked()
    {
        if (SoundManager.instance != null)
            SoundManager.instance.PlayWrongAnswer();
        _panelManager.ShowWrongAnswerPanel();
        _explanationText.text = _currentQuizItem.explanation;
    }

}
