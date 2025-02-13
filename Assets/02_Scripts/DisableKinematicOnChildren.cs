using System.Collections.Generic;
using UnityEngine;

public class DisableKinematicOnChildren : MonoBehaviour
{
    private List<Rigidbody> rbs = new List<Rigidbody>();
    private void Awake()
    {
        GetComponentsInChildren<Rigidbody>(rbs);

        foreach (var rb in rbs)
        {
            rb.isKinematic = true;
        }
    }

    public void DisableKinematic()
    {
        Debug.Log("체크");

        foreach (var rb in rbs)
        {
            rb.isKinematic = false;
        }
    }
}
