using UnityEngine;

// gentle up/down bob for prompts like the talk bubble
public class Bob : MonoBehaviour
{
    [SerializeField] private float height = 0.08f;
    [SerializeField] private float speed = 4f;

    private Vector3 start;

    private void OnEnable()
    {
        start = transform.localPosition;
    }

    private void Update()
    {
        transform.localPosition = start + Vector3.up * Mathf.Sin(Time.time * speed) * height;
    }

    private void OnDisable()
    {
        transform.localPosition = start;
    }
}
