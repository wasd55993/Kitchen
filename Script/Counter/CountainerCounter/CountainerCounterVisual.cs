using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountainerCounterVisual : MonoBehaviour
{
    //获取动画播放组件
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnimator()
    {
        animator.SetTrigger("openTrigger");
    }
}
