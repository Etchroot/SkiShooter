using UnityEngine;

public class TargetPlate : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        //총알에 맞으면
        if (other.CompareTag("BULLET"))
        {
            Destroy(this.gameObject);
            //Die();
        }
    }
}
