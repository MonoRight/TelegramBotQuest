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
        public bool SecondMessage { get; set; } = false;
        public bool ThirdMessage { get; set; } = false;
        public bool FourthMessage { get; set; } = false;
    }
}
