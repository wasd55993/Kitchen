using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationControl : MonoBehaviour
{
    private const string IsMove = "isMove";

    //组件获取
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerControl moveControl;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimationControl();
    }

    private void AnimationControl()
    {
        animator.SetBool(IsMove,moveControl.IsMove);
    }
}
