using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizItem_UI : MonoBehaviour
{
    [Header("Riferimenti UI")]
    public TMP_Text idText;
    public TMP_Text questionText;
    public Image background;

    public void Setup(QuizItem quizItem)
    {
        if (idText != null)
            idText.text = $"ID: {quizItem.category_id}";

        if (questionText!= null)
            questionText.text = $"Domanda: {quizItem.question_text}";
    }
}
