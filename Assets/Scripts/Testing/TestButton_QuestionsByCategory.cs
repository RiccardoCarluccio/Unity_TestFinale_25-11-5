using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestButton_QuestionsByCategory : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Manager_CRUD _manager;
    [SerializeField] private TMP_InputField _category;

    private void OnEnable()
    {
        _button.onClick.AddListener(OnLoadQuizItemsClicked);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnLoadQuizItemsClicked);
    }

    private void OnLoadQuizItemsClicked()
    {
        _manager.LoadQuizItemsByCategory(int.Parse(_category.text));
    }
}
