using UnityEngine;

public class MoveViaPath : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private Mesh[] availableCars;
    [SerializeField] private Material[] availableMaterials;
    [SerializeField] private GameObject car;
    private int pointIndex = 0;

    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float minSpeed = 1f;
    public float speed = 0f;

    private FrontDetector frontDetector;

    private void Awake()
    {
        if (availableCars.Length != availableMaterials.Length)
        {
            Debug.LogError("The number of available cars and materials should be the same.", this.gameObject);
        }

        frontDetector = GetComponentInChildren<FrontDetector>();
    }

    private void Start()
    {
        ResetCar();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (pointIndex < points.Length)
        {
            car.transform.position = Vector3.MoveTowards(car.transform.position, points[pointIndex].position, speed * Time.deltaTime);

            Vector3 direction = points[pointIndex].position - car.transform.position;

            Rotate(direction);

            if (Vector3.Distance(car.transform.position, points[pointIndex].position) < 0.1f)
            {
                pointIndex++;
            }

            frontDetector.transform.position = car.transform.position;
            frontDetector.transform.rotation = car.transform.rotation;
        }
        else
        {
            ResetCar();
        }
    }

    private void Rotate(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            car.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void ResetCar()
    {
        pointIndex = 0;
        int randomIndex = Random.Range(0, availableCars.Length);

        if (car != null)
        {
            car.GetComponent<MeshFilter>().mesh = availableCars[randomIndex];
            car.GetComponent<MeshRenderer>().material = availableMaterials[randomIndex];
            car.transform.position = points[pointIndex].position;
        }

        speed = Random.Range(minSpeed, maxSpeed);
    }
}