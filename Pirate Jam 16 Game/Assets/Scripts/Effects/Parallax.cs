using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float startOffsetX;
    [SerializeField] private GameObject cam;
    
    [SerializeField] [Range(0, 1)] [Tooltip("1 = move with cam || 0 = wont move || 0.5 = half")]
    private float distance;

    void Start()
    {
        startOffsetX = transform.position.x;
    }

    void LateUpdate()
    {
        float camOffset = cam.transform.position.x * distance;

        transform.position = new Vector3(startOffsetX + camOffset, transform.position.y, transform.position.z);
    }
}
