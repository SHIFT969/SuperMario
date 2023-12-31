using UnityEngine;

public static class Extensions
{
    private static LayerMask layerMask = LayerMask.GetMask("Default");

    public static bool Raycast(this Rigidbody2D rigidbody, Vector2 direction)
    {
        if (rigidbody.isKinematic) {
            return false;
        }

        var radius = 0.25f;
        var distance = 0.375f;

        var hit = Physics2D.CircleCast(rigidbody.position, radius, direction.normalized, distance, layerMask);

        return hit.collider != null && hit.rigidbody != rigidbody;
    }

    public static bool DotTest(this Transform transform, Transform other, Vector2 testDirection)
    {
        var direction = other.position - transform.position;
        var dot = Vector2.Dot(direction.normalized, testDirection);
        return dot > 0.3f;
    }
}
