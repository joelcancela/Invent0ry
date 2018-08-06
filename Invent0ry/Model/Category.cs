using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Invent0ry.Model
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Category
    {
        Auto,
        Wire,
        Phone,
        PC,
        Audio,
        Video,
        Gaming,
        Sticker,
        Goodie,
        Other
    }
}
