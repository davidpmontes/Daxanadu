using UnityEngine;

public class Skulls : MonoBehaviour
{
    [SerializeField] private GameObject skullA = default;
    [SerializeField] private GameObject skullB = default;
    [SerializeField] private GameObject skullC = default;

    [SerializeField] private GameObject rotationObj = default;
    [SerializeField] private GameObject pointA = default;
    [SerializeField] private GameObject pointB = default;
    [SerializeField] private GameObject pointC = default;

    private float rotationSpeed = 120;
    private float oscillationRate = 2;
    private float oscillationAmplitude = 1.5f;
    private float direction = 0;
    private Vector2 calculatedPosition;

    private void Update()
    {
        MoveTowardsPlayer();
        OscillateLeftRight();
        RotateSkulls();
    }

    private void MoveTowardsPlayer()
    {
        if (transform.position.x > Player.Instance.transform.position.x)
        {
            direction = -2;
        }
        else
        {
            direction = 2;
        }
    }

    private void OscillateLeftRight()
    {
        transform.position = new Vector3(transform.position.x + direction * Time.deltaTime,
                                         transform.parent.transform.TransformPoint(Vector3.zero).y +
                                            Mathf.Sin(Time.time * oscillationRate) * oscillationAmplitude,
                                         transform.position.z);
    }

    private void RotateSkulls()
    {
        rotationObj.transform.RotateAround(transform.TransformPoint(Vector3.zero), Vector3.back, -Time.deltaTime * rotationSpeed);
        if (skullA) skullA.transform.position = pointA.transform.position;
        if (skullB) skullB.transform.position = pointB.transform.position;
        if (skullC) skullC.transform.position = pointC.transform.position;
    }
}
