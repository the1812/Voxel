using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel.Model
{
    sealed class ImageTileManager : Model, IEnumerable<ImageTile>, ICollection<ImageTile>
    {
        private List<ImageTile> list;
        public ImageTileManager(IEnumerable<ImageTile> imageTiles)
        {
            list = new List<ImageTile>(imageTiles);
        }
#region 接口
        public ImageTileManager(params ImageTile[] imageTiles) : this((IEnumerable<ImageTile>) imageTiles) { }

        public int Count => list.Count;

        public bool IsReadOnly => false;

        public void Add(ImageTile item) => list.Add(item);

        public void Clear() => list.Clear();

        public bool Contains(ImageTile item) => list.Contains(item);

        public void CopyTo(ImageTile[] array, int arrayIndex)
        => list.CopyTo(array, arrayIndex);

        public IEnumerator<ImageTile> GetEnumerator() => list.GetEnumerator();

        public bool Remove(ImageTile item) => list.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
#endregion

    }
}
