using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LuckCalculator : MonoBehaviour
{
    public static int dailyScore = 0;
    [SerializeField] private Transform testLuckPanel;

    TMP_Text scoreText, phraseText;
    ImageAnimator imageAnimator = new ImageAnimator();

    bool isPressed = false;

    void Awake()
    {
        SetUI();
        CheckPreData();
    }
    //작업이 종료 될 때 까지 한번만 작동하게 됨
    void SetUI()
    {
        Transform resultPanel = testLuckPanel.Find("ResultPanel");
        scoreText = resultPanel.Find("ScoreText").GetComponent<TMP_Text>();
        phraseText = resultPanel.Find("PhraseText").GetComponent<TMP_Text>();
        //TestLuckButton Click
        Button calBtn = testLuckPanel.Find("TestLuckButton").GetComponent<Button>();
        calBtn.onClick.AddListener(() => ClickTestLuck(calBtn.transform));
    }
    void CheckPreData()
    {
        //데이터가 존재하지 않거나 자정이 지나지 않았다면
        if (!PlayerPrefsController.LoadAfterMidnight())
        {
            int preScore = PlayerPrefsController.LoadDailyScore();
            //9999일 경우는 이전 데이터가 존재하지 않는 경우이다.
            if (preScore != 9999)
            {
                UpdateScore(preScore);
            }
            isPressed = true;
        }
    }
    void ClickTestLuck(Transform target)
    {
        if (isPressed)
        {
            Notification.notiList.Add("Already checked. It is reset at 0:00.");
            return;
        }

        isPressed = true;
        Image fillImage = target.GetComponent<Image>();
        imageAnimator.FillAndCb(fillImage, 2f, Image.FillMethod.Radial360, 0, () =>
        {
            (int score, string str) = CalculateLuck();
            dailyScore = score;

            PlayerPrefsController.SaveMidnightData();
            PlayerPrefsController.SaveScore(score);

            scoreText.text = $"<color=#FFFF00>{score}</color> / 100";//노란색
            phraseText.text = str;
        });
    }
    (int, string) CalculateLuck()
    {
        int score = 0;
        //0과 1중 하나 선택됨
        int initialSelection = UnityEngine.Random.Range(0, 2);
        //100번 반복하여 처음에 선택된 숫자가 몇번 등장하는지 카운트
        for (int i = 0; i < 100; i++)
        {
            int randomValue = UnityEngine.Random.Range(0, 2);
            if (randomValue == initialSelection)
            {
                score++;
            }
        }
        string phrase = WritePhrase(score);
        return (score, phrase);
    }
    string WritePhrase(int score)
    {
        //안좋음. 빨간색
        if (score <= 35)
        {
            return "<color=#00FF00>Unfortunate...</color>";
        }
        //조금 안좋음. 연빨강색
        if (36 <= score && score <= 44)
        {
            return "<color=#FF6666>Slightly Unlucky...</color>";
        }
        //평범. 흰색
        if (45 <= score && score <= 55)
        {
            return "<color=#FFFFFF>Ordinary</color>";
        }
        //약간 좋음. 초록색
        if (56 <= score && score <= 64)
        {
            return "<color=#00FF00>Slightly Lucky!</color>";
        }
        //좋음. 파란색
        if(65 <= score)
        {
            return "<color=#0000FF>Fortunate!</color>";
        }
        return "";
    }
    public void UpdateScore(int score)
    {
        dailyScore = score;
        string phrase = WritePhrase(score);
        scoreText.text = $"<color=#FFFF00>{score}</color> / 100";//노란색
        phraseText.text = phrase;
    }
}
