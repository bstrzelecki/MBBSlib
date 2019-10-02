using System;
using System.Collections.Generic;
using System.Text;

namespace MBBSlib.Integrations
{
    class Achievement
    {
        public string Title { get; }
        public string Descrition { get; }
        public string Key { get; set; }
        public event Action OnAchievementTriggered;
        public Achievement(string title, string desc)
        {
            Title = title;
            Descrition = desc;
        }
        public void Trigger()
        {
            OnAchievementTriggered?.Invoke();
        }
    }
}
