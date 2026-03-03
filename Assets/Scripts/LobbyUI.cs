using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI goldText;
    public Button setting;

    [Header("ButtonHead")]
    public Button stroe;
    public Button abilities;
    [Header("ButtonFoot")]
    public Button home;
    public Button mercenary;
    public Button draw;
    public Button bag;
    [Header("WorldMapButton")]
    public Button worldMapBtn;
    public string sceneName;
    [Header("Panel")]
    public GameObject drawPanel;
    public GameObject bagPanel;
    public GameObject merchanaryPanel;

    private Capital activeCapital;
    [Header("BG_City")]
    public Image imageCity;




    void Start()
    {
        SetUpStart();
        worldMapBtn.onClick.AddListener(OnClickWorldMap);
        draw.onClick.AddListener(OnClickDraw);
        bag.onClick.AddListener(OnClickBag);
        mercenary.onClick.AddListener(OnClickMer);
    }

    // Setup ปุ่มให้โหลด Scene อัตโนมัติ
    public void SetUpStart()
    {
        UpdateGoldText(GameManager.Instance.Gold);
    }

    public void SetupCity(Capital capital , Sprite sprite)
    {
        activeCapital = capital;
        imageCity.sprite = sprite;
        UpdateGoldText(GameManager.Instance.Gold);
    }
    public void OnClickDraw()
    {
        drawPanel.SetActive(true);
    }
    public void OnClickBag()
    {
        bagPanel.SetActive(true);
    }
    public void OnClickMer()
    {
        merchanaryPanel.SetActive(true);
    }

    public void OnClickWorldMap()
    {
        if (activeCapital != null)
        {
            if (activeCapital.mapRoot != null)
            {
                activeCapital.mapRoot.SetActive(true);
            }
            activeCapital.Collider.enabled = false;

            gameObject.SetActive(false);
        }
    }

    public void UpdateGoldText(int goldAmount)
    {
        goldText.text = "Gold: " + goldAmount;
    }
}
