using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Projectile : MonoBehaviour
{
    public Faction faction;
    public float damage = 1f;
    [HideInInspector]
    public Rigidbody2D rb;
    public LayerMask ignore;
    public float speed = 15f;
    [SerializeField]
    protected float _angle;
    public float angle
    {
        get
        {
            return _angle;
        }

        set
        {
            _angle = value;
            _angle = ClampAngle(_angle);
        }
    }
    public float angleOffset = 0f;
    public float projectileGravity = 1f;
    // Use this for initialization
    protected virtual void Awake()
    {
        if (!rb)
            rb = GetComponent<Rigidbody2D>();
    }

    public virtual void Shoot(Vector2 dir)
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(dir.normalized * speed, ForceMode2D.Impulse);
        }

    }
    public virtual void Shoot(Vector2 dir, float force = 15f)
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);
        }

    }
    public virtual void Shoot(Vector2 dir, float force, Faction.Type faction)
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);
            this.faction.value = faction;
        }

    }
    public virtual void Shoot(Vector2 dir, float force, Faction.Type faction, float damage = 1f)
    {
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(dir.normalized * force, ForceMode2D.Impulse);
            this.faction.value = faction;
            this.damage = damage;
        }

    }

    public bool CheckLayer(int layer)
    {

        if (((1 << layer) & ignore) == 0)
        {
            return false;
        }
        else
            return true;
    }


    protected virtual void OnCollisionEnter2D(Collision2D col)
    {
        var hitbox = col.gameObject.GetComponent<Hitbox>();

        if (hitbox && hitbox.parent?.faction.value != faction.value)
        {
            hitbox.parent?.Damage(damage);
            Destroy(gameObject);
        }
    }
    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        var hitbox = col.gameObject.GetComponent<Hitbox>();

        if (hitbox && hitbox.parent?.faction.value != faction.value)
        {
            hitbox.parent?.Damage(damage);
            Destroy(gameObject);
        }
    }



    /// <summary>
    /// Receive a angle in degrees and returns the Vector 2 direction.
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public static Vector2 ConvertAngleToDirection(float angle)
    {
        Vector2 result;

        angle = ClampAngle(angle);

        float angleRad = angle * Mathf.Deg2Rad;

        result = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));

        return result;
    }

    /// <summary>
    /// Receives a Vector 2 and returns the angle in degrees.
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public static float ConvertDirectionToAngle(Vector2 direction)
    {
        Vector2 normalized = direction.normalized;
        float angle = 0;
        //0 - 180
        if (1 >= normalized.y && normalized.y >= 0)
            angle = (Mathf.Acos(normalized.x) * 180) / Mathf.PI;
        //180 - 270
        else if (-1 < normalized.x && normalized.x < 0)
            angle = ((Mathf.Abs(Mathf.Asin(normalized.y) * 180))) / Mathf.PI + 180;
        //270 - 360 
        else
            angle = ((Mathf.Abs(Mathf.Acos(-normalized.x) * 180))) / Mathf.PI + 180;
        return angle;
    }

    public static float ClampAngle(float angle)
    {

        while (angle < 0)
            angle += 360f;
        while (angle > 360)
            angle -= 360f;

        return angle;
    }
    //GET ANIMATION TIME 
    //AnimatorStateInfo animationState = myAnimator.GetCurrentAnimatorStateInfo(0);
    //AnimatorClipInfo[] myAnimatorClip = myAnimator.GetCurrentAnimatorClipInfo(0);
    //float myTime = myAnimatorClip[0].clip.length * animationState.normalizedTime;
}
