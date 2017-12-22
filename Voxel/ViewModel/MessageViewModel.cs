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
        //void showTitleOnly()
        //{
        //    TitleBackgroundOpacity = 1.0; //from 0
        //    IsContentVisible = false;// from true
        //    WindowHeight = 200.0; //from 650
        //}
        private string content;
        public string Content
        {
            get => content;
            set
            {
                content = value;
                //if (string.IsNullOrWhiteSpace(value))
                //{
                //    showTitleOnly();
                //}
                OnPropertyChanged(nameof(Content));
            }
        }

        private double titleBackgroundOpacity = 0.0;
        public double TitleBackgroundOpacity
        {
            get => titleBackgroundOpacity;
            set
            {
                titleBackgroundOpacity = value;
                OnPropertyChanged(nameof(TitleBackgroundOpacity));
            }
        }

        private bool isContentVisible = true;
        public bool IsContentVisible
        {
            get => isContentVisible;
            set
            {
                isContentVisible = value;
                OnPropertyChanged(nameof(IsContentVisible));
            }
        }

        //private double windowHeight = 650.0;
        //public double WindowHeight
        //{
        //    get => windowHeight;
        //    set
        //    {
        //        windowHeight = value;
        //        OnPropertyChanged(nameof(WindowHeight));
        //    }
        //}

    }
}
