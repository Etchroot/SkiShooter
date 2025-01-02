using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }

    public Transform[] checkpoints; // 체크포인트 배열
    private int currentCheckpointIndex = 0;

    public bool AllCheckpointsCompleted { get; private set; } = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InitializeCheckpoints();
    }

    private void InitializeCheckpoints()
    {
        for (int i = 0; i < checkpoints.Length; i++)
        {
            Vector3 checkpointPos = checkpoints[i].position;

            // Raycast로 각 체크포인트의 Y축 위치 보정
            Ray ray = new Ray(checkpointPos + Vector3.up * 10f, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                checkpointPos.y = hit.point.y + 0.2f; // GroundOffset
            }

            checkpoints[i].position = checkpointPos;
        }
    }

    public Vector3 GetCurrentCheckpointPosition()
    {
        if (currentCheckpointIndex < checkpoints.Length)
        {
            return checkpoints[currentCheckpointIndex].position;
        }

        return Vector3.zero;
    }

    public void MoveToNextCheckpoint()
    {
        currentCheckpointIndex++;
        if (currentCheckpointIndex >= checkpoints.Length)
        {
            Debug.Log("모든 체크포인트를 완료했습니다!");
            AllCheckpointsCompleted = true;
        }
    }

    // 체크포인트에 도달했는지 여부 확인
    public bool IsPlayerAtCheckpoint(Vector3 playerPosition)
    {
        if (currentCheckpointIndex < checkpoints.Length)
        {
            Vector3 checkpointPosition = checkpoints[currentCheckpointIndex].position;
            float distance = Vector3.Distance(new Vector3(playerPosition.x, 0, playerPosition.z), new Vector3(checkpointPosition.x, 0, checkpointPosition.z));
            float heightDifference = Mathf.Abs(playerPosition.y - checkpointPosition.y);

            //Debug.Log($"Player Position: {playerPosition}, Checkpoint Position: {checkpointPosition}, Distance: {distance}, Height Difference: {heightDifference}");
            
            return distance < 1.0f && heightDifference < 1.0f;
        }
        return false;
    }

}
