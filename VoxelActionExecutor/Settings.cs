using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ace;
using Ace.Files.Json;

namespace Voxel.ActionExecutor
{
    sealed class Settings
    {
        public const string FileName = "settings.json", ActionKey = "Action", DefaultAction = "none";
        public bool IsSelfExecuteCommand
        {
            get
            {
                if (Action.StartsWith("start "))
                {
                    var fileName = Action.Remove(0, "start ".Length).NoQuotes();
                    var file = new FileInfo(fileName);
                    var selfPath = Process.GetCurrentProcess().MainModule.FileName.NoQuotes();
                    if (file.FullName.ToLower() == selfPath.ToLower())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }
        public Settings()
        {
            load();
        }

        private void load()
        {
            JsonFile file = new JsonFile(FileName);
            if (file.Exists)
            {
                file.Load();
                Action = file.Content[ActionKey].StringValue;
            }
            else
            {
                createDefault();
            }
        }

        public string Action { get; internal set; }
        private void createDefault()
        {
            JsonFile file = new JsonFile(FileName)
            {
                Content = new JsonObject
                {
                    new JsonProperty(ActionKey, DefaultAction),
                },
            };
            file.Save();
            Action = DefaultAction;
        }
    }
}
