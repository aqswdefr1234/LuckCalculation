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
    //�۾��� ���� �� �� ���� �ѹ��� �۵��ϰ� ��
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
        //�����Ͱ� �������� �ʰų� ������ ������ �ʾҴٸ�
        if (!PlayerPrefsController.LoadAfterMidnight())
        {
            int preScore = PlayerPrefsController.LoadDailyScore();
            //9999�� ���� ���� �����Ͱ� �������� �ʴ� ����̴�.
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

            scoreText.text = $"<color=#FFFF00>{score}</color> / 100";//�����
            phraseText.text = str;
        });
    }
    (int, string) CalculateLuck()
    {
        int score = 0;
        //0�� 1�� �ϳ� ���õ�
        int initialSelection = UnityEngine.Random.Range(0, 2);
        //100�� �ݺ��Ͽ� ó���� ���õ� ���ڰ� ��� �����ϴ��� ī��Ʈ
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
        //������. ������
        if (score <= 35)
        {
            return "<color=#00FF00>Unfortunate...</color>";
        }
        //���� ������. ��������
        if (36 <= score && score <= 44)
        {
            return "<color=#FF6666>Slightly Unlucky...</color>";
        }
        //���. ���
        if (45 <= score && score <= 55)
        {
            return "<color=#FFFFFF>Ordinary</color>";
        }
        //�ణ ����. �ʷϻ�
        if (56 <= score && score <= 64)
        {
            return "<color=#00FF00>Slightly Lucky!</color>";
        }
        //����. �Ķ���
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
        scoreText.text = $"<color=#FFFF00>{score}</color> / 100";//�����
        phraseText.text = phrase;
    }
}
