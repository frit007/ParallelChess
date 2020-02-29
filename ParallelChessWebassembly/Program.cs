using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChessWebassembly {
    class Program {
        public static void Main() {
            Console.WriteLine(GlobalChess.GetState());
            GlobalChess.StartWorker();
            GlobalChess.AiPlayMove();

            while (GlobalChess.IsWorking()) {
                Console.WriteLine("working: " + GlobalChess.IsWorking());
                
                Console.ReadKey();
            }
            Console.WriteLine(GlobalChess.GetState());
            Console.ReadKey();
            Console.ReadKey();
            Console.ReadKey();
            Console.ReadKey();
            Console.ReadKey();
        }
    }
}
