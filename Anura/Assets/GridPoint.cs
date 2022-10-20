using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoint : MonoBehaviour
{
    //Do some sort of gizmo to tell if a point is one way or set up an automatic system
    public Transform up;
    public Transform left;
    public Transform down;
    public Transform right;

    private void OnDrawGizmos()
    {
        if(up)
            Debug.DrawLine(transform.position, up.position, Color.green);
        if (left)
            Debug.DrawLine(transform.position, left.position, Color.green);
        if (down)
            Debug.DrawLine(transform.position, down.position, Color.green);
        if (right)
            Debug.DrawLine(transform.position, right.position, Color.green);
    }
}
