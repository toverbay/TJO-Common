using System;
using System.Collections.Generic;
using System.Text;

namespace TJO.Common
{
    // Comment formatting - I use CommentsPlus from mhoumann 
    // (https://marketplace.visualstudio.com/items?itemName=mhoumann.CommentsPlus)

    //! Important
    //? Question
    //x Removed
    //TODO: Task
    //TODO@Tim: Task for Tim
    //!? WAT!!??

    public enum LogLevel
    {
        Debug,
        Information,
        Warn,
        Error,
        Fatal
    }
}
