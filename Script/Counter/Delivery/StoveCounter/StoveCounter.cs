using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StoveCounter : BaseCounter
{
    //声音播放
    public static event EventHandler OnWarning;

    [SerializeField] private DecoctionListSO decoctionListSO;
    [SerializeField] private StoveCounterVisual stoveCounterVisual;//显示和隐藏煎制动画
    [SerializeField] private AudioSource audioSource;

    private DecoctionInformation decoctionInformation;
    private float decoctionTimer = 0;

    [SerializeField] private KitchenObjectSO Overcooked;//煎糊的食材
    [SerializeField] private KitchenObjectSO cooked;//煎好的食材

    //警告间隔
    private float warningTimer = 0f;

    private DecoctionProgressUI decoctionProgressUI;//控制当前锅上的食材的进度UI

    //食材进度
    private InciseProgressUI progressUI;

    public override void Interaction(PlayerControl player)
    {
        if (player.IsHaveKitchen() && !this.IsHaveKitchen() && 
            decoctionListSO.TryGetDecoctionOutput(player.GetKitchenObject(),out DecoctionInformation newDecoctionInformation))
        {
            decoctionInformation = newDecoctionInformation;
            IngredientTransfer(player, this);
            audioSource.Play();
        }
        else if (!player.IsHaveKitchen() && this.IsHaveKitchen())
        {
            decoctionProgressUI.Hide();
            IngredientTransfer(this, player);
            audioSource.Pause();
        }
    }

    private void Update()
    {
        //灶有物体，进行煎制
        if (IsHaveKitchen())
        {
            decoctionProgressUI = GetKitchenObject().GetDecoctionProgressUI();
            stoveCounterVisual.ShowStoveEffect();
            DecoctionTreatment();
            if (GetKitchenObject().GetKitchenObjectSO() == cooked)
            {
                warningTimer += Time.deltaTime;
                if (warningTimer >= 0.5)
                {
                    OnWarning?.Invoke(this, EventArgs.Empty);
                    warningTimer = 0;
                }
            }
        }
        else
        {
            stoveCounterVisual.HideStoveEffect();
        }
    }

    private void DecoctionTreatment()
    {
        decoctionProgressUI.Show();

        if (GetKitchenObject().GetKitchenObjectSO() == Overcooked) { return; }

        decoctionTimer += Time.deltaTime;

        decoctionProgressUI.CookedProgress(decoctionTimer/ decoctionInformation.decoctionTime);

        if (decoctionTimer >= decoctionInformation.decoctionTime)
        {
            //删除源食材
            KitchenObjectPool.Instance.ReturnPool(GetKitchenObject().GetKitchenObjectSO(),GetKitchenObject());

            //添加煎制好的食材
            KitchenObject afterDecoction =  KitchenObjectPool.Instance.GetObjectFromPool(
                decoctionInformation.output.GetKitchenObjectSO(),
                GetPlacement().position,
                Quaternion.identity
                );
            SetKitchenObject(afterDecoction);

            decoctionListSO.TryGetDecoctionOutput(afterDecoction,out DecoctionInformation newDecoctionInformation);
            decoctionInformation = newDecoctionInformation;

            decoctionTimer = 0;
        }
    }
}
