using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [Header("Wall Settings")]
    [SerializeField] private int hitsToBreak = 4;

    private int currentHits = 0;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeHit()
    {
        currentHits++;

        UpdateTransparency();

        if (currentHits >= hitsToBreak)
        {
            Destroy(gameObject);
        }
    }

    private void UpdateTransparency()
    {
        if (spriteRenderer == null)
            return;

        float alpha = 1f - ((float)currentHits / hitsToBreak);

        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}