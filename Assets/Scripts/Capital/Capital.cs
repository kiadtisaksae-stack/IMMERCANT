using UnityEngine;
using System.Collections.Generic; // ต้องใช้สำหรับ List

[RequireComponent(typeof(CircleCollider2D))]
public class Capital : MonoBehaviour
{
    public GameObject capitalScene;
    [HideInInspector] public CircleCollider2D Collider;
    public GameObject mapRoot;
    public Sprite spriteCity;

    [Header("Shop Data")]
    public List<ItemSO> localShopItems; // สินค้าที่เมืองนี้ขาย (ลากใส่ใน Inspector ได้เลย)

    private void Awake()
    {
        Collider = GetComponent<CircleCollider2D>();
        Collider.isTrigger = true;
        Collider.enabled = false;
    }

    private void Start()
    {
        SetUp();
    }

    public void SetUp()
    {
        if (mapRoot == null) mapRoot = GameObject.FindWithTag("MapRoot");
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (Collider.enabled == false) return;

        if (collision.CompareTag("Player"))
        {
            capitalScene.SetActive(true);
            mapRoot.SetActive(false);

            LobbyUI lobby = capitalScene.GetComponentInChildren<LobbyUI>();
            if (lobby != null)
            {
                foreach (ItemSO item in localShopItems)
                {
                    item.maxStack = Random.Range(0, 99);
                }
                // --- ส่งข้อมูลไอเทมพ่วงไปด้วย ---
                lobby.SetupCity(this, spriteCity, localShopItems);
            }
        }
    }
}