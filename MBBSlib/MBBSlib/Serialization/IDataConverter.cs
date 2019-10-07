using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace MBBSlib.Serialization
{
    interface IDataConverter<T>
    {
        XElement Serialize();
        void ConvertBack();
    }
}
