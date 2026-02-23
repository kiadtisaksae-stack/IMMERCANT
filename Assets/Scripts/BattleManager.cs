using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public void LostBattle()
    {
        // เมื่อกดปุ่ม 'หนี' หรือ 'ชนะ' ให้เรียกฟังก์ชันนี้
        BattleBridge.IsBattleActive = false;
    }
    public void WinBattle()
    {
                // เมื่อชนะศัตรูทั้งหมด ให้เรียกฟังก์ชันนี้
        BattleBridge.IsBattleActive = false;
    }


}
