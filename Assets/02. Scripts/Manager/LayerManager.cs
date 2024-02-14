using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LayerManager : MonoBehaviour
{
    public void ChangeLayer(GameObject targetObject)
    {
        StartCoroutine(ReturnLayer(targetObject, targetObject.layer));
        targetObject.layer = 31;
    }
    IEnumerator ReturnLayer(GameObject targetObject, int originLayer)
    {
        yield return new WaitForSeconds(10f);
        targetObject.layer = originLayer;
    }
}
