using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private GameObject sizzlingParticles;
    [SerializeField] private GameObject stoveOnVisual;

    public void ShowStoveEffect()
    {
        sizzlingParticles.SetActive(true);
        stoveOnVisual.SetActive(true);
    }
    public void HideStoveEffect()
    {
        sizzlingParticles.SetActive(false);
        stoveOnVisual.SetActive(false);
    }
}
