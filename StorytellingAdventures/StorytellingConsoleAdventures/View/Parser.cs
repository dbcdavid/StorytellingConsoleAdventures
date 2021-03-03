﻿using System;
using System.Collections.Generic;
using System.Text;

namespace StorytellingConsoleAdventures.View
{
    static class Parser
    {
        public static bool ParseAction(string line, ref string[] tokens)
        {
            string[] lineTokens = line.Split(" ");

            if (lineTokens.Length > 0)
            {
                string actionName = lineTokens[0].ToLower();

                if (actionName.Equals("north") || actionName.Equals("south") || actionName.Equals("west") || actionName.Equals("east"))
                {
                    if (lineTokens.Length > 1)
                    {
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
