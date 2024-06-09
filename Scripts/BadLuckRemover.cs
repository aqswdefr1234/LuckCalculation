using System.Collections;
using UnityEngine;

public class BadLuckRemover : MonoBehaviour
{
    [SerializeField] private Transform ridBadLuckPanel;
    [SerializeField] private Transform dice;

    UnityEngine.UI.Button rollingBtn;
    bool isRolled = false;

    Quaternion[] diceFaces = new Quaternion[]
    {
        Quaternion.identity,                           // Placeholder for 0 (not used)
        Quaternion.Euler(0, 0, 90),     //1
        Quaternion.Euler(90, 0, 0),     //2
        Quaternion.Euler(0, 0, 0),      //3
        Quaternion.Euler(180, 0, 0),    //4
        Quaternion.Euler(270, 0, 0),    //5
        Quaternion.Euler(0, 0, -90)     //6
    };

    void Start()
    {
        if (PlayerPrefsController.LoadRemoveVal() != 9999) isRolled = true;
        rollingBtn = ridBadLuckPanel.Find("RollingButton").GetComponent<UnityEngine.UI.Button>();
        rollingBtn.onClick.AddListener(RollingDice);
    }
    void RollingDice()
    {
        if(isRolled == false) 
        {
            isRolled = true;
            int ranInt = UnityEngine.Random.Range(1, 7);

            LuckCalculator.dailyScore += ranInt;
            transform.GetComponent<LuckCalculator>().UpdateScore(LuckCalculator.dailyScore);

            //PlayerPrefs 저장
            PlayerPrefsController.SaveScore(LuckCalculator.dailyScore);
            PlayerPrefsController.SaveRemoveVal(ranInt);
            PlayerPrefsController.SaveAdState("Already");

            CoroutineManager.Instance.CallBack(RollDice(ranInt), () =>
            {
                Notification.notiList.Add($"+{ranInt}");
            });
        }
        else
        {
            Notification.notiList.Add("Only available once a day");
        }
    }
    IEnumerator RollDice(int targetFace)
    {
        float rollDuration = 3.0f;
        float elapsedTime = 0f;

        // 빠른 속도로 주사위 회전
        while (elapsedTime < rollDuration)
        {
            dice.Rotate(new Vector3(Random.Range(720, 1440), Random.Range(720, 1440), Random.Range(720, 1440)) * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 목표 면으로 부드럽게 회전
        elapsedTime = 0f;
        Quaternion initialRotation = dice.rotation;
        Quaternion targetRotation = diceFaces[targetFace];

        while (elapsedTime < 1.0f)
        {
            dice.rotation = Quaternion.Lerp(initialRotation, targetRotation, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        dice.rotation = targetRotation;
    }
}

