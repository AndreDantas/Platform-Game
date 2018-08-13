using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : StuckOnHit
{

    Animator anim;
    public float animSpeed = 1f;
    public float stuckDepth = 0.2f;
    protected override void Awake()
    {
        anim = GetComponent<Animator>();
        base.Awake();
        //rb.AddForce(transform.right * 10f, ForceMode2D.Impulse);
    }


    private void Update()
    {
        if (!stuck)
        {
            if (rb)
            {
                this.angle = ConvertDirectionToAngle(rb.velocity);
                TurnArrow();
            }
        }
    }

    protected override void OnStuck(Collision2D collision)
    {
        StuckAnimation();
        if (rb)
        {

            Vector3 dir = rb.velocity.normalized;
            transform.position += dir * stuckDepth;
        }
    }


    protected override void OnStuck(Collider2D col)
    {
        StuckAnimation();
        if (rb)
        {
            Vector3 dir = rb.velocity.normalized;
            transform.position += dir * stuckDepth;
        }
    }

    protected virtual void TurnArrow()
    {
        this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, this.transform.eulerAngles.y, angle + angleOffset);
    }

    public override void Shoot(Vector2 dir)
    {
        base.Shoot(dir);
    }

    void StuckAnimation()
    {
        if (anim)
        {
            anim.speed = animSpeed;
            anim.Play("ArrowStuck");
        }
    }
}
