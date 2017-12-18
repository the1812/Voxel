using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ace.Files.Json;

namespace Voxel.ActionExecutor
{
    sealed class Settings
    {
        public const string FileName = "settings.json", ActionKey = "Action", DefaultAction = "none";
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
