using ParallelChess;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChess.AI {
    interface AI {

        Move GetMove(Chess game);

    }
}
