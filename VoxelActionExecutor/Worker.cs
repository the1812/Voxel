using Ace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.ActionExecutor
{
    static class Worker
    {
        public static void Run()
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
