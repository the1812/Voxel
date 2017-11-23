using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel
{
    static class Contributors
    {
        private static List<string> names = new List<string>
        {
            "Grant Howard"
        };
        public static List<string> GetNames()
        {
            names.Sort();
            return names;
        }
    }
}
