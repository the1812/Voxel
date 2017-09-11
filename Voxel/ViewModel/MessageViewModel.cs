using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.Model.Languages;

namespace Voxel.ViewModel
{
    sealed class MessageViewModel : ViewModel
    {
        public MessageViewModel() : base(new MessageLanguage())
        {
        }

        public string WindowTitle => language[nameof(WindowTitle)];



        private bool showCancelButton;
        public bool ShowCancelButton
        {
            get => showCancelButton;
            set
            {
                showCancelButton = value;
                OnPropertyChanged(nameof(ShowCancelButton));
            }
        }

        private string title;
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string content;
        public string Content
        {
            get => content;
            set
            {
                content = value;
                OnPropertyChanged(nameof(Content));
            }
        }

    }
}
