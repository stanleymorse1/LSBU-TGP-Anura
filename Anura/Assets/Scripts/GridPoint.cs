using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridPoint : MonoBehaviour
{
    //Do some sort of gizmo to tell if a point is one way or set up an automatic system
    public Transform up;
    public Transform left;
    public Transform down;
    public Transform right;

    public GameObject prefab;

    public void CreatePoint(int dir = -1)
    {
        GameObject i = null;
        switch (dir)
        {
            case 0:
                if (!up)
                {
                    i = Instantiate(prefab, transform.position + new Vector3(0, 0, 5), Quaternion.identity, gameObject.transform.parent);
                    resetInstance(i);
                    i.GetComponent<GridPoint>().down = gameObject.transform;
                    up = i.transform;
                }
                break;
            case 1:
                if (!left)
                {
                    i = Instantiate(prefab, transform.position + new Vector3(-5, 0, 0), Quaternion.identity, gameObject.transform.parent);
                    resetInstance(i);
                    i.GetComponent<GridPoint>().right = gameObject.transform;
                    left = i.transform;
                }
                break;
            case 2:
                if (!down)
                {
                    i = Instantiate(prefab, transform.position + new Vector3(0, 0, -5), Quaternion.identity, gameObject.transform.parent);
                    resetInstance(i);
                    i.GetComponent<GridPoint>().up = gameObject.transform;
                    down = i.transform;
                }
                break;
            case 3:
                if (!right)
                {
                    i = Instantiate(prefab, transform.position + new Vector3(5, 0, 0), Quaternion.identity, gameObject.transform.parent);
                    resetInstance(i);
                    i.GetComponent<GridPoint>().left = gameObject.transform;
                    right = i.transform;
                }
                break;
            default:
                print("Wtf");
                break;
        }
        if (i != null)
        {
            i.name = "Point";
            i.GetComponent<GridPoint>().prefab = prefab;
        }

    }
    void resetInstance(GameObject gameObject)
    {
        GridPoint p = gameObject.GetComponent<GridPoint>();
        p.up = null;
        p.left = null;
        p.down = null;
        p.right = null;
    }
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
