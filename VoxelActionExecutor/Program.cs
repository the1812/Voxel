using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ace;

namespace Voxel.ActionExecutor
{
    class Program
    {
        public static void Main(string[] args)
        {
            Settings settings = new Settings();
            if (string.IsNullOrWhiteSpace(settings.Action) || 
                settings.Action == Settings.DefaultAction)
            {
                return;
            }
            Command.Run(settings.Action);
        }
    }
}
