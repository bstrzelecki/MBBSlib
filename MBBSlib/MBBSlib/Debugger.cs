﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace MBBSlib
{
    public static class Debugger
    {
        [DllImport("kernel32")]
        private static extern bool AllocConsole();
        public delegate void Command(CommandCompund cmd);
        public static event Command OnCmd;
        private static List<string> cmds = new List<string>();
        /// <summary>
        /// Opens new window console (Closing console window will close entire application)
        /// </summary>
        public static void OpenConsole()
        {
            AllocConsole();
            Thread th = new Thread(() =>
            {
                while (true)
                {
                    cmds.Add(Console.ReadLine());
                }
            });
            th.Start();
        }
        /// <summary>
        /// Executes queued commands
        /// </summary>
        public static void ExecuteCommands()
        {
            foreach (string cmd in cmds)
            {
                try
                {
                    string[] rg = cmd.Split('.');
                    string[] arg = rg[1].Split(' ');
                    List<string> c = new List<string>();
                    foreach (string n in arg)
                    {
                        c.Add(n);
                    }
                    string t = c[0];
                    c.RemoveAt(0);
                    CommandCompund cp = new CommandCompund(rg[0], t, c.ToArray());
                    OnCmd(cp);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Command:" + cmd + " " + e.ToString());
                }
            }
            cmds.Clear();
        }
    }

    public class CommandCompund
    {
        /// <summary>
        /// Targetted class (unse check function instead of this property) 
        /// </summary>
        public string Target { get; set; }
        /// <summary>
        /// Name of referenced class property (use this to check which property is going to be changed)
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// Array of arguments in commands
        /// </summary>
        public string[] Values { get; set; }

        internal CommandCompund(string dest, string res, params string[] val)
        {
            Target = dest;
            Source = res;
            Values = val;
        }
        /// <summary>
        /// Check name of target
        /// </summary>
        /// <param name="a">Name of sender class</param>
        /// <returns>Returns confirmation of vatidation sender class</returns>
        public bool Check(string a)
        {
            return Target == a;
        }
        public static implicit operator String(CommandCompund command)
        {
            return command.Source;
        }
        /// <summary>
        /// Gets Int32 value if specific argument
        /// </summary>
        /// <param name="i">index of argument form 0</param>
        /// <returns>Int32</returns>
        public int GetInt(int i)
        {
            int temp;
            if (int.TryParse(Values[i], out temp))
            {
                return temp;
            }
            else
            {
                Debug.WriteLine("Cant convert value to int !!!!");
            }
            return 0;
        }
        /// <summary>
        /// Gets Boolean value if specific argument
        /// </summary>
        /// <param name="i">index of argument form 0</param>
        /// <returns>Boolean</returns>
        public bool GetBool(int i)
        {
            bool temp;
            if (bool.TryParse(Values[i], out temp))
            {
                return temp;
            }
            else
            {
                Debug.WriteLine("Cant convert value to bool !!!!");
            }
            return false;
        }
    }
}
