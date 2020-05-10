using System.Collections;
using UnityEngine;

public class ViewSwitcher : MonoBehaviour
{
    public static ViewSwitcher Instance { get; private set; }
    [SerializeField] private GameObject player;

    [SerializeField] private SpriteRenderer blackFade;
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
        cameraFollow = true;
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
        StartCoroutine(TeleportCoroutine(playerDestination,
                                         cameraDestination,
                                         enableCameraFollow));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }

    private IEnumerator TeleportCoroutine(Vector2 playerDestination,
                            Vector2 cameraDestination,
                            bool enableCameraFollow)
    {
        player.GetComponent<Player>().Pause();

        yield return FadeToBlack();

        cameraFollow = enableCameraFollow;
        transform.position = cameraDestination;
        bounds.center = cameraDestination;
        Player.Instance.transform.position = playerDestination;

        yield return FadeToTransparent();

        player.GetComponent<Player>().Unpause();
    }

    private IEnumerator FadeToBlack()
    {
        Color newColor = Color.black;
        newColor.a = 0;
        while (newColor.a < 1)
        {
            newColor.a += 0.2f;
            blackFade.color = newColor;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator FadeToTransparent()
    {
        Color newColor = Color.black;
        newColor.a = 1;
        while (newColor.a > 0)
        {
            newColor.a -= 0.2f;
            blackFade.color = newColor;
            yield return new WaitForSeconds(0.1f);
        }
    }
}