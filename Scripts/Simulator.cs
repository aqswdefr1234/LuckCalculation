using UnityEngine;
using TMPro;
using System;

public class Simulator : MonoBehaviour
{
    [SerializeField] private Transform simulationPanel;
    TMP_InputField perInput, trialsInput;
    TMP_Text resultText;

    UnityEngine.UI.Button calBtn;

    void Start()
    {
        SetUI();
    }
    void SetUI()
    {
        resultText = simulationPanel.Find("ResultText").GetComponent<TMP_Text>();

        perInput = simulationPanel.Find("PercentageInputField").GetComponent<TMP_InputField>();
        trialsInput = simulationPanel.Find("TrialsInputField").GetComponent<TMP_InputField>();

        calBtn = simulationPanel.Find("CalculateButton").GetComponent<UnityEngine.UI.Button>();
        calBtn.onClick.AddListener(Calculate);
    }
    void Calculate()
    {
        int successes = 0;
        int failures = 0;

        (int numberOfTrials, float probability) = ReturnValue(trialsInput.text, perInput.text);
        if (numberOfTrials == -1) return;
        
        System.Random random = new System.Random();

        // 시행. NextDouble => 0 ~ 1 사이의 랜덤한 부동소수를 리턴한다.
        for (int i = 0; i < numberOfTrials; i++)
        {
            if (random.NextDouble() <= probability)
            {
                successes++;
            }
            else
            {
                failures++;
            }
        }

        // 실제 확률 계산
        float actualProbability = (float)successes / numberOfTrials * 100f;

        // 결과 텍스트 생성
        resultText.text = $"Number of Trials: {numberOfTrials}\n" +
                          $"Expected Probability: {probability * 100}%\n" +
                          $"Successes: {successes}\n" +
                          $"Failures: {failures}\n" +
                          $"Actual Probability: {actualProbability}%";
    }
    (int, float) ReturnValue(string trials, string percentage)
    {
        string message = "";
        int numberOfTrials = 0;
        float probability = 0f;

        //비어있는 항목 있으면 리턴
        if (perInput.text == "" || trialsInput.text == "")
        {
            message = "Please fill in all fields";
            Notification.notiList.Add(message);
            return (-1, -1f);
        }

        //int 또는 float범위를 벗어나면 리턴한다.
        try
        {
            numberOfTrials = int.Parse(trials);
            probability = float.Parse(percentage) / 100f;
        }
        catch (OverflowException ex)
        {
            UnityEngine.Debug.LogException(ex);
            message = "<Trials> : ​​1 to 2 billion. <Percentage> : 0 to 100";
            Notification.notiList.Add(message);
            return (-1, -1f);
        }
        //둘다 음수면 리턴, Percentage > 100이면 리턴
        if (numberOfTrials < 1)
        {
            message = "<Trials> Must be 1 or more.";
            Notification.notiList.Add(message);
            return (-1, -1f);
        }
        if (probability < 0)
        {
            message = "<Percentage> Must be 0 or more.";
            Notification.notiList.Add(message);
            return (-1, -1f);
        }
        if (probability > 100)
        {
            message = "<Percentage> Must be less than 100.";
            Notification.notiList.Add(message);
            return (-1, -1f);
        }
        return (numberOfTrials, probability);
    }
}
