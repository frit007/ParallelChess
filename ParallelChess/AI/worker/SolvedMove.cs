using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChess.AI.worker {
    public class SolvedMove {
        public EvaluatedMove move;
        public Move startSolvingMove;
        public float min;
        public float max;
        public int taskId;

        // debug information
        public float startFromMin;
        public long durationMS;
    }
}
