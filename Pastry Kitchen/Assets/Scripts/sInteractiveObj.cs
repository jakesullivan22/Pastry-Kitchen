using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class sInteractiveObj : MonoBehaviour
{
    //List<GameObject> interObjList;
    [NamedArray(typeof(eInteractiveObj))] public GameObject[] interObj;

    private void Update()
    {
        float curDis;
        curDis = Vector3.Distance(transform.position, getClosestInterObj().transform.position);
        if (Input.GetKey(KeyCode.F))
        {
            Debug.Log(getClosestInterObj());
        }
    }

    public GameObject getClosestInterObj()
    {
        GameObject closestObj = null;
        float closestDis = float.PositiveInfinity;
        float curDis;
        foreach(GameObject i in interObj)
        {
            curDis = Vector3.Distance(transform.position, i.transform.position);
            if (curDis < closestDis)
            {
                closestObj = i;
                closestDis = curDis;
            }
        }
        return closestObj;
    }
}
