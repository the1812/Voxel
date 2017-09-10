using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel.View;
using Voxel.Model.Languages;

namespace Voxel.ViewModel
{
    sealed class MainViewModel : ViewModel
    {
        public MainViewModel(MainView mainView) : base(new MainLanguage())
        {
            Window = mainView;
        }

        public MainView Window { get; private set; }


        public string WindowTitle => language[nameof(WindowTitle)];
        public string ButtonNonscalableTile => language[nameof(ButtonNonscalableTile)];
        public string ButtonScalableTile => language[nameof(ButtonScalableTile)];
        public string ButtonImageTile => language[nameof(ButtonImageTile)];
        public string ButtonAbout => language[nameof(ButtonAbout)];

        public Command ButtonNonscalableTileCommand => new Command
        {
            CanExecuteAction = (o) => true,
            ExcuteAction = (o) =>
            {
                var window = new NonscalableTileView
                {
                    Owner = Window
                };
                window.ShowDialog();
            },
        };
    }
}
