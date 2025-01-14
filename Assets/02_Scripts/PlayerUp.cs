using UnityEngine;

public class PlayerUp : MonoBehaviour
{
    public float speed = 10f; // 상승속도

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }
}
