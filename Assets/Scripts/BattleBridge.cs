using System;

public static class BattleBridge
{
    public static bool IsBattleActive = false;
    public static float EnemyDifficulty = 1f; // เพิ่มบรรทัดนี้เพื่อแก้ Error

    public static Action OnBattleEnded;

    public static void EndBattle()
    {
        IsBattleActive = false;
        OnBattleEnded?.Invoke();
    }
}