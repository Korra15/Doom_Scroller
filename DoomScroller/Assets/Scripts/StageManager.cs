using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] private Transform _spawnPoint;

    [Header("Bounds")]
    [Tooltip("The left bound of the stage based on the transform position")]
    [SerializeField, Range(-100, -10)] int _leftBound;
    [Tooltip("The right bound of the stage based on the transform position")]
    [SerializeField, Range(10, 100)] int _rightBound;
    public int LeftBound => _leftBound + (int)transform.position.x;
    public int RightBound => _rightBound + (int)transform.position.x;
    public int StageHeight => (int)this.transform.position.y;

    /// <summary>
    /// A helper function to determine if a transform is within the stage bounds
    /// </summary>
    /// <param name="transform">The transform to be checked</param>
    /// <returns>True if the position is inside the bounds. False if not.</returns>
    public bool IsTransformInBounds(Transform transform)
    {
        if (transform == null) return false;
        float x_value = transform.position.x;

        if (x_value >= LeftBound && x_value <= RightBound) return true;
        return false;
    }


    /// <summary>
    /// A helper function to determine if a Vector2 value is within the bounds
    /// </summary>
    /// <param name="pos">The position value to be checked.</param>
    /// <returns>True if the position is inside the bounds. False if not.</returns>
    public bool IsPositionInBounds(Vector2 pos)
    {
        if (pos.x > LeftBound && pos.x < RightBound) return true;
        return false;
    }

    /// <summary>
    /// A helper function to get a random stage position 
    /// </summary>
    public Vector2 GetRandomPointWithinBounds()
    {
        float random_x_value = Random.Range(LeftBound, RightBound);
        return new Vector2(random_x_value, this.transform.position.y);
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDrawGizmos()
    {
        if (_spawnPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_spawnPoint.transform.position, 1f);
        }

        // ---------------- STAGE BOUNDS --------------------------------
        Gizmos.color = Color.red;

        int localHeightValue = (int)this.transform.position.y;

        Vector3 localLeftBoundPos = new Vector3(LeftBound, localHeightValue, 0);
        Vector3 localRightBoundPos = new Vector3(RightBound, localHeightValue, 0);

        Gizmos.DrawLine(localLeftBoundPos, localLeftBoundPos + (Vector3.up * 10));
        Gizmos.DrawLine(localRightBoundPos, localRightBoundPos + (Vector3.up * 10));

    }
}
