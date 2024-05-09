using UnityEngine;

/// <summary>
/// Class to control the main camera such that it follows the player.
/// Note: Use this instead of setting the camera as a child of the player to keep 
/// any changes to the player transform seperated.
/// </summary>
public class CameraController : MonoBehaviour {
    public PlayerController player;
    private Vector3 lastPlayerPos;
    private float distToMoveX;
    private float distToMoveY;

	void Start(){
        player = FindObjectOfType<PlayerController>();
        lastPlayerPos = player.transform.position;
    }

	void LateUpdate(){
        // Calculate where the player has moved relative to camera
        distToMoveX = player.transform.position.x - lastPlayerPos.x;
        distToMoveY = player.transform.position.y - lastPlayerPos.y;

        // Move the camera to new location
        transform.position = new Vector3(transform.position.x + distToMoveX,
                                         transform.position.y + distToMoveY,
                                         transform.position.z);
        lastPlayerPos = player.transform.position;
	}
}
