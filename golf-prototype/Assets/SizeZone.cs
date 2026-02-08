using UnityEngine;

public enum SizeZoneType
{
    Shrink,
    Grow
}

public class SizeZone : MonoBehaviour
{
    [SerializeField] private SizeZoneType zoneType = SizeZoneType.Shrink;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BallSizeController ballSize = collision.gameObject.GetComponent<BallSizeController>();
        if (ballSize != null)
        {
            ApplySizeChange(ballSize);
        }
    }

    private void ApplySizeChange(BallSizeController ballSize)
    {
        switch (zoneType)
        {
            case SizeZoneType.Shrink:
                ballSize.Shrink();
                break;
            case SizeZoneType.Grow:
                ballSize.Grow();
                break;
        }
    }

    public void SetZoneType(SizeZoneType type)
    {
        zoneType = type;
    }
}
