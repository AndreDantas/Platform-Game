using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Check2DCollisions
{
    #region Side Collisions
    /// <summary>
    /// Checks up collisions on BoxCollider2D.
    /// </summary>
    /// <param name="boxCol">Collider to check collision.</param>
    /// <param name="rayCastCount">Quantity of raycasts. (Min: 1)</param>
    /// <param name="edgeOffSet">The distance from the edges of the collider.</param>
    /// <param name="distance">Distance to check for collision.</param>
    /// <returns></returns>
    public static List<GameObject> CheckCollisionUp(BoxCollider2D boxCol, int rayCastCount = 5, float edgeOffSet = 0.05f, float distance = 0.03f, int layerMask = ~0)
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
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(boxCol.gameObject.transform.position.x, boxCol.gameObject.transform.position.y + height / 2), Vector2.up, distance, layerMask);
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
                    hits.Add(Physics2D.Raycast(origin, Vector2.up, distance, layerMask));
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
    public static List<GameObject> CheckCollisionDown(BoxCollider2D boxCol, int rayCastCount = 5, float edgeOffSet = 0.05f, float distance = 0.03f, int layerMask = ~0)
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
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(boxCol.gameObject.transform.position.x, boxCol.gameObject.transform.position.y - height / 2), Vector2.down, distance, layerMask);
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
                    hits.Add(Physics2D.Raycast(origin, Vector2.down, distance, layerMask));
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
    public static List<GameObject> CheckCollisionRight(BoxCollider2D boxCol, int rayCastCount = 5, float edgeOffSet = 0.05f, float distance = 0.03f, int layerMask = ~0)
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
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(boxCol.gameObject.transform.position.x + width / 2, boxCol.gameObject.transform.position.y), Vector2.right, distance, layerMask);
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
                    hits.Add(Physics2D.Raycast(origin, Vector2.right, distance, layerMask));
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
    public static List<GameObject> CheckCollisionLeft(BoxCollider2D boxCol, int rayCastCount = 5, float edgeOffSet = 0.05f, float distance = 0.03f, int layerMask = ~0)
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
                RaycastHit2D hit = Physics2D.Raycast(new Vector2(boxCol.gameObject.transform.position.x - width / 2, boxCol.gameObject.transform.position.y), Vector2.left, distance, layerMask);
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
                    hits.Add(Physics2D.Raycast(origin, Vector2.left, distance, layerMask));
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
    #endregion

    #region Extra Checks
    ///<summary>
    ///This function uses raycasts to check a square area in a world position 
    ///</summary>
    ///<param name="point">Vector2 world position to check </param>
    ///<param name="width">Width of the square </param>
    ///<param name="height">Height of the square </param>
    ///<param name="linesQuantity">Number of lines to be cast horizontaly and verticaly </param>
    public static bool CheckSquare2D(Vector2 point, float width = 1f, float height = 1f, int linesQuantity = 10, int layerMask = ~0, Directions raycastDirection = Directions.Right)
    {
        bool result = false;
        if (linesQuantity <= 0)
            linesQuantity = 1;
        RaycastHit2D[] hits = new RaycastHit2D[linesQuantity];

        if (linesQuantity == 1)
        {


            Vector2 startPoint = point;
            Vector2 direction = Vector2.right;
            float distance = 1f;
            switch (raycastDirection)
            {
                case Directions.Left:
                    startPoint = new Vector2(point.x + width / 2f, point.y);
                    direction = Vector2.left;
                    distance = width;
                    break;
                case Directions.Right:
                    startPoint = new Vector2(point.x - width / 2f, point.y);
                    direction = Vector2.right;
                    distance = width;
                    break;
                case Directions.Up:
                    startPoint = new Vector2(point.x, point.y - height / 2f);
                    direction = Vector2.up;
                    distance = height;
                    break;
                case Directions.Down:
                    startPoint = new Vector2(point.x, point.y + height / 2f);
                    direction = Vector2.down;
                    distance = height;
                    break;
            }

            Debug.DrawRay(startPoint, direction, Color.red);
            hits[0] = Physics2D.Raycast(startPoint, direction, distance, layerMask);
            if (hits[0])
                return true;
        }
        else
        {
            Vector2 startPoint = point;
            Vector2 direction = Vector2.right;
            float lineSpacingX = 0;
            float lineSpacingY = 0;
            float distance = 1f;

            switch (raycastDirection)
            {
                case Directions.Left:
                    startPoint = new Vector2(point.x + width / 2f, point.y - height / 2f);
                    direction = Vector2.left;
                    distance = width;
                    lineSpacingX = 0f;
                    lineSpacingY = height / (linesQuantity - 1);
                    break;
                case Directions.Right:
                    startPoint = new Vector2(point.x - width / 2f, point.y - height / 2f);
                    direction = Vector2.right;
                    distance = width;
                    lineSpacingX = 0f;
                    lineSpacingY = height / (linesQuantity - 1);
                    break;
                case Directions.Up:
                    startPoint = new Vector2(point.x - width / 2f, point.y - height / 2f);
                    direction = Vector2.up;
                    distance = height;
                    lineSpacingX = width / (linesQuantity - 1);
                    lineSpacingY = 0f;
                    break;
                case Directions.Down:
                    startPoint = new Vector2(point.x - width / 2f, point.y + height / 2f);
                    direction = Vector2.down;
                    distance = height;
                    lineSpacingX = width / (linesQuantity - 1);
                    lineSpacingY = 0f;
                    break;

            }
            hits[0] = Physics2D.Raycast(startPoint, direction, distance, layerMask);
            Debug.DrawRay(startPoint, direction * distance, Color.red);

            for (int i = 1; i <= linesQuantity - 1; i++)
            {
                Debug.DrawRay(new Vector2(startPoint.x + lineSpacingX * i, startPoint.y + lineSpacingY * i), direction * distance, Color.red);
                hits[i] = Physics2D.Raycast(new Vector2(startPoint.x + lineSpacingX * i, startPoint.y + lineSpacingY * i), direction, distance, layerMask);
            }

            //Debug.DrawRay(new Vector2(point.x - width / 2, point.y + height / 2), Vector2.right * width, Color.red);
            // hits[linesQuantity - 1] = Physics2D.Raycast(new Vector2(point.x - width / 2, point.y + height / 2), Vector2.right, width, layerMask);
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

    public static bool CheckForFallLeft(BoxCollider2D col, float checkDistance = 0.1f, float checkOffset = 0.05f, int layerMask = ~0)
    {
        if (!col)
            return false;
        Vector2 lowerLeft = col.bounds.min - new Vector3(Mathf.Abs(checkOffset), 0f);
        Debug.DrawRay(lowerLeft, Vector3.down * checkDistance, Color.blue);
        return !Physics2D.Raycast(lowerLeft, Vector3.down, checkDistance);

    }
    public static bool CheckForFallRight(BoxCollider2D col, float checkDistance = 0.1f, float checkOffset = 0.05f, int layerMask = ~0)
    {
        if (!col)
            return false;
        Vector2 lowerRight = new Vector3(col.bounds.max.x, col.bounds.min.y) + new Vector3(Mathf.Abs(checkOffset), 0f);
        Debug.DrawRay(lowerRight, Vector3.down * checkDistance, Color.blue);
        return !Physics2D.Raycast(lowerRight, Vector3.down, checkDistance);

    }
    #endregion
}
