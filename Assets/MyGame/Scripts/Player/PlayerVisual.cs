using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private Player player;
    private Animator animator;

    private const string IS_WALKING = "Walk";
    private const string IS_IDLE = "Idle";



    private void Start()
    {
        player = GetComponentInParent<Player>();
        animator = GetComponent<Animator>();

        player.OnMoveChanged += Player_OnMoveChanged;
    }

    private void Player_OnMoveChanged(object sender, System.EventArgs e)
    {
        if (!player.IsWalking())
        {
            animator.CrossFade(IS_WALKING, 0f);
        }
        else
        {
            animator.CrossFade(IS_IDLE, 0.5f);
        }
    }

    //private void Update()
    //{
    //    string current = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    //    if (player.IsWalking())
    //    {
    //        if (!current.Equals(IS_WALKING))
    //            animator.CrossFade(IS_WALKING, 0f);
    //    }
    //    else
    //    {
    //        if (!current.Equals(IS_IDLE))
    //            animator.CrossFade(IS_IDLE, 0.01f);
    //    }
    //    Debug.Log(current);
    //}

}
