using Player;
using Player.Skills;
using TMPro;
using UnityEngine;

namespace Stats
{
    public class StatDisplayPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statsText;

        private StatCollection statCollection;
        private PlayerHealth playerHealth;
        private PlayerMana playerMana;
        private PlayerMovement playerMovement;

        // Cache last displayed values to avoid unnecessary updates (optional)
        private float lastHaste, lastMastery, lastCrit, lastVit, lastArmor, lastMana, lastMove;
        private float lastCurrentHealth, lastMaxHealth, lastCurrentMana, lastMaxMana, lastCurrentMovement;

        public void SetPlayerReferences(GameObject player)
        {
            statCollection = player.GetComponent<StatCollection>();
            playerHealth = player.GetComponent<PlayerHealth>();
            playerMana = player.GetComponent<PlayerMana>();
            playerMovement = player.GetComponent<PlayerMovement>();

            UpdateStatDisplay(); 
        }

        private void Update()
        {
            if (statCollection == null) return;
            
            if (CheckForStatChanges())
            {
                UpdateStatDisplay();
            }
        }

        private bool CheckForStatChanges()
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

            bool changed = !Mathf.Approximately(haste, lastHaste) ||
                           !Mathf.Approximately(mastery, lastMastery) ||
                           !Mathf.Approximately(crit, lastCrit) ||
                           !Mathf.Approximately(vit, lastVit) ||
                           !Mathf.Approximately(armor, lastArmor) ||
                           !Mathf.Approximately(mana, lastMana) ||
                           !Mathf.Approximately(move, lastMove) ||
                           !Mathf.Approximately(currentHealth, lastCurrentHealth) ||
                           !Mathf.Approximately(maxHealth, lastMaxHealth) ||
                           !Mathf.Approximately(currentMana, lastCurrentMana) ||
                           !Mathf.Approximately(maxMana, lastMaxMana) ||
                           !Mathf.Approximately(currentMovement, lastCurrentMovement);

            if (changed)
            {
                lastHaste = haste;
                lastMastery = mastery;
                lastCrit = crit;
                lastVit = vit;
                lastArmor = armor;
                lastMana = mana;
                lastMove = move;
                lastCurrentHealth = currentHealth;
                lastMaxHealth = maxHealth;
                lastCurrentMana = currentMana;
                lastMaxMana = maxMana;
                lastCurrentMovement = currentMovement;
            }

            return changed;
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
                $"Move Speed: {move} = <color=#ffaa00>{move:0.#}% Speed</color>(<b>{currentMovement}</b>)\n" +
                $"Haste: {haste} = <color=#00ffff>{haste * 3f:0.#}% Dot/Cast/Attack/CD/Duration</color>\n" +
                $"Mastery: {mastery} = <color=#ffcc00>{mastery * 1f:0.#}% All Damage</color>\n" +
                $"Crit: {crit} = <color=#ff4444>{crit * 1f:0.#}% Crit Chance</color>\n";
        }
    }
}
