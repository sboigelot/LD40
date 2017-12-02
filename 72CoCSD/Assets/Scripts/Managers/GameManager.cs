using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Model;

namespace Assets.Scripts.Managers
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public Game Game;
        public Player Player;

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
    }
}
