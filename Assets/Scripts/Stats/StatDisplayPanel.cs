using Player;
using Player.Skills;
using TMPro;
using UnityEngine;

namespace Stats
{
    public class StatDisplayPanel : MonoBehaviour
    {
      [SerializeField] private TextMeshProUGUI statsText;
      [SerializeField] private StatCollection statCollection;
      [SerializeField] private PlayerHealth playerHealth;
      [SerializeField] private PlayerMana playerMana;
      [SerializeField] private PlayerMovement playerMovement;
      


      private void Update()
      {
          UpdateStatDisplay();
      }

      private void UpdateStatDisplay()
      {
          var haste = statCollection.GetStatValue(StatTypeEnum.Haste);
          var mastery = statCollection.GetStatValue(StatTypeEnum.Mastery);
          var crit = statCollection.GetStatValue(StatTypeEnum.Critical);
          var vit = statCollection.GetStatValue(StatTypeEnum.Vitality);
          var armor = statCollection.GetStatValue(StatTypeEnum.Armor);
          var mana = statCollection.GetStatValue(StatTypeEnum.Mana);
          var move = statCollection.GetStatValue(StatTypeEnum.MovementSpeed);
          var currentHealth = playerHealth.CurrentHealth;
          var maxHealth = playerHealth.MaxHealth;
          var currentMana = playerMana.CurrentMana;
          var maxMana = playerMana.MaxMana;
          var currentMovement = playerMovement.MovementSpeed;
          
          statsText.text =
              $"<b>Stats</b>\n" +
              $"Vitality: {vit} = <color=#44ff44>{maxHealth} Max Health</color> (<b>{currentHealth}</b>)\n" +
              $"Mana: {mana} = <color=#4444ff>{maxMana} Max Mana</color> (<b>{currentMana}</b>)\n" +
              $"Armor: {armor} = <color=#bbbbbb>{armor:0} Defense</color>\n" +
              $"Move Speed: {move} = <color=#ffaa00>{move * 1f:0.#}% Speed</color>(<b>{currentMovement}</b>)\n" +
              $"Haste: {haste} = <color=#00ffff>{haste * 3f:0.#}% Dot/Cast/Attack/CD/Duration</color>\n" +
              $"Mastery: {mastery} = <color=#ffcc00>{mastery * 1f:0.#}% All Damage</color>\n" +
              $"Crit: {crit} = <color=#ff4444>{crit * 1f:0.#}% Crit Chance</color>\n";




      }
    }
}
