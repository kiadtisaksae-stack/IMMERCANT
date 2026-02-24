using UnityEngine;

public class BattleManager : MonoBehaviour
{
    // ลากวัตถุที่มีสคริปต์ MainGameplayController มาใส่ในช่องนี้
    public MapTravel gameController;
    private void Start()
    {
        // ค้นหา MapTravel อัตโนมัติถ้าตัวแปรยังว่างอยู่
        if (gameController == null)
        {
            gameController = FindFirstObjectByType<MapTravel>();
        }
    }
    public void WinBattle()
    {
        // สั่งผ่านตัวคุมหลัก
        gameController.FinishBattleProcess();
    }
    public void LostBattle()
    {
        // สั่งผ่านตัวคุมหลัก
        gameController.LoseBattleProcess();
    }
}