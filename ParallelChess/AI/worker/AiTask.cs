using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChess.AI.worker {
    public class AITask {
        public int taskId;
        public Board board;
        public List<List<Move>> moves;
        public int depth;
        public Action<SolvedMove> onMoveComplete;
        internal HashSet<ulong> tiedPositions;
    }
}
