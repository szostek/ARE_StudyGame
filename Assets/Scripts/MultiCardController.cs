using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MultiCardController : MonoBehaviour
{
    [Header("Info for spawning:")]
    public string questionType;
    public int qIndex;
    public string question;
    public string instructions;
    public string description;
    public string[] answers;
    public Sprite[] imgAnswers;
    public List<int> correctAnswersList = new List<int>();
    public int answerAmount;
    public bool hasMultAnswers;
    public bool hasImagesForAnswers;

    public TapToMark imageToTapPrefab;
    private TapToMark imageToTap;
    
    public Sprite imageRef;
    public bool tappedOnCorrectArea;
    public bool hasTappedImage;

    private GameManager gameManager;
    private StatusController statusController;

    [Header("UI References:")]
    [SerializeField] TMP_Text questionText;
    [SerializeField] TMP_Text instructionTextPrefab;
    private TMP_Text instructionText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] Button answerButtonPrefab;
    [SerializeField] Button imgAnswerButtonPrefab;
    [SerializeField] TMP_InputField fillInBlankPrefab;
    private TMP_InputField answerTextField;
    [SerializeField] Image imageRefPrefab;
    [SerializeField] Button submitButtonPrefab;
    [SerializeField] RectTransform content;

    [SerializeField] GameObject finishedPanel;
    [SerializeField] Button retryButton;
    [SerializeField] Animator correctAnimator;
    [SerializeField] Animator wrongAnimator;

    private Button submitButton;    
    private List<Button> selectedButtons = new List<Button>();
    private List<Button> allAnswerButtons = new List<Button>();
    private List<int> selectedAnswerList = new List<int>();
    private string[] alphabet = new string[] {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J"};

    private void Awake() 
    {
        gameManager = FindObjectOfType<GameManager>();
        statusController = FindObjectOfType<StatusController>();
    }
    private void Start() 
    {
        PopulateQuestion();
    }

    private void Update() 
    {
        if (!submitButton) return;
        if (questionType == "isMultiChoice") {
            submitButton.interactable = selectedButtons.Count > 0;
        } else if (questionType == "isFillInBlank") {
            submitButton.interactable = answerTextField.text.Length > 0;
        } else if (questionType == "isTapOnImage") {
            submitButton.interactable = hasTappedImage;
        }
    }

    public void PopulateQuestion()
    {
        // Clear the list of selected buttons
        selectedButtons.Clear();
        
        questionText.text = question;

        if (questionType == "isMultiChoice") {
            if (imageRef != null) {
                AddImageRefIfAvailable();
            }
            AddInstructionText();
            PopulateAnswerButtons();
            AddMultChoiceSubmitButton();
        } else if (questionType == "isFillInBlank") {
            if (imageRef != null) {
                AddImageRefIfAvailable();
            }
            AddInstructionText();
            answerTextField = Instantiate(fillInBlankPrefab, content);
            AddFillBlankSubmitButton();
        } else if (questionType == "isTapOnImage") {
            imageToTap = Instantiate(imageToTapPrefab, content);
            RectTransform rt = imageToTap.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, 1000);
            AddInstructionText();
            AddImageTapSubmitButton();
        }

    }

    public void AddInstructionText()
    {
        TMP_Text instruction = Instantiate(instructionTextPrefab, content);
        instruction.text = instructions;
    }

    public void AddImageRefIfAvailable()
    {
        Image referenceImage = Instantiate(imageRefPrefab, content);
        referenceImage.sprite = imageRef;
    }

    public void AddMultChoiceSubmitButton()
    {
        submitButton = Instantiate(submitButtonPrefab, content);
        submitButton.interactable = false;
        submitButton.onClick.AddListener(() => 
        {
            foreach (Button button in selectedButtons) {
                int selectedAnswer = button.GetComponent<AnswerButton>().answerIndex;
                selectedAnswerList.Add(selectedAnswer);
            }
            // Check if the answers are correct:
            if (CompareLists(selectedAnswerList, correctAnswersList)) {
                foreach (Button button in allAnswerButtons) {
                    button.interactable = false;
                }
                FinishQuestion(true);
            } else {
                selectedAnswerList.Clear();
                selectedButtons.Clear();
                foreach (Button button in allAnswerButtons) {
                    button.GetComponent<Image>().color = Color.white;
                    button.interactable = true;
                }
                FinishQuestion(false);
            }
            
        });
    }

    public void AddFillBlankSubmitButton()
    {
        submitButton = Instantiate(submitButtonPrefab, content);
        submitButton.interactable = false;
        submitButton.onClick.AddListener(() => 
        {
            if (answerTextField.text.ToLower() == answers[0].ToLower() || answerTextField.text.ToLower().Contains(answers[0].ToLower())) {
                FinishQuestion(true);
                answerTextField.text = "";
            } else {
                FinishQuestion(false);
                answerTextField.text = "";
            }            
        });
    }

    public void AddImageTapSubmitButton()
    {
        submitButton = Instantiate(submitButtonPrefab, content);
        submitButton.interactable = false;
        submitButton.onClick.AddListener(() => 
        {
            if (hasTappedImage && tappedOnCorrectArea) {
                FinishQuestion(true);
            } else {
                FinishQuestion(false);
            }            
        });
    }

    private void FinishQuestion(bool success)
    {
        if (!gameManager.isChallengeMode) {
            if (success) {
                descriptionText.text = description;
                retryButton.gameObject.SetActive(false);
                finishedPanel.SetActive(true);
                correctAnimator.SetTrigger("show");
            } else {
                descriptionText.text = "That's not quite correct.";
                finishedPanel.SetActive(true);
                wrongAnimator.SetTrigger("show");
            }
        } else {
            if (success) {
                gameManager.AddPoints();
            } else {
                statusController.AddQuestionToStruggleList(qIndex);
            }
            Next();
        }
    }

    public void Retry()
    {
        finishedPanel.SetActive(false);
    }

    public void Next()
    {
        Destroy(gameObject);
        gameManager.CreateQuestion();
    }

    private void PopulateAnswerButtons() 
    {
        for (int i = 0; i < answerAmount; i++) 
        {
            Button answerButton;
            AnswerButton buttonInfo;
            TMP_Text answerText;
            if (hasImagesForAnswers) {
                answerButton = Instantiate(imgAnswerButtonPrefab, content);
                allAnswerButtons.Add(answerButton);
                buttonInfo = answerButton.GetComponent<AnswerButton>();
                buttonInfo.answerIndex = i;
                answerText = answerButton.GetComponentInChildren<TMP_Text>();
                answerText.text = alphabet[i].ToString();
                buttonInfo.answerImage.sprite = imgAnswers[i];
            } else {
                answerButton = Instantiate(answerButtonPrefab, content);
                allAnswerButtons.Add(answerButton);
                buttonInfo = answerButton.GetComponent<AnswerButton>();
                buttonInfo.answerIndex = i;
                answerText = answerButton.GetComponentInChildren<TMP_Text>();
                answerText.text = $"{alphabet[i]}. {answers[i]}";
            }
            
            // Add event listener to the button
            if (hasMultAnswers) 
            {
                answerButton.onClick.AddListener(() => 
                {
                    if (selectedButtons.Contains(answerButton)) 
                    {
                        // Button is already selected, so unselect it
                        selectedButtons.Remove(answerButton);
                        answerButton.GetComponent<Image>().color = Color.white;
                    } 
                    else 
                    {
                        // Button is not selected, so select it
                        selectedButtons.Add(answerButton);
                        answerButton.GetComponent<Image>().color = Color.green;
                    }
                });
            } 
            else 
            {
                // Add event listener to the button to select it
                answerButton.onClick.AddListener(() => 
                {
                    foreach (Button button in allAnswerButtons) 
                    {
                        if (button == answerButton) 
                        {
                            // Button is selected
                            selectedButtons.Clear();
                            selectedButtons.Add(button);
                            button.GetComponent<Image>().color = Color.green;
                        } 
                        else 
                        {
                            // Button is unselected
                            button.GetComponent<Image>().color = Color.white;
                        }
                    }
                });
            }
        }
    }

    public bool CompareLists(List<int> list1, List<int> list2)
    {
        // If the lists are different lengths, they can't be the same
        if (list1.Count != list2.Count)
        {
            return false;
        }

        // Create two sorted lists to compare
        List<int> sortedList1 = list1.OrderBy(i => i).ToList();
        List<int> sortedList2 = list2.OrderBy(i => i).ToList();

        // Compare each element in the sorted lists
        for (int i = 0; i < sortedList1.Count; i++)
        {
            if (sortedList1[i] != sortedList2[i])
            {
                return false;
            }
        }

        // If all elements match, return true
        return true;
    }

}
