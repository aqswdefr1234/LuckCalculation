using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OthersController : MonoBehaviour
{
    Transform mainCanvas, cam;
    Transform testLuckPanel, othersButtonPanel, others;
    Button backBtn;
    void Start()
    {
        SetUI();
    }
    void SetUI()
    {
        //루트 오브젝트
        mainCanvas = FindScene.FindSceneRoot("MainCanvas");
        cam = FindScene.FindSceneRoot("MainCamera");
        //메인 패널
        testLuckPanel = mainCanvas.Find("TestLuckPanel");
        othersButtonPanel = mainCanvas.Find("OthersButtonPanel");
        others = mainCanvas.Find("Others");

        //뒤로가기버튼
        backBtn = others.Find("BackButton").GetComponent<Button>();
        backBtn.onClick.AddListener(BackButtonClick);

        //OthersButtonPanel의 하위 패널
        foreach (Transform child in othersButtonPanel)
        {
            if (child.GetComponent<Button>() == null) continue;
            child.GetComponent<Button>().onClick.AddListener(() => SetButtonClick(child));
        }
    }
    void SetButtonClick(Transform child)
    {
        if (child.name == "RidBadLuckButton")
        {
            if(LuckCalculator.dailyScore == 0)
            {
                Notification.notiList.Add("Please check <Daily Luck> first");
                return;
            }
            //광고. 오늘 아직 안봤다면 광고 실행
            if(PlayerPrefsController.LoadAdForwardState() == "Yet")
                transform.GetComponent<AdMobForward>().ShowInterstitialAd();

            Transform panel = others.Find("RidBadLuckPanel");
            panel.gameObject.SetActive(true);

            RidBadLuckButtonClick();
        }
        if (child.name == "SimulationButton")
        {
            Transform panel = others.Find("SimulationPanel");
            panel.gameObject.SetActive(true);
        }
        if (child.name == "PinballButton")
        {
            Notification.notiList.Add("Service coming soon!");
        }
        //활성화 된 패널이 있다면 뒤로가기 버튼 활성화
        BackButtonActivation();
    }
    void BackButtonActivation()
    {
        foreach (Transform panel in others)
        {
            //태그가 없다면 건너뛰기. Others안의 BackButton을 건너 뛰기 위해서.
            if (panel.tag != "OtherPanel") continue;

            if(panel.gameObject.activeSelf == true)
            {
                backBtn.gameObject.SetActive(true);
                return;
            }
        }
    }
    void BackButtonClick()
    {
        ResetState();
        foreach (Transform child in others) child.gameObject.SetActive(false);
    }
    void RidBadLuckButtonClick()
    {
        cam.position = new Vector3(53f, 12f, 3f);
        testLuckPanel.gameObject.SetActive(false);
        othersButtonPanel.gameObject.SetActive(false);
    }
    
    void ResetState()
    {
        cam.position = new Vector3(0, 0, 0);
        testLuckPanel.gameObject.SetActive(true);
        othersButtonPanel.gameObject.SetActive(true);
    }
    public Transform FindSceneRoot(string rootName)
    {
        foreach (GameObject rootObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (rootName == rootObject.transform.name) return rootObject.transform;
        }
        return null;
    }
}
