using System.Collections;
using UnityEngine;

namespace VFX
{
    public class DebuffVFXHandler : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        private float _lifetime;
        private Coroutine _despawnRoutine;

        public void Init(ParticleSystem ps, float lifetime)
        {
            _particleSystem = ps;
            _lifetime = lifetime;

            if (_particleSystem != null)
            {
                _particleSystem.Play();
                _despawnRoutine = StartCoroutine(DestroyAfterLifetime());
            }
        }

        public void ResetTimer(float newLifetime)
        {
            _lifetime = newLifetime;

            if (_despawnRoutine != null)
            {
                StopCoroutine(_despawnRoutine);
            }

            if (_particleSystem != null && !_particleSystem.isPlaying)
            {
                _particleSystem.Play();
            }

            _despawnRoutine = StartCoroutine(DestroyAfterLifetime());
        }

        private IEnumerator DestroyAfterLifetime()
        {
            yield return new WaitForSeconds(_lifetime);

            if (_particleSystem != null)
            {
                _particleSystem.Stop();
                Destroy(_particleSystem.gameObject);
            }

            Destroy(this); // clean up handler too
        }
    }
}