using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Capital : MonoBehaviour
{
    public GameObject capitalScene;
    private Player player;
    [HideInInspector]
    public CircleCollider2D Collider;
    public GameObject mapRoot;
    public Sprite spriteCity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetUp();
    }
    private void Awake()
    {
        Collider = GetComponent<CircleCollider2D>();
        Collider.isTrigger = true;
        Collider.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetUp()
    {
        Collider = GetComponent<CircleCollider2D>();
        mapRoot = GameObject.FindWithTag("MapRoot");
        


    }
    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (Collider.enabled == false) return;
        if (collision.CompareTag("Player"))
        {
            Debug.Log("gggggggggggggggggggggggggggggggggggggggggggggg");
            // เปิด UI เมือง
            capitalScene.SetActive(true);
            mapRoot.SetActive(false);

            // ส่งข้อมูลเมืองนี้ไปให้ LobbyUI รู้จัก (เพื่อตอนกดกลับจะได้คืนค่าถูกเมือง)
            LobbyUI lobby = capitalScene.GetComponentInChildren<LobbyUI>();
            if (lobby != null)
            {
                lobby.SetupCity(this, spriteCity);
            }
        }
    }
   
}
