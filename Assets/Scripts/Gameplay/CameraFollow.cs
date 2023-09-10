using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float speed = 20f;
    [SerializeField] private Vector3 offset;

    public void SetPlayer(Transform player) => this.player = player;
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.deltaTime * speed);
    }

}
