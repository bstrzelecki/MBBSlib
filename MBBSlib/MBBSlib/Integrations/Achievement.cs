using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace MBBSlib.Integrations
{
    public class Achievement
    {
        public string Title { get; }
        public string Descrition { get; }
        public string Key { get; set; }
        public bool IsTriggered { get; protected set; }
        public event Action OnAchievementTriggered;
        public Achievement(string title, string desc)
        {
            Title = title;
            Descrition = desc;
        }
        public void Trigger()
        {
            IsTriggered = true;
            OnAchievementTriggered?.Invoke();
        }
        public XElement Serialize()
        {
            return new XElement("Hello, World");
        }
    }
}
