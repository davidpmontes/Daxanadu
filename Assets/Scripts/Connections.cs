using UnityEditor;
using UnityEngine;

public class Connections : MonoBehaviour
{
    public bool showLines;
    public bool showLabels;
    public bool showCameras;

    private void OnDrawGizmos()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var doorPair = transform.GetChild(i);
            var doorA = doorPair.transform.GetChild(0);
            var doorB = doorPair.transform.GetChild(1);
            var cameraB = doorPair.transform.GetChild(2);

            Gizmos.color = Color.red;
            if (showLines)
            {
                Gizmos.DrawLine(doorA.transform.position, doorB.transform.position);

                Gizmos.DrawIcon(doorA.transform.position, "redUpArrow", true);
                //Gizmos.DrawWireCube(doorA.transform.position, new Vector2(1, 2));
                Gizmos.DrawWireCube(doorB.transform.position, new Vector2(1, 2));
            }

            Gizmos.color = Color.blue;
            if (showCameras)
            {
                Gizmos.DrawWireCube(cameraB.transform.position,
                                    new Vector2(16, 15));
            }

            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.red;

            if (showLabels)
            {
                Handles.Label(doorA.transform.position, "Entrance", style);
                Handles.Label(doorB.transform.position, "Exit", style);
            }


        }
    }
}
