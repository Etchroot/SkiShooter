using UnityEngine;

public class HeadPointTrigger : MonoBehaviour
{
    public CollisionCheck collisionCheck;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TRUNK"))
        {
            Debug.Log("나무와 충돌");

            collisionCheck.onObstacleEnemyCollision.Invoke();

        }
    }
}
