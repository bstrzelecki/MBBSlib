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
        public static event Action<Achievement> OnAchievementTriggered;
        public Achievement(string title, string desc)
        {
            Title = title;
            Descrition = desc;
        }
        public Achievement(XElement data)
        {
            Title = data.Element("Title").Value;
            Descrition = data.Element("Desc").Value;
            Key = data.Element("Key").Value;
            IsTriggered = bool.Parse(data.Element("IsTriggered").Value);
        }
        public void Trigger()
        {
            IsTriggered = true;
            OnAchievementTriggered?.Invoke(this);
        }
        public XElement Serialize()
        {
            return new XElement("Achievement", new XElement("Title", Title)
                                             , new XElement("Desc", Descrition)
                                             , new XElement("Key", Key)
                                             , new XElement("IsTriggered", IsTriggered));
        }

    }
}
