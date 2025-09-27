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
    public event EventHandler OnPauseEvent;

    private GameInputControl inputControl;

    //单例模式，提供外界访问点
    public static GameInput Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        inputControl = new GameInputControl();
        inputControl.Player.Enable();
    }

    private void Start()
    {
        if (DataManager.Instance.LoadKeyData() != null)
        {
            inputControl.LoadBindingOverridesFromJson(DataManager.Instance.LoadKeyData());
        }

        inputControl.Player.Interaction.performed += InteractionEvent => { OnInteractionEvent?.Invoke(this, EventArgs.Empty); };
        inputControl.Player.Make.performed += MakeEvent => { OnMakeEvent?.Invoke(this,EventArgs.Empty); };
        inputControl.Player.Pause.performed += PauseEvent => { OnPauseEvent?.Invoke(this, EventArgs.Empty); };
    }

    private void OnDisable()
    {
        inputControl.Player.Interaction.performed -= InteractionEvent => { OnInteractionEvent?.Invoke(this, EventArgs.Empty); };
        inputControl.Player.Make.performed -= MakeEvent => { OnMakeEvent?.Invoke(this, EventArgs.Empty); };
        inputControl.Player.Pause.performed -= PauseEvent => { OnPauseEvent?.Invoke(this, EventArgs.Empty); };

        inputControl.Dispose();
    }

    /// <summary>
    /// 移动值
    /// </summary>
    /// <returns></returns>
    public Vector3 GetMoveInputValue()
    {
        Vector2 inputDir =  inputControl.Player.Move.ReadValue<Vector2>();

        return new Vector3(inputDir.x, 0, inputDir.y).normalized;
    }

    //获取移动的按键信息
    public List<String> GetMoveButton()
    {
        List<String> buttons = new List<String>();
        foreach (var binding in inputControl.Player.Move.bindings)
        {
            buttons.Add(binding.ToDisplayString());
        }
        return buttons;
    }
    //获取交互的按键信息
    public String GetTakeButton()
    {
        return inputControl.Player.Interaction.bindings[0].ToDisplayString();
    }
    //获取切的按键信息
    public String GetCutButton()
    {
        return inputControl.Player.Make.bindings[0].ToDisplayString();
    }

    //修改移动按键设置
    public void ModifyMoveButton(int index,Action OnComplete)
    {
        inputControl.Player.Disable();
        inputControl.Player.Move.PerformInteractiveRebinding(index).OnComplete(callback =>
        {
            inputControl.Enable();
            OnComplete?.Invoke();

            DataManager.Instance.SaveKeyData(inputControl.SaveBindingOverridesAsJson());
        }).Start();
    }
    //修改拿按键设置
    public void ModifyTakeButton(int index,Action OnComplete)
    {
        inputControl.Player.Disable();
        inputControl.Player.Interaction.PerformInteractiveRebinding(index).OnComplete(callback =>
        {
            inputControl.Enable();
            OnComplete?.Invoke();

            DataManager.Instance.SaveKeyData(inputControl.SaveBindingOverridesAsJson());
        }).Start();
    }
    //修改切按键设置
    public void ModifyCutButton(int index, Action OnComplete)
    {
        inputControl.Player.Disable();
        inputControl.Player.Make.PerformInteractiveRebinding(index).OnComplete(callback =>
        {
            inputControl.Enable();
            OnComplete?.Invoke();

            DataManager.Instance.SaveKeyData(inputControl.SaveBindingOverridesAsJson());
        }).Start();
    }
}
