
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[System.Serializable]
public struct Float01
{

    private float value;


    public Float01(float value)
    {
        this.value = Mathf.Clamp(value, 0, 1f);

    }

    public static implicit operator float(Float01 f)
    {
        return f.value;
    }
    public static implicit operator Float01(float f)
    {
        return new Float01(f);
    }


    public static Float01 operator +(Float01 f1, Float01 f2) => new Float01(f1.Value + f2.Value);
    public static Float01 operator +(Float01 f1, float f2) => new Float01(f1.Value + f2);
    public static Float01 operator +(float f1, Float01 f2) => new Float01(f1 + f2.Value);

    public static Float01 operator -(Float01 f1, Float01 f2) => new Float01(f1.Value - f2.Value);
    public static Float01 operator -(Float01 f1, float f2) => new Float01(f1.Value - f2);
    public static Float01 operator -(float f1, Float01 f2) => new Float01(f1 - f2.Value);
    /// <summary>Increases the value by 0.1f.</summary>
    public static Float01 operator ++(Float01 f1) => new Float01(f1.Value += 0.1f);
    /// <summary>Decreases the value by 0.1f.</summary>
    public static Float01 operator --(Float01 f1) => new Float01(f1.Value -= 0.1f);
    public static Float01 operator *(Float01 f1, Float01 f2) => new Float01(f1.Value * f2.Value);
    public static Float01 operator /(Float01 f1, Float01 f2) => new Float01(f1.Value / f2.Value);
    public static bool operator ==(Float01 f1, Float01 f2) => EqualityComparer<Float01>.Default.Equals(f1, f2);
    public static bool operator !=(Float01 f1, Float01 f2) => !(f1 == f2);
    public static bool operator >(Float01 f1, Float01 f2) => f1.Value > f2.Value;
    public static bool operator <(Float01 f1, Float01 f2) => f1.Value < f2.Value;
    public static bool operator >=(Float01 f1, Float01 f2) => f1.Value >= f2.Value;
    public static bool operator <=(Float01 f1, Float01 f2) => f1.Value <= f2.Value;
    public static bool operator true(Float01 f1) => f1.Value <= 1 && f1.Value > 0;
    public static bool operator false(Float01 f1) => f1.Value == 0;


    [ShowInInspector]
    public float Value { get => value; set => this.value = Mathf.Clamp(value, 0, 1f); }

    public override bool Equals(object obj)
    {

        var @float = (Float01)obj;
        return Value == @float.Value;
    }

    public override int GetHashCode()
    {
        return -1937169414 + Value.GetHashCode();
    }

    public override string ToString()
    {
        return value.ToString();
    }
}
