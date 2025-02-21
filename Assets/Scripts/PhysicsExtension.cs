using UnityEngine;

public class PhysicsExtension
{
    static public bool ArcCast(Vector3 center, Quaternion rotation, 
                               float angle, float radius, int resolution, 
                               LayerMask layer, out RaycastHit hit, 
                               Quaternion rotationOffset, Vector3 positionOffset)
    {
        // Apply position offset
        center += positionOffset;

        // Apply rotation offset
        rotation *= rotationOffset;
        rotation *= Quaternion.Euler(-angle / 2, 0, 0);

        for (int i = 0; i < resolution; i++)
        {
            Vector3 A = center + rotation * Vector3.forward * radius;
            rotation *= Quaternion.Euler(angle / resolution, 0, 0);
            Vector3 B = center + rotation * Vector3.forward * radius;
            Vector3 AB = B - A;

            if (Physics.Raycast(A, AB, out hit, AB.magnitude * 1.001f, layer))
                return true;
        }

        hit = new RaycastHit();
        return false;
    }

    // Function to draw the ArcCast visualization
    static public void DrawArcGizmo(Vector3 center, Quaternion rotation, 
                                    float angle, float radius, int resolution, 
                                    LayerMask layer, Quaternion rotationOffset, Vector3 positionOffset)
    {
        // Apply position offset
        center += positionOffset;

        // Apply rotation offset
        rotation *= rotationOffset;
        rotation *= Quaternion.Euler(-angle / 2, 0, 0);
        Vector3 prevPoint = center + rotation * Vector3.forward * radius;

        for (int i = 0; i < resolution; i++)
        {
            rotation *= Quaternion.Euler(angle / resolution, 0, 0);
            Vector3 nextPoint = center + rotation * Vector3.forward * radius;

            // Draw arc segment
            Gizmos.color = Color.green;
            Gizmos.DrawLine(prevPoint, nextPoint);

            // Perform raycast
            Vector3 direction = (nextPoint - prevPoint).normalized;
            float distance = (nextPoint - prevPoint).magnitude * 1.001f;
            if (Physics.Raycast(prevPoint, direction, out RaycastHit hit, distance, layer))
            {
                Gizmos.color = Color.red; // Hit detected
                Gizmos.DrawSphere(hit.point, 0.1f);
            }

            prevPoint = nextPoint;
        }
    }
}
