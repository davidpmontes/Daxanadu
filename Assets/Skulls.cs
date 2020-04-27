using UnityEngine;

public class Skulls : MonoBehaviour
{
    [SerializeField] private GameObject skullA;
    [SerializeField] private GameObject skullB;
    [SerializeField] private GameObject skullC;

    [SerializeField] private GameObject rotationObj;
    [SerializeField] private GameObject pointA;
    [SerializeField] private GameObject pointB;
    [SerializeField] private GameObject pointC;

    private float rotationSpeed = 120;
    private float oscillationRate = 2;
    private float oscillationAmplitude = 1.5f;
    private float direction = 0;

    private void Update()
    {
        OscillateLeftRight();
        RotateSkulls();
        MoveTowardsPlayer();
    }

    private void RotateSkulls()
    {
        rotationObj.transform.RotateAround(transform.TransformPoint(Vector3.zero), Vector3.back, -Time.deltaTime * rotationSpeed);
        skullA.transform.position = pointA.transform.position;
        skullB.transform.position = pointB.transform.position;
        skullC.transform.position = pointC.transform.position;
    }

    private void OscillateLeftRight()
    {
        transform.position = new Vector3(transform.position.x + direction * Time.deltaTime,
                                         transform.parent.transform.TransformPoint(Vector3.zero).y +
                                            Mathf.Sin(Time.time * oscillationRate) * oscillationAmplitude,
                                         transform.position.z);
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
}
