using System;

namespace MBBSlib.Components {
    [AttributeUsage(AttributeTargets.Class)]
    public class CommonComponent : Attribute {
        public CommonComponent(){}
    }

    public enum ComponentType {
        Singleton,
        Static,
        Univerasal
    }
}