using System;
using UnityEngine;

public class Radar_Detection_Method : MonoBehaviour
{
    [SerializeField] private int rayCount = 100;
    public bool isPlayerDetected = false;
    public Vector3 player;
    private float detectionRange = 13f;
    private float fieldOfView = 360f;
    private Movement movement;

    private float scanTimer;
    private float scanInterval = 1.5f;

    private void Awake()
    {
        movement = GetComponent<Movement>();
    }

    private void Update()
    {
        DetectPlayer();
    }

    private void DetectPlayer()
    {
        scanTimer += Time.deltaTime;
        if (scanTimer < scanInterval) return;

        scanTimer = 0f;

        float startingAngle = -90f;
        float angleStep = fieldOfView / (rayCount - 1);

        int detectionScore = 0;

        for (int i = 0; i < rayCount; i++)
        {
            float angle = startingAngle + (angleStep * i);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, detectionRange))
            {
                //Debug.Log("Ray hit: " + hit.collider.name);
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    detectionScore++;
                    isPlayerDetected = true;
                    player = hit.collider.transform.position;
                    Debug.Log("Player detected!");
                    return;
                }

                if (detectionScore == 0)
                {
                    isPlayerDetected = false;
                }
            }
            Debug.DrawRay(transform.position, direction * detectionRange, Color.red);
        }


    }

}
