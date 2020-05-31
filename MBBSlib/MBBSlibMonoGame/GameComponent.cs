using System;

namespace MBBSlib.MonoGame
{
    /// <summary>
    /// Every <see cref="Object"/> with this attribute will be automaticly instantiated during <see cref="GameMain.Initialize()"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class GameComponent : Attribute
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public GameComponent()
        {

        }
    }
}
