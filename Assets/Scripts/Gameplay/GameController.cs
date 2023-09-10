using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private EnemySpawner spawner;
    [SerializeField] private CameraFollow cameraFollow;

    private void Awake()
    {
        GameObject receive = GameObject.FindGameObjectWithTag("Param");
        if (receive != null)
        {
            Parameter parameter = receive.GetComponent<Parameter>();
            spawner.Init(parameter.color, cameraFollow.SetPlayer);
            Destroy(receive);
        }
    }
}
