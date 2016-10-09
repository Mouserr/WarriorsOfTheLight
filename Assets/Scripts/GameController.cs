using Assets.Events;
using Assets.Scripts.Buildings;
using Assets.Scripts.Configs;
using Assets.Scripts.Core.Events;
using Assets.Scripts.Core.Scenarios;
using Assets.Scripts.UI;
using Assets.Scripts.UI.SelectionPanels;
using Assets.Scripts.Units;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private LightTreeController lightTreeController;
        [SerializeField] private MobsSpawnController spawnController;
        [SerializeField] private HeroConfig heroConfig;
        [SerializeField] private HeroController heroController;
        [SerializeField] private GameOverPopup gameOverPopup;
        [SerializeField] private TipPopup tipPopup;
        [SerializeField] private int userPlayerId;
        [SerializeField] private int mobsPlayerId;
        [SerializeField] private int prespawnTime;
        [SerializeField] private int startGoldAmount;
        

        private void Awake()
        {
            PlayerManager.Instance.AddPlayer(mobsPlayerId);
            PlayerManager.Instance.AddPlayer(userPlayerId, true);
            
            lightTreeController.OnHPChange += onLightTreeHpChange;
            MapController.Instance.NoMoreUnitsAtMap += noMoreUnitsAtMap;
            
            StartGame();
        }

        public void StartGame()
        {
            PlayerManager.Instance.UserPlayer.ProcessAward(startGoldAmount);

            Hero hero = new Hero(UnitType.Hero, heroConfig, userPlayerId);
            heroController.Unit = hero;
            MapController.Instance.AddUnit(heroController);
            tipPopup.Show("Prepare Youself!");
            new Scenario(
                new IterateItem(prespawnTime),
                new ActionItem(startSpawn)).Play();
        }

        private void startSpawn()
        {
            tipPopup.Show("THEY'RE COMMING!");
            spawnController.StartSpawn();
            MapController.Instance.StartSpawn();
        }

        private void noMoreUnitsAtMap(int playerId)
        {
            if (playerId == mobsPlayerId && spawnController.SpawnEnded)
            {
                Debug.Log("Victory!");
                processGameOver(GameResult.Win);
            }
        }

        private void onLightTreeHpChange()
        {
            if (!lightTreeController.IsAlive)
            {
                Debug.Log("Defeat!");
                processGameOver(GameResult.Defeat);
            }
        }

        private void processGameOver(GameResult result)
        {
            GameEventManager.Instance.Raise(new GameOverEvent(result));
            
            gameOverPopup.Show(result, restart);
        }

        private void restart()
        {
            lightTreeController.OnHPChange -= onLightTreeHpChange;
            MapController.Instance.NoMoreUnitsAtMap -= noMoreUnitsAtMap;
            PlayerManager.Instance.Clear();
            MapController.Instance.Clear();
            UnitsPool.Instance.Clear();
            Application.LoadLevel(Application.loadedLevelName);
        }
    }
}