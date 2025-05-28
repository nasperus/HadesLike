using System.Collections;
using Enemy.Archer;
using UnityEngine;
using Stats;

namespace Player.Skills
{
    public class DebuffDamage : MonoBehaviour
    {
        private float _damagePerTick;
        private float _tickInterval;
        private float _duration;
        private float _criticalChance;

        private Coroutine _dotCoroutine;
        private IEnemyDamageable _damageable;
        private StatCollection _statCollection;

        private void Awake()
        {
            _damageable = GetComponent<IEnemyDamageable>();
        }

        public void Init(float baseTickDamage, float tickInterval, float duration,  StatCollection stats)
        {
            _damagePerTick = ApplyStatsToAbilities.ApplyMastery(baseTickDamage,stats);
            _tickInterval = tickInterval;
            _duration = duration;
            _statCollection = stats;
            

            if (_dotCoroutine != null)
            {
                StopCoroutine(_dotCoroutine);
            }

            _dotCoroutine = StartCoroutine(ApplyDamageOverTime());
        }



        private IEnumerator ApplyDamageOverTime()
        {
            var fullTickCount = Mathf.FloorToInt(_duration / _tickInterval);
            var leftoverTime = _duration - (fullTickCount * _tickInterval);
            
            for (var i = 0; i < fullTickCount; i++)
            {
                DoTick();
                yield return new WaitForSeconds(_tickInterval);
            }
            
            if (leftoverTime > 0f)
            {
                Debug.Log("Leftover Damage");
                yield return new WaitForSeconds(leftoverTime);
                var ratio = leftoverTime / _tickInterval;
                DoTick(partial: true, ratio: ratio);
            }
            Destroy(this);
        }

        private void DoTick(bool partial = false, float ratio = 1f)
        {
            var damage = _damagePerTick * ratio;

           damage = ApplyStatsToAbilities.ApplyCritChance(damage, _statCollection);
            _damageable?.TakDamage(damage);
        }
    }
}
