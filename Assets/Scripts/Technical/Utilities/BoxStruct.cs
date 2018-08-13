﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[System.Serializable]
public class BoxStruct<T>
{
    public T top;
    public T bottom;
    public T left;
    public T right;


    public BoxStruct(BoxStruct<T> other) : this(other.top, other.bottom, other.left, other.right)
    {

    }

    public BoxStruct(T top, T bottom, T left, T right)
    {
        this.top = top;
        this.bottom = bottom;
        this.left = left;
        this.right = right;
    }

    public T GetObjectFromDirection(Vector2Int direction)
    {
        direction = direction.Normalized();
        if (direction == Vector2Int.up)
        {
            return top;
        }
        else if (direction == Vector2Int.down)
        {
            return bottom;
        }
        else if (direction == Vector2Int.left)
        {
            return left;
        }
        else if (direction == Vector2Int.right)
        {
            return right;
        }

        return default(T);
    }

    public T[] GetValues()
    {

        return new T[4] { top, bottom, left, right };
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }



    public override string ToString()
    {
        return "Top: " + top.ToString() + ";\nBottom: " + bottom.ToString() + ";\nLeft: " + left.ToString() + ";\nRight: " + right.ToString();
    }

    public override int GetHashCode()
    {
        var hashCode = 1723369467;
        hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(top);
        hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(bottom);
        hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(left);
        hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(right);
        return hashCode;
    }
}
