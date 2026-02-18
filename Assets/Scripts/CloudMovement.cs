using UnityEngine;
using DG.Tweening;

public class CloudMovement : MonoBehaviour
{
    public enum Direction { Left, Right }

    [Header("Movement Settings")]
    public Direction startDirection = Direction.Right; // เลือกทิศทางเริ่มต้นใน Inspector
    public float distance = 2f;

    [Header("Speed Settings")]
    public float minDuration = 3f; // เวลาเร็วที่สุด
    public float maxDuration = 6f; // เวลาช้าที่สุด

    void Start()
    {
        // 1. สุ่มความเร็ว (Duration)
        float randomDuration = Random.Range(minDuration, maxDuration);

        // 2. คำนวณจุดหมายตามทิศทางที่เลือก
        float targetX = transform.position.x;
        if (startDirection == Direction.Right)
        {
            targetX += distance;
        }
        else
        {
            targetX -= distance;
        }

        // 3. เริ่มการเคลื่อนที่
        transform.DOMoveX(targetX, randomDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(UpdateType.Normal); // ทำงานตาม Update ปกติของ Unity
    }
}