using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private GameObject tutorialUI;

    private void Awake()
    {
        Hide();
    }

    private void Update()
    {
        if (GameManager.Instance.GetGameState() == GameManager.GameState.WaitingToStart)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    public void Show()
    {
        tutorialUI.SetActive(true);
    }
    public void Hide()
    {
        tutorialUI.SetActive(false);
    }
}
