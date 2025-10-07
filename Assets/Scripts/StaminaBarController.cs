using UnityEngine;

public class StaminaBarController : MonoBehaviour
{
    public SpriteRenderer fillRenderer;
    public float maxWidth = 1f;
    public float maxHeight = 1f;

    public void SetStamina(float current, float max)
    {
        float ratio = Mathf.Clamp01(current / max);
        fillRenderer.transform.localScale = new Vector3(ratio * maxWidth, maxHeight, 1f);
    }
}

