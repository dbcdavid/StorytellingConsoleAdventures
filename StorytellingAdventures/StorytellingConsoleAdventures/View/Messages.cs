using System;
using System.Collections.Generic;
using System.Text;

namespace StorytellingConsoleAdventures.View
{
    static class Messages
    {
        public static string INVALIDCOMMAND = "is not a valid action";

        public static string NOUSEMESSAGE = "cannot be used here";
        public static string MOVEMESSAGE = "%entity went from %from to %to";
        public static string MOVEFAILMESSAGE = "%entity cannot go %direction";

        public static string PLAYERNEARMONSTER = "You feel an evil presence nearby...";
        public static string PLAYERWITHMONSTER = "THE MONSTER IS HERE!!!";

        public static string GAMEOVERMESSAGE = "You died...";
        public static string CONTINUEQUESTION = "Do you wish to try again? (Y/N)";

        public static string MONSTERATTACK = "Before you can act, the monster attacks you!";
        public static string PLAYERINGOODCONDITION = "Despite the attack, you still feel strong enough to keep going";
        public static string PLAYERINBADCONDITION = "You start to feel weak as if it would be impossible to take another hit...";

        public static string CONTINUEMESSAGE = "You are determined... So let's try again";
        public static string NOTCONTINUEMESSAGE = "We will wait your return";
        public static string REPLAYMESSAGE = "Let's do it again. From the beggining";

        public static string SUCCESSMESSAGE = "You killed the monster! Congratulations!";
        public static string PLAYAGAINQUESTION = "Do you wish to play again? (Y/N)";

        public static string FOUNDMESSAGE = "found";
        public static string EMPTYLOCATIONMESSAGE = "Nothing found";

        public static string GETWHATMESSAGE = "get what?";
        public static string GOTITEMMESSAGE = "got";
        public static string DIDNOTGOTITEMMESSAGE = "can't get";
        public static string HASNOITEM = "doesn't have";

        public static string SAVESUCCESSFUL = "SAVED SUCCESSFULLY";
        public static string SAVEFAILED = "SAVE FAILED";
        public static string LOADSUCCESSFUL = "LOADED SUCCESSFULLY";
        public static string LOADFAILED = "LOAD FAILED";

        public static string ATTACKVERB = "attacked";
        public static string UNLOCKVERB = "unlocked";

    }
}
