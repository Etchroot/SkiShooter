using Unity.XR.CoreUtils;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager Instance { get; private set; }

    [SerializeField] GameObject checkpointPack;
    [SerializeField] Transform[] checkpoints;

    private int playerCheckpointIndex = 0;
    private int droneCheckpointIndex = 0;

    public bool PlayerAllCheckpointsCompleted { get; private set; } = false;
    //public bool DroneAllCheckpointsCompleted { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (checkpointPack == null)
        {
            Debug.LogError("CheckpointPack이 설정되지 않았습니다.");
            return;
        }

        int childCount = checkpointPack.transform.childCount;
        checkpoints = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
            checkpoints[i] = checkpointPack.transform.GetChild(i);
    }

    public Vector3 GetCheckpointPosition(bool isPlayer)
    {
        int index = isPlayer ? playerCheckpointIndex : droneCheckpointIndex;
        return index < checkpoints.Length ? checkpoints[index].position : Vector3.zero;
    }

    public void MoveToNextCheckpoint(bool isPlayer)
    {
        if (isPlayer)
        {
            playerCheckpointIndex++;
            if (playerCheckpointIndex >= checkpoints.Length)
            {
                PlayerAllCheckpointsCompleted = true;
            }
        }
        else
        {
            droneCheckpointIndex++;
            //if (droneCheckpointIndex >= checkpoints.Length)
            //{
            //    DroneAllCheckpointsCompleted = true;
            //}
        }
    }

    public bool IsAtCheckpoint(Vector3 position, bool isPlayer)
    {
        int index = isPlayer ? playerCheckpointIndex : droneCheckpointIndex;
        if (index < checkpoints.Length)
        {
            Vector3 checkpointPosition = checkpoints[index].position;
            float distance = Vector3.Distance(
                new Vector3(position.x, 0, position.z),
                new Vector3(checkpointPosition.x, 0, checkpointPosition.z));

            return distance < 1.0f;
        }
        return false;
    }
}
