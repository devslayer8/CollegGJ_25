using UnityEngine;

public class DynamicCamera : MonoBehaviour
{
    public Transform player1; // Reference to the first player (Attacker)
    public Transform player2; // Reference to the second player (Healer)
    public float minFOV = 60f; // Minimum field of view
    public float maxFOV = 90f; // Maximum field of view
    public float zoomOutDistance = 10f; // Distance at which the camera starts to zoom out
    public float followSpeed = 5f; // Speed at which the camera follows the players

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        // Check if both players are not null
        if (player1 != null && player2 != null)
        {
            // Calculate the middle point between both players
            Vector3 middlePoint = (player1.position + player2.position) / 2f;
            Vector3 newPosition = new Vector3(middlePoint.x, middlePoint.y, transform.position.z);

            // Move the camera towards the new position based on the follow speed
            transform.position = Vector3.MoveTowards(transform.position, newPosition, followSpeed * Time.deltaTime);

            // Calculate the distance between the players
            float distance = Vector3.Distance(player1.position, player2.position);

            // Adjust the field of view based on the distance between the players
            float targetFOV = Mathf.Lerp(minFOV, maxFOV, distance / zoomOutDistance);
            cam.fieldOfView = targetFOV;
        }
    }
}
