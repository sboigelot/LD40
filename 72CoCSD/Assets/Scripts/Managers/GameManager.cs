using Assets.Scripts.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public Game Game;
        public Player Player;
        public AnimationCurve WordLengths;
        public PrototypeManager PrototypeManager;

        public void Start()
        {
            //make the prototypeManager visible in unity inspector
            PrototypeManager = PrototypeManager.Instance;
            StartCoroutine(PrototypeManager.LoadPrototypes(()=>NewGame()));
        }

        public void NewGame()
        {
            Player = new Player();
            Game = new Game();
            Game.Initialize();
        }
        
        public void Update()
        {
            if (Game != null)
            {
                Game.Update(Time.deltaTime);
            }
        }
    }
}
