using System;
using System.Collections;
using Enemy.Mutant;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy.Warlock_Boss
{
    public class WarlockStateMachine : EnemyStateMachine
    {
        [field: SerializeField] public GameObject[] SkillPrefabs { get; set; }
      
        
        public int CurrentSkillIndex { get; private set; }


        private void Start()
        {
            TransitionToState(new WarlockMoveState(this));
        }

       

        public void ChooseRandomSkill()
        {
            CurrentSkillIndex = Random.Range(0, SkillPrefabs.Length);
            
        }

        public void UseCurrentSkill()
        {
            UseSkills(CurrentSkillIndex);
        }
        private void UseSkills(int index)
        {
            switch (index)
            {
                case 0:
                    UseIceMeteor();
                    break;
                case 1:
                    UseLaserBeam();
                    break;
                case 2:
                    UseRedStone();
                    break;
            }
          
        }
        

        private void UseIceMeteor()
        {
             var obj = Instantiate(SkillPrefabs[0], Player.position, transform.rotation);
             Destroy(obj.gameObject, 2f);
        }
        
        private void UseLaserBeam()
        {
            var obj = Instantiate(SkillPrefabs[1], Player.position, transform.rotation);
            Destroy(obj.gameObject, 2f);
        }
        
        private void UseRedStone()
        {
            var obj = Instantiate(SkillPrefabs[2], Player.position, transform.rotation);
            Destroy(obj.gameObject, 2f);
        }
        
    }
}
    