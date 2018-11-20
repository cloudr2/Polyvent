using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public float smoothness = 0.125f;
    public Vector3 offset;

    private Player player;

    private void Awake() {
        player = FindObjectOfType<Player>();
    }

    private void LateUpdate() {
        Vector3 targetPos = player.transform.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, targetPos, smoothness);
        transform.position = new Vector3 (smoothPos.x, smoothPos.y, smoothPos.z);
    }
}
