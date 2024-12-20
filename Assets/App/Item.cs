
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public abstract void ActivateEffect();

    void OnDestroy()
    {
        ActivateEffect();
    }
}