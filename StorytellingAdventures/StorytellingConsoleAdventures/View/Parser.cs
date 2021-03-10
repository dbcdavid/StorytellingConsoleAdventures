using System;
using System.Collections.Generic;
using System.Text;

namespace StorytellingConsoleAdventures.View
{
    static class Parser
    {
        public static bool ParseAction(string line, ref string[] tokens, ref string message)
        {
            string[] lineTokens = line.Split(" ");

            if (lineTokens.Length > 0)
            {
                string actionName = lineTokens[0].ToLower();

                if (actionName.Equals(Commands.NORTH) || actionName.Equals(Commands.SOUTH) || actionName.Equals(Commands.WEST) || actionName.Equals(Commands.EAST))
                {
                    if (lineTokens.Length > 1)
                    {
                        message = line + " " + Messages.INVALIDCOMMAND;
                        return false;
                    }
                    else
                    {
                        tokens = lineTokens;
                        return true;
                    }
                }

                else if (actionName.Equals(Commands.SEARCH))
                {
                    if (lineTokens.Length > 1)
                    {
                        message = line + " " + Messages.INVALIDCOMMAND;
                        return false;
                    }
                    else
                    {
                        tokens = lineTokens;
                        return true;
                    }
                }

                else if (actionName.Equals(Commands.GET))
                {
                    if (lineTokens.Length < 2)
                    {
                        message = Messages.GETWHATMESSAGE;
                        return false;
                    }
                    else if (lineTokens.Length == 2)
                    {
                        tokens = lineTokens;
                        return true;
                    }
                    else
                    {
                        message = line + " " + Messages.INVALIDCOMMAND;
                        return false;
                    }
                }

                else if (actionName.Equals(Commands.USE))
                {
                    if (lineTokens.Length != 2)
                    {
                        message = line + " " + Messages.INVALIDCOMMAND;
                        return false;
                    }
                    else
                    {
                        tokens = lineTokens;
                        return true;
                    }
                }

                else if (actionName.Equals(Commands.SAVE))
                {
                    if (lineTokens.Length != 1)
                    {
                        message = line + " " + Messages.INVALIDCOMMAND;
                        return false;
                    }
                    else
                    {
                        tokens = lineTokens;
                        return true;
                    }
                }

                else if (actionName.Equals(Commands.LOAD))
                {
                    if (lineTokens.Length != 1)
                    {
                        message = line + " " + Messages.INVALIDCOMMAND;
                        return false;
                    }
                    else
                    {
                        tokens = lineTokens;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
