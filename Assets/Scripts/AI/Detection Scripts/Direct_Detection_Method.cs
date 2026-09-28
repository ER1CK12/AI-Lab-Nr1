using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Detection : MonoBehaviour
{
    [SerializeField] private int rayCount = 50;
    public bool isPlayerDetected = false;
    public Vector3 player;
    private float detectionRange = 20f;
    private float fieldOfView = 180f;
    private Movement movement;



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
