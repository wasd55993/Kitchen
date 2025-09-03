using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    //事件，处理交互按键，通知所有与交互相关的物体执行响应逻辑
    public event EventHandler OnInteractionEvent;
    public event EventHandler OnMakeEvent;

    private GameInputControl inputControl;
    private void Awake()
    {
        inputControl = new GameInputControl();
        inputControl.Player.Enable();
    }

    private void Start()
    {
        inputControl.Player.Interaction.performed += InteractionEvent => { OnInteractionEvent?.Invoke(this, EventArgs.Empty); };
        inputControl.Player.Make.performed += MakeEvent => { OnMakeEvent?.Invoke(this,EventArgs.Empty); };
    }

    public Vector3 GetMoveInputValue()
    {
        Vector2 inputDir =  inputControl.Player.Move.ReadValue<Vector2>();

        return new Vector3(inputDir.x, 0, inputDir.y).normalized;
    }
}
