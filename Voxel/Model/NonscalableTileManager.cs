using Ace.Files.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.Model
{
    sealed class NonscalableTileManager : TileManager
    {

        private NonscalableTile tile = new NonscalableTile();
        public NonscalableTile Tile
        {
            get => tile;
            //set
            //{
            //    tile = value;
            //    OnPropertyChanged(nameof(Tile));
            //}
        }

        public override void LoadData()
        {
            base.LoadData();
            if (data[TypeKey].StringValue != nameof(NonscalableTile))
            {
                throw new TileTypeNotMatchException();
            }
            tile.SmallImagePath = data[nameof(tile.SmallImagePath)].StringValue;
            tile.LargeImagePath = data[nameof(tile.LargeImagePath)].StringValue;
            tile.ShowName = data[nameof(tile.ShowName)].BooleanValue.Value;
            tile.Background = ((int) data[nameof(tile.Background)].NumberValue).ToColor();
            tile.TargetPath = data[nameof(tile.TargetPath)].StringValue;
            tile.XmlPath = data[nameof(tile.XmlPath)].StringValue;
            string themeString = data[nameof(tile.Theme)].StringValue;
            tile.Theme = themeString == DarkThemeString ? TextTheme.Dark : TextTheme.Light;
        }
        public override void SaveData()
        {
            data = new JsonObject
            {
                [TypeKey] = nameof(NonscalableTile),
                [nameof(tile.SmallImagePath)] = tile.SmallImagePath,
                [nameof(tile.LargeImagePath)] = tile.LargeImagePath,
                [nameof(tile.ShowName)] = tile.ShowName,
                [nameof(tile.Background)] = tile.Background.ToInt32(),
                [nameof(tile.TargetPath)] = tile.TargetPath,
                [nameof(tile.XmlPath)] = tile.XmlPath,
                [nameof(tile.Theme)] = tile.Theme == TextTheme.Dark ? DarkThemeString : LightThemeString,
            };
            base.SaveData();
        }
    }
}
