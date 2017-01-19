using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrom_trail1
    {
    //Will handle the life cycle of a single carrom game
    class Game
        {
        Player playerOne;
        Player playerTwo;

        //Reset game stage here, all coins must be at initial position
        //striker must be in Player 1's hand
        public void BeginGame ()
            {
            }

        //All standard rules that applies in the turn must apply here
        //PhysicsEngine's HitCarromObject will be called here
        public void Turn ()
            {

            }

        //Set variables for next turn
        // e.g. 1. Switch Player
        //      2. Striker must be in another player's hand or 
        //         in the same player's hand depending on the rules
        public void NextTurn ()
            {
            }
        
        //Game will end here and it shuld return winner Player of the game
        public Player EndGame ()
            {
            // Find max score and return respective Player
            Player p = new Player ("ONE");
            return p;
            }
        }
    }
