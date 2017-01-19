using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carrom_trail1
    {
    class Game
        {
        Player playerOne;
        Player playerTwo;

        public void BeginGame ()
            {
            }

        public void ApplyStandardRulesToGame ()
            {
            }
        
        
        public Player EndGame ()
            {
            // Find max score and return respective Player
            Player p = new Player ("ONE");
            return p;
            }
        }
    }
