using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameButtonSettingUI : MonoBehaviour
{
    [SerializeField] private Button Back;

    [SerializeField] private Button upKeyButton;
    [SerializeField] private Button downKeyButton;
    [SerializeField] private Button leftKeyButton;
    [SerializeField] private Button rightKeyButton;
    [SerializeField] private Button takeKeyButton;
    [SerializeField] private Button cutKeyButton;

    [SerializeField] private Text upKeyButtonText;
    [SerializeField] private Text downKeyButtonText;
    [SerializeField] private Text leftKeyButtonText;
    [SerializeField] private Text rightKeyButtonText;
    [SerializeField] private Text takeKeyButtonText;
    [SerializeField] private Text cutKeyButtonText;

    Dictionary<Text, int> moveKeyMap;

    [SerializeField] private GameObject inputNewButton;

    private void Awake()
    {
        Hide();

        //建立移动按键的映射
        moveKeyMap = new Dictionary<Text, int>
        {
            {upKeyButtonText,1 },
            {downKeyButtonText,2 },
            {leftKeyButtonText,3 },
            {rightKeyButtonText,4 }
        };

        Back.onClick.AddListener(() => 
        {
            Hide();
        });


        upKeyButton.onClick.AddListener(() => {ReBinding(moveKeyMap[upKeyButtonText], upKeyButtonText);});
        downKeyButton.onClick.AddListener(() => {ReBinding(moveKeyMap[downKeyButtonText], downKeyButtonText);});
        leftKeyButton.onClick.AddListener(() => {ReBinding(moveKeyMap[leftKeyButtonText], leftKeyButtonText);});
        rightKeyButton.onClick.AddListener(() => {ReBinding(moveKeyMap[rightKeyButtonText], rightKeyButtonText);});
        takeKeyButton.onClick.AddListener(() => {ReBinding(0,takeKeyButtonText);});
        cutKeyButton.onClick.AddListener(() => {ReBinding(0,cutKeyButtonText);});
    }

    private void Start()
    {
        UpdateButtonText();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide() 
    {
        inputNewButton.SetActive(false);
        gameObject.SetActive(false);
    }

    private void UpdateButtonText()
    {
        foreach (var map in moveKeyMap)
        {
            map.Key.text = GameInput.Instance.GetMoveButton()[map.Value];
        }
        takeKeyButtonText.text = GameInput.Instance.GetTakeButton();
        cutKeyButtonText.text = GameInput.Instance.GetCutButton();
    }
    private void ReBinding(int index,Text buttonText)
    {
        inputNewButton.SetActive(true);
        if (index != 0 && buttonText != takeKeyButtonText && buttonText != cutKeyButtonText)
        {
            GameInput.Instance.ModifyMoveButton(index, () => { 
                inputNewButton.SetActive(false); 
                UpdateButtonText(); 
            });
        }
        else if(buttonText == takeKeyButtonText)
        {
            GameInput.Instance.ModifyTakeButton(index, () => {
                inputNewButton.SetActive(false);
                UpdateButtonText(); 
            });
            
        }else if (buttonText == cutKeyButtonText) 
        {
            GameInput.Instance.ModifyCutButton(index, () => {
                inputNewButton.SetActive(false);
                UpdateButtonText(); 
            });
        }
    }
}
