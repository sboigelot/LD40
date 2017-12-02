using Assets.Scripts.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public Game Game;
        public Player Player;
        public AnimationCurve WordLengths;
            
        public void Start()
        {
            NewGame();
        }

        public void NewGame()
        {
            Player = new Player();
            Game = new Game();
            Game.Initialize();
        }
        
        public void Update()
        {
            Game.Update(Time.deltaTime);
        }
    }
}
