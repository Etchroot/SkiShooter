using System.Collections;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Firepointcheck : MonoBehaviour
{
    void OnEnable()
    {
        transform.localPosition = Vector3.zero;
    }
    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        transform.localPosition = Vector3.zero;
    }
}
