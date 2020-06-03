using System;
using System.Xml.Linq;

namespace MBBSlib.Integrations
{
    /// <summary>
    /// Archievement engine 
    /// </summary>
    public class Achievement
    {
        /// <summary>
        /// Display name of the <see cref="Achievement"/>
        /// </summary>
        public string Title { get; }
        /// <summary>
        /// Displayed description of <see cref="Achievement"/>
        /// </summary>
        public string Descrition { get; }
        /// <summary>
        /// Id of <see cref="Achievement"/>
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Is true when <see cref="Achievement"/> is already obtained
        /// </summary>
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
        public XElement Serialize() => new XElement("Achievement", new XElement("Title", Title)
                                             , new XElement("Desc", Descrition)
                                             , new XElement("Key", Key)
                                             , new XElement("IsTriggered", IsTriggered));
    }
}
