using UnityEngine;

public class DiscretePositionFixer : MonoBehaviour
{
    public float pixPerUnit = 16;

    void LateUpdate()
    {
        transform.position = new Vector3(
            Mathf.Round(transform.parent.position.x * pixPerUnit) / pixPerUnit,
            Mathf.Round(transform.parent.position.y * pixPerUnit) / pixPerUnit,
            -10);
    }
}
