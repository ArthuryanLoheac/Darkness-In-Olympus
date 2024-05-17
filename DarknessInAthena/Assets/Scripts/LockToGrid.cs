using UnityEditor;
using UnityEngine;


[ExecuteInEditMode]
public class LockToGrid : MonoBehaviour
{
    public float tileSize = 0.16f;
    public Vector3 tileOffset = Vector3.zero;

    void Update()
    {
        if(!EditorApplication.isPlaying)
        {
            Vector3 currentPosition = transform.position;

            float snappedX = Mathf.Round(currentPosition.x / tileSize) * tileSize + tileOffset.x;
            float snappedZ = tileOffset.z;
            float snappedY = Mathf.Round(currentPosition.y / tileSize) * tileSize + tileOffset.y;

            transform.position = new Vector3(snappedX, snappedY, snappedZ);;
        }
    }
}