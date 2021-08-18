using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerGen.Api
{
    public static class GuidEncoder
    {
        public static string Encode(Guid guid)
        {
            var encoded = Convert.ToBase64String(guid.ToByteArray());
            encoded = encoded.Replace('/', '_');
            encoded = encoded.Replace('+', '-');
            return encoded.Substring(0, 22);
        }

        public static Guid Decode(string encodedGuid)
        {
            encodedGuid = encodedGuid.Replace('_', '/');
            encodedGuid = encodedGuid.Replace('-', '+');
            var bytes = Convert.FromBase64String(encodedGuid + "==");
            return new Guid(bytes);
        }
    }
}
