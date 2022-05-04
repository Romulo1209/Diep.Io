using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerController player;
    public float cameraHeight = -10;
    public float cameraSpeed;
    public float cameraDistance = 8;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }
    private void FixedUpdate()
    {
        CameraSettings();

        Vector3 camPos = new Vector3(transform.position.x, transform.position.y, cameraHeight);
        Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y, cameraHeight);
        transform.position = Vector3.Lerp(camPos, playerPos, cameraSpeed * Time.deltaTime);
    }
    void CameraSettings()
    {
        GetComponent<Camera>().orthographicSize = cameraDistance;
    }
}
