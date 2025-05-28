using UnityEngine;
using UnityEngine.AI;

namespace Enemy.Archer
{
    public class NavMeshDebugger : MonoBehaviour
    {
        [Header("Sampling Settings")]
        public float sampleRadius = 5f;
        public int numberOfSamples = 10;
        public float minRepositionDistance = 2f;

        [Header("Debug Settings")]
        public Color samplePointColor = Color.yellow;
        public Color validNavMeshPointColor = Color.green;
        public Color failedSampleColor = Color.red;
        public float gizmoSphereSize = 0.3f;

        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying) return;

            Vector3 origin = transform.position;

            for (int i = 0; i < numberOfSamples; i++)
            {
                // Generate random point in a circle
                Vector3 randomDirection = Random.insideUnitSphere * sampleRadius;
                randomDirection.y = 0;
                Vector3 samplePosition = origin + randomDirection;

                // Draw the raw sample position
                Gizmos.color = samplePointColor;
                Gizmos.DrawSphere(samplePosition, gizmoSphereSize);

                // Try to sample a point on the NavMesh
                if (NavMesh.SamplePosition(samplePosition, out NavMeshHit hit, sampleRadius, NavMesh.AllAreas))
                {
                    float distance = Vector3.Distance(hit.position, origin);

                    if (distance >= minRepositionDistance)
                    {
                        Gizmos.color = validNavMeshPointColor;
                        Gizmos.DrawLine(samplePosition, hit.position); // Line to valid NavMesh point
                        Gizmos.DrawSphere(hit.position, gizmoSphereSize); // Draw valid point
                    }
                    else
                    {
                        // Valid, but too close
                        Gizmos.color = Color.cyan;
                        Gizmos.DrawLine(samplePosition, hit.position);
                        Gizmos.DrawSphere(hit.position, gizmoSphereSize * 0.5f);
                    }
                }
                else
                {
                    // Failed sample
                    Gizmos.color = failedSampleColor;
                    Gizmos.DrawSphere(samplePosition, gizmoSphereSize * 0.5f);
                }
            }

            // Optional: Draw reposition radius circle
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(origin, sampleRadius);
        }
    }
}
