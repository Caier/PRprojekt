using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CellLibrary {
    public static class Extensions {
        public static Guid FromMessage(this UUID uuid) {
            return new Guid(uuid.Value.Memory.ToArray());
        }

        public static UUID ToMessage(this Guid guid) {
            return new UUID { Value = ByteString.CopyFrom(guid.ToByteArray()) };
        }
    }
}
