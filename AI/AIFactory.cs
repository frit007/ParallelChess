using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChess.AI {
    interface AIFactory {

        string Name();

        AI Create(List<AIOption> options);

        List<AIOption> configureable();
    }
}
