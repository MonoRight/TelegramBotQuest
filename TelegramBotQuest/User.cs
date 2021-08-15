using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBotQuest
{
    class User
    {
        public long UsersIds { get; set; }
        public int ProgressLevel { get; set; } = 0;
        public bool FirstMessage { get; set; } = false;
        public int FirstMessageFirstTryTrue = 0;
        public bool SecondMessage { get; set; } = false;
        public int SecondMessageFirstTryTrue = 0;
        public bool ThirdMessage { get; set; } = false;
        public int ThirdMessageFirstTryTrue = 0;
        public bool FourthMessage { get; set; } = false;
        public int FourthMessageFirstTryTrue = 0;
    }
}
