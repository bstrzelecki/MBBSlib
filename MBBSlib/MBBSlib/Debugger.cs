using System;
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
        private static readonly List<string> _cmds = new List<string>();
        /// <summary>
        /// Opens new window console (Closing console window will close entire application)
        /// </summary>
        public static void OpenConsole()
        {
            AllocConsole();
            var th = new Thread(() =>
            {
                while (true)
                {
                    _cmds.Add(Console.ReadLine());
                }
            });
            th.Start();
        }
        /// <summary>
        /// Executes queued commands
        /// </summary>
        public static void ExecuteCommands()
        {
            foreach (string cmd in _cmds)
            {
                try
                {
                    string[] rg = cmd.Split('.');
                    string[] arg = rg[1].Split(' ');
                    var c = new List<string>();
                    foreach (string n in arg)
                    {
                        c.Add(n);
                    }
                    string t = c[0];
                    c.RemoveAt(0);
                    var cp = new CommandCompund(rg[0], t, c.ToArray());
                    OnCmd(cp);
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Command:{cmd} {e}");
                }
            }
            _cmds.Clear();
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
        public bool Check(string a) => Target == a;
        public static implicit operator String(CommandCompund command) => command.Source;
        /// <summary>
        /// Gets Int32 value if specific argument
        /// </summary>
        /// <param name="i">index of argument form 0</param>
        /// <returns>Int32</returns>
        public int GetInt(int i)
        {
            if (int.TryParse(Values[i], out int temp))
            {
                return temp;
            }
            else
            {
                Debug.WriteLine($"Cant convert {Values[i]} to bool !!!!");
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
            if (bool.TryParse(Values[i], out bool temp))
            {
                return temp;
            }
            else
            {
                Debug.WriteLine($"Cant convert {Values[i]} to bool !!!!");
            }
            return false;
        }
    }
}
