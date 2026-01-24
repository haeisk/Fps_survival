using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Walk(bool Walk)
    {
        animator.SetBool(AnimationTags.WALK_PARAMETER, Walk);
    }
     public void Run(bool Run)
    {
        animator.SetBool(AnimationTags.RUN_PARAMETER, Run);
    }


    public void Attack(bool Attack)
    {
        animator.SetTrigger(AnimationTags.ATTACK_PARAMETER);
    }

    public void Die()
    {
        animator.SetTrigger(AnimationTags.DEAD_PARAMETER);
    }       

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
