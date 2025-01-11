using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private Sprite[] availableClouds;

    private int pointIndex = 0;

    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float minSpeed = 1f;
    public float speed = 0f;

    [SerializeField] private float minScale = 0.5f;
    [SerializeField] private float maxScale = 2f;

    [SerializeField] private GameObject cloud;

    private void Start()
    {
        ResetCloud();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (pointIndex < points.Length)
        {
            cloud.transform.position = Vector3.MoveTowards(cloud.transform.position, points[pointIndex].position, speed * Time.deltaTime);
            if (Vector3.Distance(cloud.transform.position, points[pointIndex].position) < 0.1f)
            {
                pointIndex++;
            }
        }
        else
        {
            ResetCloud();
        }
    }

    private void ResetCloud()
    {
        pointIndex = 0;
        cloud.transform.position = points[0].position;
        cloud.transform.localScale = Vector3.one * Random.Range(minScale, maxScale);
        GetComponentInChildren<SpriteRenderer>().sprite = availableClouds[Random.Range(0, availableClouds.Length)];
        speed = Random.Range(minSpeed, maxSpeed);
    }
}
