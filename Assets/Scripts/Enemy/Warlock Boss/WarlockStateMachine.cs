using Enemy.Mutant;
using UnityEngine;

namespace Enemy.Warlock_Boss
{
    public class WarlockStateMachine : EnemyStateMachine
    {
        [field: SerializeField] public GameObject[] SkillPrefabs { get; set; }


        private void Start()
        {
            TransitionToState(new WarlockMoveState(this));
        }


        public void UseSkills()
        {
            
            var obj = Instantiate(SkillPrefabs[Random.Range(0, SkillPrefabs.Length)], Player.position, transform.rotation);
            Destroy(obj.gameObject, 1f);
        }
    }
}
    