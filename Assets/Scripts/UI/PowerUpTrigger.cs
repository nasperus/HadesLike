using UnityEngine;
using System.Collections;

namespace UI
{
    public class PowerUpTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject powerUpUi;
        

        private void Start()
        {
            //StartCoroutine(TriggerPowerUpPanel());
        }
        // private IEnumerator TriggerPowerUpPanel()
        // {
        //     while (true)
        //     {
        //         yield return new WaitForSeconds(2f); 
        //
        //         //powerUpUi.SetActive(true);
        //         //Time.timeScale = 0f;
        //     }
        //   
        // }

        //    public void OnPowerUpChosen()
        //    {
        //        
        //        //powerUpUi.SetActive(false); 
        //        //Time.timeScale = 1f; 
        //    }
        // }
    }
}
