using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Doors : MonoBehaviour
{
    public GameObject playerDestination;
    public GameObject cameraDestination;
    public bool enableCameraFollowAtDestination;

    private bool enforceUpDirectionReleasedBeforeEntering;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (InputController.Instance.DirectionalInput.y < 0.5f)
        {
            enforceUpDirectionReleasedBeforeEntering = true;
        }

        if (InputController.Instance.DirectionalInput.y > 0.5f)
        {
            if (enforceUpDirectionReleasedBeforeEntering)
            {
                enforceUpDirectionReleasedBeforeEntering = false;
                ViewSwitcher.Instance.Teleporting(playerDestination.transform.position,
                                                  cameraDestination.transform.position,
                                                  enableCameraFollowAtDestination);
            }
        }
    }
}
