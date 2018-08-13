using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Check2DCollisions
{
    /// <summary>
    /// Checks up collisions on BoxCollider2D.
    /// </summary>
    /// <param name="boxCol">Collider to check collision.</param>
    /// <param name="rayCastCount">Quantity of raycasts. (Min: 1)</param>
    /// <param name="edgeOffSet">The distance from the edges of the collider.</param>
    /// <param name="distance">Distance to check for collision.</param>
    /// <returns></returns>
    public static List<GameObject> CheckCollisionUp(BoxCollider2D boxCol, int rayCastCount = 5, float edgeOffSet = 0.05f, float distance = 0.03f)
    {
        List<GameObject> result = new List<GameObject>();
        if (boxCol)
        {

            rayCastCount = Mathf.Clamp(rayCastCount, 1, 99);
            distance = Mathf.Clamp(distance, 0f, 99f);
            edgeOffSet = Mathf.Clamp(edgeOffSet, 0, boxCol.size.x / 2f);
            boxCol.offset = Vector2.zero;
            float width = boxCol.size.x - edgeOffSet * 2;
            float height = boxCol.size.y - 0.01f;
            if (rayCastCount == 1)
            {
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(boxCol.gameObject.transform.position.x, boxCol.gameObject.transform.position.y + height / 2), Vector2.up, distance);
                Debug.DrawRay(new Vector2(boxCol.gameObject.transform.position.x, boxCol.gameObject.transform.position.y + height / 2), new Vector2(0, distance), Color.red, 0.05f);
                if (hit.collider != null)
                {
                    result.Add(hit.collider.gameObject);

                }
            }
            else
            {
                List<RaycastHit2D> hits = new List<RaycastHit2D>();
                float division = width / (rayCastCount - 1);
                for (int i = 0; i < rayCastCount; i++)
                {
                    Vector2 origin = new Vector2(((boxCol.gameObject.transform.position.x - width / 2f) + division * i), boxCol.gameObject.transform.position.y + height / 2);
                    hits.Add(Physics2D.Raycast(origin, Vector2.up, distance));
                    Debug.DrawRay(origin, new Vector2(0, distance), Color.red, 0.05f);
                }
                for (int i = 0; i < hits.Count; i++)
                {
                    if (hits[i].collider != null)
                    {
                        if (!result.Contains(hits[i].collider.gameObject))
                        {
                            result.Add(hits[i].collider.gameObject);
                        }


                    }
                }
            }
            return result;
        }

        else
        {
            return result;
        }
    }
    /// <summary>
    /// Checks down collisions on BoxCollider2D.
    /// </summary>
    /// <param name="boxCol">Collider to check collision.</param>
    /// <param name="rayCastCount">Quantity of raycasts. (Min: 1)</param>
    /// <param name="edgeOffSet">The distance from the edges of the collider.</param>
    /// <param name="distance">Distance to check for collision.</param>
    /// <returns></returns>
    public static List<GameObject> CheckCollisionDown(BoxCollider2D boxCol, int rayCastCount = 5, float edgeOffSet = 0.05f, float distance = 0.03f)
    {
        List<GameObject> result = new List<GameObject>();
        if (boxCol)
        {

            rayCastCount = Mathf.Clamp(rayCastCount, 1, 99);
            distance = Mathf.Clamp(distance, 0f, 99f);
            edgeOffSet = Mathf.Clamp(edgeOffSet, 0, boxCol.size.x / 2f);
            boxCol.offset = Vector2.zero;
            float width = boxCol.size.x - edgeOffSet * 2;
            float height = boxCol.size.y - 0.01f;
            if (rayCastCount == 1)
            {
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(boxCol.gameObject.transform.position.x, boxCol.gameObject.transform.position.y - height / 2), Vector2.down, distance);
                Debug.DrawRay(new Vector2(boxCol.gameObject.transform.position.x, boxCol.gameObject.transform.position.y - height / 2), new Vector2(0, -distance), Color.red, 0.05f);
                if (hit.collider != null)
                {
                    result.Add(hit.collider.gameObject);

                }
            }
            else
            {
                List<RaycastHit2D> hits = new List<RaycastHit2D>();
                float division = width / (rayCastCount - 1);
                for (int i = 0; i < rayCastCount; i++)
                {
                    Vector2 origin = new Vector2(((boxCol.gameObject.transform.position.x - width / 2f) + division * i), boxCol.gameObject.transform.position.y - height / 2);
                    hits.Add(Physics2D.Raycast(origin, Vector2.down, distance));
                    Debug.DrawRay(origin, new Vector2(0, -distance), Color.red, 0.05f);
                }
                for (int i = 0; i < hits.Count; i++)
                {
                    if (hits[i].collider != null)
                    {
                        if (!result.Contains(hits[i].collider.gameObject))
                        {
                            result.Add(hits[i].collider.gameObject);
                        }


                    }
                }
            }
            return result;
        }

        else
        {
            return result;
        }
    }
    /// <summary>
    /// Checks right collisions on BoxCollider2D.
    /// </summary>
    /// <param name="boxCol">Collider to check collision.</param>
    /// <param name="rayCastCount">Quantity of raycasts. (Min: 1)</param>
    /// <param name="edgeOffSet">The distance from the edges of the collider.</param>
    /// <param name="distance">Distance to check for collision.</param>
    /// <returns></returns>
    public static List<GameObject> CheckCollisionRight(BoxCollider2D boxCol, int rayCastCount = 5, float edgeOffSet = 0.05f, float distance = 0.03f)
    {
        List<GameObject> result = new List<GameObject>();
        if (boxCol)
        {

            rayCastCount = Mathf.Clamp(rayCastCount, 1, 99);
            distance = Mathf.Clamp(distance, 0f, 99f);
            edgeOffSet = Mathf.Clamp(edgeOffSet, 0, boxCol.size.x / 2f);
            boxCol.offset = Vector2.zero;
            float width = boxCol.size.x - 0.01f;
            float height = boxCol.size.y - edgeOffSet * 2;
            if (rayCastCount == 1)
            {
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(boxCol.gameObject.transform.position.x + width / 2, boxCol.gameObject.transform.position.y), Vector2.right, distance);
                Debug.DrawRay(new Vector2(boxCol.gameObject.transform.position.x - width / 2, boxCol.gameObject.transform.position.y), new Vector2(distance, 0), Color.red, 0.05f);
                if (hit.collider != null)
                {
                    result.Add(hit.collider.gameObject);

                }
            }
            else
            {
                List<RaycastHit2D> hits = new List<RaycastHit2D>();
                float division = height / (rayCastCount - 1);
                for (int i = 0; i < rayCastCount; i++)
                {
                    Vector2 origin = new Vector2(boxCol.gameObject.transform.position.x + width / 2, (boxCol.gameObject.transform.position.y - height / 2f) + division * i);
                    hits.Add(Physics2D.Raycast(origin, Vector2.right, distance));
                    Debug.DrawRay(origin, new Vector2(distance, 0), Color.red, 0.05f);
                }
                for (int i = 0; i < hits.Count; i++)
                {
                    if (hits[i].collider != null)
                    {
                        if (!result.Contains(hits[i].collider.gameObject))
                        {
                            result.Add(hits[i].collider.gameObject);
                        }


                    }
                }
            }
            return result;
        }

        else
        {
            return result;
        }
    }
    /// <summary>
    /// Checks left collisions on BoxCollider2D.
    /// </summary>
    /// <param name="boxCol">Collider to check collision.</param>
    /// <param name="rayCastCount">Quantity of raycasts. (Min: 1)</param>
    /// <param name="edgeOffSet">The distance from the edges of the collider.</param>
    /// <param name="distance">Distance to check for collision.</param>
    /// <returns></returns>
    public static List<GameObject> CheckCollisionLeft(BoxCollider2D boxCol, int rayCastCount = 5, float edgeOffSet = 0.05f, float distance = 0.03f)
    {
        List<GameObject> result = new List<GameObject>();
        if (boxCol)
        {

            rayCastCount = Mathf.Clamp(rayCastCount, 1, 99);
            distance = Mathf.Clamp(distance, 0f, 99f);
            edgeOffSet = Mathf.Clamp(edgeOffSet, 0, boxCol.size.x / 2f);
            boxCol.offset = Vector2.zero;
            float width = boxCol.size.x - 0.01f;
            float height = boxCol.size.y - edgeOffSet * 2;
            if (rayCastCount == 1)
            {
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(boxCol.gameObject.transform.position.x - width / 2, boxCol.gameObject.transform.position.y), Vector2.left, distance);
                Debug.DrawRay(new Vector2(boxCol.gameObject.transform.position.x - width / 2, boxCol.gameObject.transform.position.y), new Vector2(-distance, 0), Color.red, 0.05f);
                if (hit.collider != null)
                {
                    result.Add(hit.collider.gameObject);

                }
            }
            else
            {
                List<RaycastHit2D> hits = new List<RaycastHit2D>();
                float division = height / (rayCastCount - 1);
                for (int i = 0; i < rayCastCount; i++)
                {
                    Vector2 origin = new Vector2(boxCol.gameObject.transform.position.x - width / 2, (boxCol.gameObject.transform.position.y - height / 2f) + division * i);
                    hits.Add(Physics2D.Raycast(origin, Vector2.left, distance));
                    Debug.DrawRay(origin, new Vector2(-distance, 0), Color.red, 0.05f);
                }
                for (int i = 0; i < hits.Count; i++)
                {
                    if (hits[i].collider != null)
                    {
                        if (!result.Contains(hits[i].collider.gameObject))
                        {
                            result.Add(hits[i].collider.gameObject);
                        }


                    }
                }
            }
            return result;
        }

        else
        {
            return result;
        }
    }

    ///<summary>
    ///This function uses raycasts to check a square area in a world position 
    ///</summary>
    ///<param name="point">Vector2 world position to check </param>
    ///<param name="width">Width of the square </param>
    ///<param name="height">Height of the square </param>
    ///<param name="linesQuantity">Number of lines to be cast horizontaly and verticaly </param>
    public static bool CheckSquare2D(Vector2 point, float width = 1f, float height = 1f, int linesQuantity = 10)
    {
        bool result = false;
        if (linesQuantity <= 0)
            linesQuantity = 1;
        RaycastHit2D[] hits = new RaycastHit2D[linesQuantity];

        if (linesQuantity == 1)
        {
            Debug.DrawRay(new Vector2(point.x - width / 2, point.y), Vector2.right, Color.red);
            hits[0] = Physics2D.Raycast(new Vector2(point.x - width / 2, point.y), Vector2.right, width);
            if (hits[0])
                result = true;
        }
        else
        {
            float lineSpacing = height / (linesQuantity - 1);
            hits[0] = Physics2D.Raycast(new Vector2(point.x - width / 2, point.y - height / 2), Vector2.right, width);
            Debug.DrawRay(new Vector2(point.x - width / 2, point.y - height / 2), Vector2.right * width, Color.red);
            for (int i = 1; i <= linesQuantity - 2; i++)
            {
                Debug.DrawRay(new Vector2(point.x - width / 2, (point.y - height / 2) + lineSpacing * i), Vector2.right * width, Color.red);
                hits[i] = Physics2D.Raycast(new Vector2(point.x - width / 2, (point.y - height / 2) + lineSpacing * i), Vector2.right, width);
            }
            Debug.DrawRay(new Vector2(point.x - width / 2, point.y + height / 2), Vector2.right * width, Color.red);
            hits[linesQuantity - 1] = Physics2D.Raycast(new Vector2(point.x - width / 2, point.y + height / 2), Vector2.right, width);
        }
        for (int i = 0; i < linesQuantity; i++)
        {
            if (hits[i])
            {
                result = true;
                break;
            }

        }
        return result;
    }
}
