using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelChess.AI {
    interface AIOption {
        // name of the configured item
        string Name();

        // default value or configured
        string Value();

        string Description();

        // if you want to limit options provide valid options here
        List<string> limitedOptions();
    }
}
