using Unity.XR.CoreUtils;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager Instance { get; private set; }

    [SerializeField] GameObject checkpointPack;
    [SerializeField] Transform[] checkpoints; // 체크포인트 배열

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
    }

    private void Start()
    {
        // Null 체크
        if (checkpointPack == null)
        {
            Debug.LogError("CheckpointPack이 설정되지 않았습니다.");
            return;
        }

        // 자식의 수 가져오기
        int childCount = checkpointPack.transform.childCount;

        // 배열 초기화
        checkpoints = new Transform[childCount];

        // 자식 Transform을 배열에 추가
        for (int i = 0; i < childCount; i++)
        {
            checkpoints[i] = checkpointPack.transform.GetChild(i);
            //Debug.Log($"Checkpoint {i + 1}: 이름 = {checkpoints[i].name}, 위치 = {checkpoints[i].position}");
        }
    }

    // 현재 체크포인트 위치
    public Vector3 GetCurrentCheckpointPosition()
    {
        if (currentCheckpointIndex < checkpoints.Length)
        {
            return checkpoints[currentCheckpointIndex].position;
        }

        return Vector3.zero;
    }

    // 다음 체크포인트로 이동
    public void MoveToNextCheckpoint()
    {
        currentCheckpointIndex++;
        Debug.Log($"현재 체크포인트 : {currentCheckpointIndex}");

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
            float distance = Vector3.Distance(
                new Vector3(playerPosition.x, 0, playerPosition.z),
                new Vector3(checkpointPosition.x, 0, checkpointPosition.z)); // 거리

            // 현재 체크포인트에 도달한 것을 확인
            Debug.Log($"Distance to checkpoint: {distance}");

            // 체크포인트에 도달했다고 판단되는 최소 거리 조건 (1.0f는 플레이어와 체크포인트 간의 근접한 거리)
            return distance < 1.0f;
        }
        return false;
    }
}
