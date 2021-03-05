using System;
using System.Collections.Generic;
using System.Text;

namespace StorytellingConsoleAdventures.View
{
    static class Messages
    {
        public static string NOUSEMESSAGE = "cannot be used here";
        public static string MOVEMESSAGE = "%entity went from %from to %to";
        public static string MOVEFAILMESSAGE = "%entity cannot go %direction";

        public static string PLAYERNEARMONSTER = "You feel an evil presence nearby...";
        public static string PLAYERWITHMONSTER = "THE MONSTER IS HERE!!!";

        public static string GAMEOVERMESSAGE = "You died...";
        public static string CONTINUEQUESTION = "Do you wish to try again? (Y/N)";

        public static string CONTINUEMESSAGE = "You are determined... So let's try again";
        public static string NOTCONTINUEMESSAGE = "We will wait your return";
        public static string REPLAYMESSAGE = "Let's do it again. From the beggining";

        public static string SUCCESSMESSAGE = "You killed the monster! Congratulations!";
        public static string PLAYAGAINQUESTION = "Do you wish to play again? (Y/N)";

        public static string FOUNDMESSAGE = "found";
        public static string EMPTYLOCATIONMESSAGE = "Nothing found";

        public static string GOTITEMMESSAGE = "got";
        public static string DIDNOTGOTITEMMESSAGE = "can't get";

        public static string HASNOITEM = "doesn't have";
    }
}
