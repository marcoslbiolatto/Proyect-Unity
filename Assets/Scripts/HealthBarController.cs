using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    public SpriteRenderer fillRenderer;
    public float maxWidth = 2f;
    public float maxHeight = 2f;

    public void SetHealth(float current, float max)
    {
        float ratio = Mathf.Clamp01(current / max);
        fillRenderer.transform.localScale = new Vector3(ratio * maxWidth, maxHeight, 1f);
    }
}
