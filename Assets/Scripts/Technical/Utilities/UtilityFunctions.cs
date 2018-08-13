using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;
using UnityEngine.SceneManagement;
[System.Serializable]
public enum LerpMode
{
    EaseIn,
    EaseOut,
    Exponential,
    Smoothstep,
    None

}

public static class UtilityFunctions
{
    #region Math
    public static float ClampMax(float value, float maxValue)
    {

        if (value > maxValue)
            value = maxValue;
        return value;
    }
    public static float ClampMin(float value, float minValue)
    {
        if (value < minValue)
            value = minValue;
        return value;
    }
    public static int ClampMax(int value, int maxValue)
    {
        if (value > maxValue)
            value = maxValue;
        return value;
    }
    public static int ClampMin(int value, int minValue)
    {
        if (value < minValue)
            value = minValue;
        return value;
    }

    public static float ClampAngle(float angle)
    {
        if (angle > 360)
        {

            while (angle > 360)
                angle -= 360;

        }
        if (angle < 0)
        {

            while (angle < 0)
                angle += 360;

        }

        return angle;
    }

    public static float GetAngle(this Vector2 v)
    {
        // normalize the vector: this makes the x and y components numerically
        // equal to the sine and cosine of the angle:
        v.Normalize();
        // get the basic angle:
        var ang = Mathf.Asin(v.y) * Mathf.Rad2Deg;
        // fix the angle for 2nd and 3rd quadrants:
        if (v.x < 0)
        {
            ang = 180 - ang;
        }
        else // fix the angle for 4th quadrant:
        if (v.y < 0)
        {
            ang = 360 + ang;
        }
        return ang;

    }

    public static float SmallerAngleDifference(float alpha, float beta)
    {

        float phi = Mathf.Abs(beta - alpha) % 360;       // This is either the distance or 360 - distance
        float distance = phi > 180 ? 360 - phi : phi;
        return distance;
    }
    /// <summary>
    /// Calculates the points of a circle.
    /// </summary>
    /// <param name="points"></param>
    /// <param name="radius"></param>
    /// <param name="positionReference"></param>
    /// <returns></returns>
    public static List<Vector2> CalculateCirclePoints(int points, Vector2? positionReference = null, float radius = 1f)
    {
        return CalculateCirclePoints(points, positionReference, radius, radius);
    }

    /// <summary>
    /// Calculates the points of a circle.
    /// </summary>
    /// <param name="points"></param>
    /// <param name="positionReference"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <returns></returns>
    public static List<Vector2> CalculateCirclePoints(int points, Vector2? positionReference = null, float width = 1f, float height = 1f)
    {
        float x;
        float y;
        points = ClampMin(points, 3);
        float angle = 0;
        float angleStep = (360f / points);

        List<Vector2> positions = new List<Vector2>();
        for (int i = 0; i < (points); i++)
        {
            y = Mathf.Sin(Mathf.Deg2Rad * angle) * width / 2f;
            x = Mathf.Cos(Mathf.Deg2Rad * angle) * height / 2f;

            var pos = new Vector2(x, y);

            if (positionReference != null)
                pos += positionReference ?? Vector2.zero;

            positions.Add(pos);

            angle += angleStep;
        }

        return positions;
    }

    /// <summary>
    /// Returns the factorial of the given number.
    /// </summary>
    /// <param name="number"></param>
    /// <returns></returns>
    public static int Factorial(this int number)
    {
        int result = 1;
        while (number != 1)
        {
            result = result * number;
            number = number - 1;
        }
        return result;
    }

    public static float ChangeToZero(this float f, float speed = 30f)
    {
        if (Mathf.Approximately(f, 0f))
            return 0;
        float changeValue = ClampMin(speed, 0f) * Time.deltaTime;
        if (f > 0)
        {

            if (f - changeValue < 0f)
                return 0f;
            return f - changeValue;
        }
        else
        {

            if (f + changeValue > 0f)
                return 0f;
            return f + changeValue;
        }
    }

    /// <summary>
    /// Returns the sign of the number.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int Sign(int value)
    {
        if (value >= 0)
            return 1;
        else
            return -1;
    }

    public static int SignZero(int value)
    {
        if (value > 0)
            return 1;
        else if (value < 0)
            return -1;
        else
            return 0;
    }

    /// <summary>
    /// Returns -1 or 1 at random.
    /// </summary>
    /// <returns></returns>
    public static float RandomSign()
    {
        return UnityEngine.Random.Range(0, 2) * 2 - 1;
    }

    /// <summary>
    /// Used to modify the "t" value in a lerp function.
    /// </summary>
    /// <param name="lerpMode"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static float ChangeLerpT(LerpMode lerpMode = LerpMode.EaseOut, float t = 0f)
    {
        switch (lerpMode)
        {
            case LerpMode.EaseIn:
                t = 1f - Mathf.Cos(t * Mathf.PI * 0.5f);
                break;
            case LerpMode.EaseOut:
                t = Mathf.Sin(t * Mathf.PI * 0.5f);
                break;
            case LerpMode.Exponential:
                t = t * t;
                break;
            case LerpMode.Smoothstep:
                t = t * t * t * (t * (6f * t - 15f) + 10f);
                break;
            case LerpMode.None:
                break;
            default:
                t = t * t * t * (t * (6f * t - 15f) + 10f);
                break;
        }
        return t;
    }

    public static float Map(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }

    private delegate float RoundingFunction(float value);

    private enum RoundingDirection { Up, Down }


    public static float RoundUp(float value, int precision)
    {
        return Round(value, precision, RoundingDirection.Up);
    }

    public static float RoundDown(float value, int precision)
    {
        return Round(value, precision, RoundingDirection.Down);
    }

    private static float Round(float value, int precision,
                RoundingDirection roundingDirection)
    {
        RoundingFunction roundingFunction;
        if (roundingDirection == RoundingDirection.Up)
            roundingFunction = Mathf.Ceil;
        else
            roundingFunction = Mathf.Floor;
        value *= Mathf.Pow(10, precision);
        value = roundingFunction(value);
        return value * Mathf.Pow(10, -1 * precision);
    }



    #endregion

    #region IEnumerables
    /// <summary>
    /// Returns a random element from a IEnumerable.
    /// </summary>
    /// <typeparam name="T">The object Type.</typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static T PickRandom<T>(this IEnumerable<T> source)
    {
        return source.PickRandom(1).Single();
    }

    /// <summary>
    /// Returns a IEnumerable with random elements from the source IEnumerable.
    /// </summary>
    /// <typeparam name="T">The object Type.</typeparam>
    /// <param name="source"></param>
    /// <param name="count">The amount of elements to pick.</param>
    /// <returns></returns>
    public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
    {
        return source.Shuffle().Take(count);
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
    {
        return source.OrderBy(x => Guid.NewGuid());
    }


    #endregion

    #region Lists
    /// <summary>
    /// Converts an array to a list.
    /// </summary>
    /// <typeparam name="T">The object Type.</typeparam>
    /// <param name="array">The array to convert to a list.</param>
    /// <returns></returns>
    public static List<T> ToList<T>(this T[] array)
    {
        List<T> result = new List<T>();
        foreach (T t in array)
        {
            result.Add(t);
        }
        return result;
    }

    public static List<T> ToNonNullList<T>(this IEnumerable<T> obj)
    {
        return obj == null ? new List<T>() : obj.ToList();
    }
    /// <summary>
    /// Prints all elements on the list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="l"></param>
    public static void PrintAll<T>(this List<T> l)
    {
        if (l != null)
            foreach (var item in l)
            {
                Debug.Log(item);
            }
    }

    /// <summary>
    /// Fills a list with the contents of another list, without increasing or decreasing the size.
    /// </summary>
    /// <typeparam name="T">The object Type.</typeparam>
    /// <param name="l">The list to fill.</param>
    /// <param name="other">The list that will be used to fill.</param>
    /// <returns></returns>
    public static List<T> FillList<T>(this List<T> l, List<T> other)
    {
        if (l == null || other == null)
            return l;
        for (int i = 0; i < l.Count; i++)
        {
            if (other.Count - 1 < i)
                break;
            l[i] = other[i];
        }

        return l;
    }

    public static void TryAddRange<T>(this List<T> l, List<T> other)
    {
        if (other != null)
            l.AddRange(other);
    }


    /// <summary>
    /// Is the list empty?
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static bool IsEmpty<T>(this List<T> list)
    {
        return list?.Count == 0;
    }

    public static bool ValidIndex<T>(this List<T> list, int index)
    {
        return (index >= 0 && index < list.Count);
    }

    #endregion

    #region Arrays
    /// <summary>
    /// Prints all elements on the array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    public static void PrintAll<T>(this T[] array)
    {
        if (array != null)
            foreach (var item in array)
            {
                Debug.Log(item);
            }
    }
    #endregion

    #region 2D Arrays
    /// <summary>
    /// Returns a list with all the items of the two-dimensional array.
    /// </summary>
    /// <typeparam name="T">The object Type.</typeparam>
    /// <param name="twoDimensionalArray"></param>
    /// <returns></returns>
    public static List<T> GetItems<T>(this T[,] twoDimensionalArray)
    {
        if (twoDimensionalArray == null)
            return null;
        List<T> result = new List<T>();

        for (int i = 0; i < twoDimensionalArray.GetLength(0); i++)
        {
            for (int j = 0; j < twoDimensionalArray.GetLength(1); j++)
            {
                result.Add(twoDimensionalArray[i, j]);
            }
        }
        return result;
    }



    /// <summary>
    /// Checks if the (x,y) coordinates is valid in the two-dimensional array.
    /// </summary>
    /// <typeparam name="T">The object Type.</typeparam>
    /// <param name="twoDimensionalArray"></param>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate</param>
    /// <returns></returns>
    public static bool ValidCoordinates<T>(this T[,] twoDimensionalArray, int x, int y)
    {
        if (twoDimensionalArray == null)
            return false;

        return (x >= 0 && x < twoDimensionalArray.GetLength(0) &&
                y >= 0 && y < twoDimensionalArray.GetLength(1));
    }
    /// <summary>
    /// Checks if the (x,y) coordinates is valid in the two-dimensional array.
    /// </summary>
    /// <typeparam name="T">The object Type.</typeparam>
    /// <param name="twoDimensionalArray"></param>
    /// <param name="x">X coordinate.</param>
    /// <param name="y">Y coordinate</param>
    /// <returns></returns>
    public static bool ValidCoordinates<T>(this T[,] twoDimensionalArray, Position pos)
    {
        return twoDimensionalArray.ValidCoordinates(pos.x, pos.y);
    }

    /// <summary>
    /// Rotates a two-dimensional array by +90.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static T[,] RotateMatrix90R<T>(this T[,] matrix)
    {
        T[,] ret = new T[matrix.GetLength(0), matrix.GetLength(1)];

        for (int i = 0; i < matrix.GetLength(0); ++i)
        {
            for (int j = 0; j < matrix.GetLength(1); ++j)
            {
                ret[i, j] = matrix[matrix.GetLength(0) - j - 1, i];
            }
        }

        return ret;
    }

    /// <summary>
    /// Rotates a two-dimensional array by -90.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static T[,] RotateMatrix90L<T>(this T[,] matrix)
    {

        T[,] ret = new T[matrix.GetLength(0), matrix.GetLength(1)];

        for (int i = 0; i < matrix.GetLength(0); ++i)
        {
            for (int j = 0; j < matrix.GetLength(1); ++j)
            {
                ret[i, j] = matrix[j, matrix.GetLength(1) - i - 1];
            }
        }

        return ret;
    }
    /// <summary>
    /// Rotates a two-dimensional array by 180.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static T[,] Reverse<T>(this T[,] matrix)
    {

        return matrix.RotateMatrix90R().RotateMatrix90R();
    }
    public static bool Check2DArray<T>(this T[,] data, T[,] find) where T : class
    {
        if (data == null || find == null)
            return false;
        int dataLen = data.Length; // length of the whole data
        int findLen = find.Length; // length of the whole find

        for (int i = 0; i < dataLen; i++) // iterate through data
        {
            int dataX = i % data.GetLength(0); // get current column index
            int dataY = i / data.GetLength(0); // get current row index

            bool okay = true; // declare result placeholder for that check
            for (int j = 0; j < findLen && okay; j++) // iterate through find
            {
                int findX = j % find.GetLength(1); // current column in find
                int findY = j / find.GetLength(1); // current row in find

                int checkedX = findX + dataX; // column index in data to check
                int checkedY = findY + dataY; // row index in data to check

                // check if checked index is not going outside of the data boundries 
                if (checkedX >= data.GetLength(0) || checkedY >= data.GetLength(1))
                {
                    // we are outside of the data boundries
                    // set flag to false and break checks for this data row and column
                    okay = false;
                    break;
                }

                // we are still inside of the data boundries so check if values matches
                okay = data[dataY + findY, dataX + findX] == find[findY, findX]; // check if it matches
            }
            if (okay) // if all values from both fragments are equal
                return true; // return true

        }
        return false;
    }
    #endregion

    #region Vectors
    public static Vector3 RoundVector3(Vector3 v)
    {
        return new Vector3(Mathf.Round(v.x), Mathf.Round(v.y), Mathf.Round(v.z));
    }

    public static Vector2 RoundVector2(Vector2 v)
    {
        return new Vector2(Mathf.Round(v.x), Mathf.Round(v.y));
    }

    public static Vector3 ClampToBounds(this Vector3 v, Bounds b)
    {
        Vector3 pos = new Vector3(v.x, v.y, v.z);

        pos.x = Mathf.Clamp(pos.x, b.min.x, b.max.x);

        pos.y = Mathf.Clamp(pos.y, b.min.y, b.max.y);

        pos.z = Mathf.Clamp(pos.z, b.min.z, b.max.z);

        return pos;
    }

    public static Vector2Int Normalized(this Vector2Int v)
    {
        return new Vector2Int(Sign(v.x), Sign(v.y));
    }
    public static Vector2Int NormalizedZero(this Vector2Int v)
    {
        return new Vector2Int(SignZero(v.x), SignZero(v.y));
    }

    public static Vector2 GetAngleDirection(this float degreesAngle)
    {
        return new Vector2(Mathf.Sin(degreesAngle), Mathf.Cos(degreesAngle));
    }
    #endregion

    #region GameObject
    /// <summary>
    /// Tries to get a component in GameObject, adds new one if not found.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="g"></param>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this GameObject g) where T : Component
    {
        if (g.GetComponent<T>() == null)
            return g.AddComponent<T>();
        else
            return g.GetComponent<T>();
    }


    /// <summary>
    /// Checks if a component is present on GameObject.
    /// </summary>
    /// <typeparam name="T">The component type.</typeparam>
    /// <param name="g"></param>
    /// <returns></returns>
    public static bool CheckForComponent<T>(this GameObject g) where T : Component
    {
        bool hasComponent = false;
        foreach (Component comp in g.GetComponents<Component>())
        {

            if (comp is T)
            {
                hasComponent = true;
                break;
            }
        }

        return hasComponent;
    }

    /// <summary>
    /// Sets the GameObject's scale.
    /// </summary>
    /// <param name="g"></param>
    /// <param name="size"></param>
    public static void SetScale(this GameObject g, float size)
    {
        g.transform.localScale = new Vector3(size, size, size);
    }

    public static void DestroySelf(this UnityEngine.Object c)
    {
        UnityEngine.Object.Destroy(c);
    }

    /// <summary>
    /// Deactivates the GameObject.
    /// </summary>
    /// <param name="g"></param>
    public static void Deactivate(this GameObject g)
    {
        g.SetActive(false);
    }

    /// <summary>
    /// Activates the GameObject.
    /// </summary>
    /// <param name="g"></param>
    public static void Activate(this GameObject g)
    {
        g.SetActive(true);
    }
    /// <summary>
    /// Use this method to get all loaded objects of some type, including inactive objects. 
    /// </summary>
    public static List<T> FindObjectsOfTypeAll<T>()
    {
        List<T> results = new List<T>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var s = SceneManager.GetSceneAt(i);
            if (s.isLoaded)
            {
                var allGameObjects = s.GetRootGameObjects();
                for (int j = 0; j < allGameObjects.Length; j++)
                {
                    var go = allGameObjects[j];
                    results.AddRange(go.GetComponentsInChildren<T>(true));
                }
            }
        }
        return results;
    }

    /// <summary>
    /// Destroys all child objects on transform.
    /// </summary>
    /// <param name="parent"></param>
    public static void DestroyChildren(this Transform parent, bool destroyInInspector = false)
    {
        List<GameObject> destroyList = new List<GameObject>();
        foreach (Transform child in parent.transform)
        {
            destroyList.Add(child.gameObject);
        }

        for (int i = destroyList.Count - 1; i >= 0; i--)
        {
            if (Application.isEditor && destroyInInspector)
                UnityEngine.Object.DestroyImmediate(destroyList[i]);
            else
                UnityEngine.Object.Destroy(destroyList[i]);
        }

    }
    /// <summary>
    /// Destroys all child objects on transform that have the component.
    /// </summary>
    /// <param name="parent"></param>
    public static void DestroyChildren<T>(this Transform parent, bool destroyInInspector = false) where T : Component
    {
        List<GameObject> destroyList = new List<GameObject>();
        foreach (Transform child in parent.transform)
        {
            if (child.gameObject.CheckForComponent<T>())
                destroyList.Add(child.gameObject);
        }

        for (int i = destroyList.Count - 1; i >= 0; i--)
        {
            if (Application.isEditor && destroyInInspector)
                UnityEngine.Object.DestroyImmediate(destroyList[i]);
            else
                UnityEngine.Object.Destroy(destroyList[i]);
        }

    }
    /// <summary>
    /// Changes the GameObject's scale without affecting the children.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="scale"></param>
    public static void ChangeParentScale(this Transform parent, Vector3 scale)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
        {
            child.parent = null;
            children.Add(child);
        }
        parent.localScale = scale;
        foreach (Transform child in children) child.parent = parent;
    }
    #endregion

    #region String
    public static string RemoveLineEndings(this string value)
    {
        if (String.IsNullOrEmpty(value))
        {
            return value;
        }
        string lineSeparator = ((char)0x2028).ToString();
        string paragraphSeparator = ((char)0x2029).ToString();

        return value.Replace("\r\n", string.Empty)
                    .Replace("\n", string.Empty)
                    .Replace("\r", string.Empty)
                    .Replace(lineSeparator, string.Empty)
                    .Replace(paragraphSeparator, string.Empty)
                    .Replace("\u200B", "");
    }

    public static bool ContainsIgnoreCase(this List<string> l, string s)
    {
        if (l == null)
            return false;
        for (int i = 0; i < l.Count; i++)
        {
            if (l[i].ToLower().Trim().RemoveZeroWidthSpace() == s.ToLower().Trim().RemoveZeroWidthSpace())
                return true;
        }

        return false;
    }

    public static string FirstCharToUpper(this string input)
    {
        switch (input)
        {
            case null: throw new ArgumentNullException(nameof(input));
            case "": throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input));
            default: return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }

    public static string RemoveZeroWidthSpace(this string value)
    {
        if (String.IsNullOrEmpty(value))
        {
            return value;
        }

        return value.Replace("\u200B", "");
    }
    #endregion

    #region Events and UI

    /// <summary>
    /// The width of the screen.
    /// </summary>
    public static float ScreenWidth
    {
        get
        {
#if UNITY_EDITOR

            var screenSize = Handles.GetMainGameViewSize();
            return ScreenHeight * screenSize.x / screenSize.y;

#else
                return ScreenHeight * Screen.width / Screen.height;
#endif
        }
    }
    /// <summary>
    /// The height of the screen.
    /// </summary>
    public static float ScreenHeight
    {
        get
        {
            return Camera.main.orthographicSize * 2.0f;
        }
    }

    /// <summary>
    /// Removes and adds a listener.
    /// </summary>
    /// <param name="y"></param>
    /// <param name="listener"></param>
    public static void RemoveAndAddListener(this UnityEvent y, UnityAction listener)
    {
        y.RemoveListener(listener);
        y.AddListener(listener);
    }
    /// <summary>
    /// Removes and adds a listener.
    /// </summary>
    /// <param name="y"></param>
    /// <param name="listener"></param>
    public static void RemoveAndAddListener<T>(this UnityEvent<T> y, UnityAction<T> listener)
    {
        y.RemoveListener(listener);
        y.AddListener(listener);
    }
    /// <summary>
    /// Removes and adds a listener.
    /// </summary>
    /// <param name="y"></param>
    /// <param name="listener"></param>
    public static void RemoveAndAddListener<T1, T2>(this UnityEvent<T1, T2> y, UnityAction<T1, T2> listener)
    {
        y.RemoveListener(listener);
        y.AddListener(listener);
    }/// <summary>
     /// Removes and adds a listener.
     /// </summary>
     /// <param name="y"></param>
     /// <param name="listener"></param>
    public static void RemoveAndAddListener<T1, T2, T3>(this UnityEvent<T1, T2, T3> y, UnityAction<T1, T2, T3> listener)
    {
        y.RemoveListener(listener);
        y.AddListener(listener);
    }
    /// <summary>
    /// Checks if the pointer was over a UI element.
    /// </summary>
    /// <returns></returns>
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    #endregion

    #region Extras
    /// <summary>
    /// Draws bounds in a DrawGizmos function.
    /// </summary>
    /// <param name="b"></param>
    public static void GizmosDrawBounds(Bounds b, Color? gizmosColor = null)
    {
        Gizmos.color = gizmosColor ?? Color.green;
        Gizmos.DrawWireCube(b.center, b.size);
    }

    /// <summary>
    /// Draws a circle in a DrawGizmos function.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="radius"></param>
    /// <param name="points"></param>
    /// <param name="gizmosColor"></param>
    public static void GizmosDrawCircle(Vector2 pos, float radius = 1f, int points = 30, Color? gizmosColor = null)
    {
        GizmosDrawCircle(pos, radius, radius, points, gizmosColor);
    }/// <summary>
     /// Draws a circle in a DrawGizmos function.
     /// </summary>
     /// <param name="pos"></param>
     /// <param name="radius"></param>
     /// <param name="points"></param>
     /// <param name="gizmosColor"></param>
    public static void GizmosDrawCircle(Vector2 pos, float width = 1f, float height = 1f, int points = 30, Color? gizmosColor = null)
    {
        Gizmos.color = gizmosColor ?? Color.green;
        var circlePoints = CalculateCirclePoints(points, pos, width, height);

        var current = circlePoints[0];

        for (int i = 1; i < circlePoints.Count; i++)
        {
            Gizmos.DrawLine(current, circlePoints[i]);
            current = circlePoints[i];
        }

        Gizmos.DrawLine(circlePoints[circlePoints.Count - 1], circlePoints[0]);
    }

    /// <summary>
    /// Draws a cross in a DrawGizmos function.
    /// </summary>
    /// <param name="pos"></param>
    /// <param name="size"></param>
    /// <param name="gizmosColor"></param>
    public static void GizmosDrawCross(Vector2 pos, float size = 1f, Color? gizmosColor = null)
    {
        Gizmos.color = gizmosColor ?? Color.green;
        Gizmos.DrawLine(new Vector3(pos.x, pos.y - size / 2f, 0f), new Vector3(pos.x, pos.y + size / 2f));
        Gizmos.DrawLine(new Vector3(pos.x - size / 2f, pos.y, 0f), new Vector3(pos.x + size / 2f, pos.y));
    }

    /// <summary>
    /// Swaps two references.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public static void Swap<T>(ref T a, ref T b)
    {
        T temp = a;
        a = b;
        b = temp;
    }

    /// <summary>
    /// Returns a random point inside this bounds.
    /// </summary>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Vector3 RandomPoint(this Bounds b)
    {

        Vector3 randomPos = new Vector3(UnityEngine.Random.Range(b.min.x, b.max.x),
                                        UnityEngine.Random.Range(b.min.y, b.max.y),
                                        UnityEngine.Random.Range(b.min.z, b.max.z));

        return randomPos;
    }

    public static T RandomEnumValue<T>()
    {
        try
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(new System.Random().Next(v.Length));
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
        return default(T);
    }

    #endregion

}