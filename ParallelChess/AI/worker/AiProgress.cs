using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChess.AI.worker {
    public class AIProgress {
        public int total;
        public int progress;
        public float foundScore;
        public int depth;
        public SolvedMove move;
    }
}
