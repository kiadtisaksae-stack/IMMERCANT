using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class IMMInput : MonoBehaviour
{
    // สร้าง Action เพื่อให้ PlayerController มาสมัครรับข้อมูล
    public Action<Vector2> OnMouseClick;

    private void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            OnMouseClick?.Invoke(mousePosition);
        }
    }
}