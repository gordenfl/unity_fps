using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Walk(bool walk)
    {
        anim.SetBool(AnimationTags.WALK_PARAMETER, walk);
    }

    public void Run(bool run)
    {
        anim.SetBool(AnimationTags.RUN_PARAMETER, run);
    }
    public void Attack()
    {
        anim.SetTrigger(AnimationTags.ATTACK_PARAMETER);
    }

    public void Dead()
    {
        anim.SetTrigger(AnimationTags.DEAD_PARAMETER);
    }
}
