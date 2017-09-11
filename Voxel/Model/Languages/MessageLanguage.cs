using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.Model.Languages
{
    sealed class MessageLanguage : Language
    {
        public MessageLanguage() : base()
        {
        }

        protected override void Load()
        {
            keys = new List<string>
            {
                "WindowTitle",
            };
            simplifiedChinese = new List<string>
            {
                "消息",
            };
            americanEnglish = new List<string>
            {
                "Message",
            };
        }
    }
}
