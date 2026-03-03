using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LobbyUI : MonoBehaviour
{
    public static LobbyUI Instance;

    [Header("Header UI")]
    public TextMeshProUGUI goldText;
    public Button settingBtn;

    [Header("Footer Buttons")]
    public Button homeBtn;
    public Button mercenaryBtn;
    public Button drawBtn;
    public Button bagBtn;
    public Button worldMapBtn;
    public Button shop;

    [Header("Panels")]
    public GameObject drawPanel;
    public GameObject bagPanel;
    public GameObject mercenaryPanel;
    public GameObject estatePanel;
    public GameObject shopPanel;

    [Header("Inventory Settings")]
    public Transform inventoryContainer; // วัตถุที่มี Grid Layout Group
    public GameObject slotPrefab;
    private InventorySlot currentSelectedSlot;

    [Header("City Data")]
    public Image imageCityBG;
    private Capital activeCapital;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // ผูกฟังก์ชันเข้ากับปุ่ม
        worldMapBtn.onClick.AddListener(OnClickWorldMap);
        drawBtn.onClick.AddListener(() => OpenPanel(drawPanel));
        bagBtn.onClick.AddListener(() => { OpenPanel(bagPanel); RefreshInventoryUI(); });
        mercenaryBtn.onClick.AddListener(() => OpenPanel(mercenaryPanel));
        homeBtn.onClick.AddListener(CloseAllPanels);

        UpdateGoldText(GameManager.Instance.Gold);
    }

    // --- ระบบจัดการฉากเมือง ---
    public void SetupCity(Capital capital, Sprite citySprite)
    {
        activeCapital = capital;
        if (imageCityBG != null) imageCityBG.sprite = citySprite;
        UpdateGoldText(GameManager.Instance.Gold);
        CloseAllPanels();
    }

    public void OnClickWorldMap()
    {
        if (activeCapital != null)
        {
            if (activeCapital.mapRoot != null) activeCapital.mapRoot.SetActive(true);
            activeCapital.Collider.enabled = false;
            gameObject.SetActive(false);
        }
    }

    // --- ระบบจัดการหน้าจอ (Panels) ---
    public void OpenPanel(GameObject targetPanel)
    {
        CloseAllPanels();
        targetPanel.SetActive(true);
        // เล่น Animation เล็กน้อยตอนเปิด Panel
        targetPanel.transform.localScale = Vector3.one * 0.8f;
        targetPanel.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
    }

    public void CloseAllPanels()
    {
        drawPanel.SetActive(false);
        bagPanel.SetActive(false);
        mercenaryPanel.SetActive(false);
        if (estatePanel != null) estatePanel.SetActive(false);
        currentSelectedSlot = null; // ล้างสถานะการเลือก
    }

    // --- ระบบคลังไอเทม (Inventory) ---
    public void RefreshInventoryUI()
    {
        // ล้าง Slot เก่า
        foreach (Transform child in inventoryContainer) Destroy(child.gameObject);

        // สร้าง Slot ใหม่จากข้อมูลใน GameManager
        foreach (var entry in GameManager.Instance.inventory)
        {
            GameObject obj = Instantiate(slotPrefab, inventoryContainer);
            InventorySlot slot = obj.GetComponent<InventorySlot>();
            slot.SetItem(entry.Key, entry.Value);
        }
    }

    public void SelectSlot(InventorySlot newSlot)
    {
        if (currentSelectedSlot != null && currentSelectedSlot != newSlot)
        {
            currentSelectedSlot.DeselectAnim();
        }
        currentSelectedSlot = newSlot;
        currentSelectedSlot.SelectAnim();
    }

    public void UpdateGoldText(int amount)
    {
        goldText.text = "Gold: " + amount.ToString("N0");
        // เอฟเฟกต์ตัวเลขเด้งตอนเงินเปลี่ยน
        goldText.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f);
    }
}