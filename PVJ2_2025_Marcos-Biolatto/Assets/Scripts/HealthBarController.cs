using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    public SpriteRenderer fillRenderer;
    public float maxWidth = 1f;

    public void SetHealth(float current, float max)
    {
        float ratio = Mathf.Clamp01(current / max);
        fillRenderer.transform.localScale = new Vector3(ratio * maxWidth, 1f, 1f);
    }
}
