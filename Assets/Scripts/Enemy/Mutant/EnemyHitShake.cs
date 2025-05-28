// using UnityEngine;
// using DG.Tweening;
//
// namespace Enemy
// {
//     public class EnemyHitShake : MonoBehaviour
//     {
//
//         [SerializeField] private Transform visualTransform;
//         private Vector3 _originalLocalPos;
//
//         private void Awake()
//         {
//             if (visualTransform == null)
//                 visualTransform = transform; // fallback
//
//             _originalLocalPos = visualTransform.localPosition;
//         }
//
//         public void ShakeOnHit()
//         {
//             visualTransform.DOKill(); // stop any existing shakes to avoid conflicts
//
//             // Shake visual transform local position
//             visualTransform.DOShakePosition(
//                 duration: 0.3f,
//                 strength: new Vector3(0.2f, 0.2f, 0),
//                 vibrato: 10,
//                 randomness: 90,
//                 snapping: false,
//                 fadeOut: true
//             ).OnComplete(() =>
//             {
//                 // Reset local position after shake
//                 visualTransform.localPosition = _originalLocalPos;
//             });
//         }
//
//     }
// }
