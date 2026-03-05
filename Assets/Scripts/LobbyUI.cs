using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class LobbyUI : MonoBehaviour
{
    public static LobbyUI Instance;
    public TextMeshProUGUI goldText;
    public Button shopBtn;
    public GameObject shopPanel;
    public GameObject bagPanel;

    [Header("Static Inventory Slots")]
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();
    private InventorySlot currentSelectedSlot;

    public Image imageCityBG;
    private Capital activeCapital;

    private void Awake() => Instance = this;

    private void Start()
    {
        if (shopBtn != null) shopBtn.onClick.AddListener(() => {
            OpenPanel(shopPanel);
            
        });
        UpdateGoldText(GameManager.Instance.Gold);
    }

    public void SetupCity(Capital capital, Sprite citySprite, List<ItemSO> items)
    {
        activeCapital = capital;
        if (imageCityBG != null) imageCityBG.sprite = citySprite;

        UpdateGoldText(GameManager.Instance.Gold);
        CloseAllPanels();
    }

    public void RefreshInventoryUI()
    {
        var items = GameManager.Instance.inventory.ToList();
  
    }

    public void OpenPanel(GameObject targetPanel)
    {
        CloseAllPanels();
        targetPanel.SetActive(true);
        targetPanel.transform.DOKill();
        targetPanel.transform.localScale = Vector3.one * 0.8f;
        targetPanel.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
    }

    public void CloseAllPanels()
    {
        if (bagPanel) bagPanel.SetActive(false);
        if (shopPanel) shopPanel.SetActive(false);
        currentSelectedSlot = null;
    }

    public void SelectSlot(InventorySlot newSlot)
    {

    }

    public void UpdateGoldText(int amount)
    {
        goldText.text = "Gold: " + amount.ToString("N0");
        goldText.transform.DOPunchScale(Vector3.one * 0.1f, 0.2f);
    }
}