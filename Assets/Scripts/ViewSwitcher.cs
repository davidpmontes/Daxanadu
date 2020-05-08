using UnityEngine;

public class ViewSwitcher : MonoBehaviour
{
    public static ViewSwitcher Instance { get; private set; }
    [SerializeField] private GameObject player;

    private Bounds bounds;
    private string state;
    private Vector2 direction;
    private Vector2 destination;
    private float transitionPercent;
    private Vector3 oldPosition;

    private int BOUNDS_WIDTH = 16;
    private int BOUNDS_HEIGHT = 15;

    private bool cameraFollow;

    private void Awake()
    {
        Instance = this;
        bounds = new Bounds(transform.position, new Vector2(BOUNDS_WIDTH, BOUNDS_HEIGHT));
        state = "playing";
    }

    private void Update()
    {
        if (!cameraFollow)
            return;

        if (state == "playing")
        {
            if (!bounds.Contains(player.transform.position))
            {
                player.GetComponent<Player>().Pause();
                if (player.transform.position.x > bounds.max.x)
                {
                    direction = new Vector2(1, 0);
                }
                else if (player.transform.position.x < bounds.min.x)
                {
                    direction = new Vector2(-1, 0);
                }
                else if (player.transform.position.y > bounds.max.y)
                {
                    direction = new Vector2(-1, 0);
                }
                else
                {
                    direction = new Vector2(1, 0);
                }
                destination = transform.position + new Vector3(direction.x, direction.y, 0) * BOUNDS_WIDTH;
                state = "transitioning";
                transitionPercent = 0f;
                oldPosition = transform.position;
            }
        }
        else if (state == "transitioning")
        {
            transitionPercent += Time.deltaTime;
            Vector2 newPosition = Vector2.Lerp(oldPosition, destination, transitionPercent);
            transform.position = newPosition;
            if (transitionPercent > 1)
            {
                player.GetComponent<Player>().Unpause();
                bounds.center = transform.position;
                state = "playing";
            }
        }
    }

    public void Teleporting(Vector2 playerDestination,
                            Vector2 cameraDestination,
                            bool enableCameraFollow)
    {
        cameraFollow = enableCameraFollow;
        player.GetComponent<Player>().Pause();
        transform.position = cameraDestination;
        bounds.center = cameraDestination;
        Player.Instance.transform.position = playerDestination;
        player.GetComponent<Player>().Unpause();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}