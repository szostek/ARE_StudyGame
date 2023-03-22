using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Test Question", fileName = "New Question")]

public class QuestionSO : ScriptableObject
{
    [HideInInspector] public int questionIndex;
    [TextArea(2,6)]
    public string question = "Enter new question text here";
    [TextArea(2,6)]
    public string description = "More info about the correct answer..";
    public bool hasImageAnswers;
    public string[] answers; // Only text will be in the answer buttons
    public Sprite[] imgAnswers; // Use when images should appear in the answer buttons
    public List<int> correctAnswerIndicies;
    public InstructionTypes instruction;
    public QuestionType questionType;
    public Categories questionCategory;
    public TapToMark imageToTap;
    public Sprite imageRef;

    public string GetQuestionType()
    {
        return questionType.ToString();
    }

    public string GetInstruction() {
        string output = Regex.Replace(instruction.ToString(), @"\p{Lu}", m => " " + m.Value.ToLowerInvariant());
        output = char.ToUpperInvariant(output[0]) + output.Substring(1);
        return output;
    }

}
public enum InstructionTypes {
    chooseOnlyOnceAnswer,
    chooseAllThatApply,
    chooseTrueOrFalse,
    fillInTheBlank,
    tapALocationOnTheImage,
}

public enum QuestionType {
    isMultiChoice,
    isFillInBlank,
    isTapOnImage,
}

public enum Categories {
    pcm,
    pjm,
    pa,
    ppd,
    pdd,
    ce,
}

public class QuestionCategories {
    public int key;
    public SaveQuestionObject question;
}