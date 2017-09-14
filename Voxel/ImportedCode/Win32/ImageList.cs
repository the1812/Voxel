using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using Ace.Win32.Enumerations;
using Ace.Win32.Structures;

namespace Ace.Win32
{
    /// <summary>
    /// 表示Win32图像列表（有删减，只留下图标获取相关代码）
    /// </summary>
    class ImageList : IDisposable
    {
        #region 私有成员
        private IntPtr imageListHandle = IntPtr.Zero;
        private IImageList imageListInterface = null;
        private IconSize iconSize = IconSize.Maximum;
        private bool disposed = false;
        #endregion

        #region 方法
        /// <summary>
        /// 根据索引从图像列表中取出对应的图标
        /// </summary>
        /// <param name="index">指定的索引</param>
        /// <returns>对应的图标</returns>
        public Icon GetIcon(int index)
        {
            Icon icon = null;

            IntPtr iconHandle = IntPtr.Zero;
            if (imageListInterface == null)
            {
                iconHandle = Api.ImageListGetIcon(
                    imageListHandle,
                    index,
                    (int)ImageListDrawItem.Transparent);

            }
            else
            {
                imageListInterface.GetIcon(
                    index,
                    (int)ImageListDrawItem.Transparent,
                    ref iconHandle);
            }

            if (iconHandle != IntPtr.Zero)
            {
                icon = Icon.FromHandle(iconHandle);
            }
            return icon;
        }

        /// <summary>
        /// 获取文件中图标的索引，总是从磁盘中加载
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns>图标索引</returns>
        public int GetIconIndex(string fileName)
        {
            return GetIconIndex(fileName, true);
        }

        /// <summary>
        /// 获取文件中图标的索引
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="forceLoadFromDisk">指定是否总是从磁盘加载</returns>
        public int GetIconIndex(
            string fileName,
            bool forceLoadFromDisk)
        {
            return GetIconIndex(
                fileName,
                forceLoadFromDisk,
                ShellIconState.ShellIconStateNormal);
        }

        /// <summary>
        /// 获取文件中图标的索引
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="forceLoadFromDisk">指定是否总是从磁盘加载</param>
        /// <param name="iconState">指定图标的状态</param>
        /// <returns>图标的索引</returns>
        public int GetIconIndex(
            string fileName,
            bool forceLoadFromDisk,
            ShellIconState iconState
            )
        {
            ShellGetFileInfoConstants fileInfoConfig = ShellGetFileInfoConstants.SystemIconIndex;
            int attributes = 0;
            if (iconSize ==  IconSize.Minimum)
            {
                fileInfoConfig |= ShellGetFileInfoConstants.SmallIcon;
            }
            // 可以选择是否从磁盘加载，如果不从磁盘加载且图标没有可用的缓存，可能会得到错误的图标，这个参数只对文件有效
            if (!forceLoadFromDisk)
            {
                fileInfoConfig |= ShellGetFileInfoConstants.UseFileAttributes;
                attributes = (int)ShellFileAttributes.Normal;
            }
            else
            {
                attributes = 0;
            }

            // 事实上可以传入不存在的文件，也能得到相应的图标
            ShellFileInfo fileInfo = new ShellFileInfo();
            uint fileInfoSize = (uint)Marshal.SizeOf(fileInfo.GetType());
            IntPtr result = Api.ShellGetFileInfo(
                fileName, attributes, ref fileInfo, fileInfoSize,
                ((uint)(fileInfoConfig) | (uint)iconState));

            if (result.Equals(IntPtr.Zero))
            {
                Debug.Assert((!result.Equals(IntPtr.Zero)), "Failed to get icon index");
                return 0;
            }
            else
            {
                return fileInfo.IconIndex;
            }
        }
        /// <summary>
        /// 初始化图像列表
        /// </summary>
        private void initialize()
        {
            // 舍弃当前的图像列表句柄（如果有）
            imageListHandle = IntPtr.Zero;

            // 这里移除了原文件中对XP的判断，因为现在运行.NET 4.5以上的系统必定是Vista以上

            // 调用系统API获取图像列表接口
            Guid imageListGuid = new Guid("46EB5926-582E-4017-9FDF-E8998DAA0950");
            Api.ShellGetImageList(
                (int)iconSize,
                ref imageListGuid,
                ref imageListInterface
                );
            // 图像列表句柄是一个 IUnknown 指针，但是使用 Marshal.GetIUnknownForObject 不能得到正确的值。所以这里再次调用系统API来获取句柄。
            Api.ShellGetImageListHandle((int)iconSize, ref imageListGuid, ref imageListHandle);
        }
        #endregion

        #region 构造器，终结器与释放函数
        /// <summary>
        /// 用默认的图标大小创建图像列表（默认为最大）
        /// </summary>
        public ImageList()
        {
            initialize();
        }
        /// <summary>
        /// 用指定的图标大小创建图像列表
        /// </summary>
        /// <param name="size">指定的图标大小</param>
        public ImageList(IconSize size)
        {
            this.iconSize = size;
            initialize();
        }

        /// <summary>
        /// IDispose接口实现
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 清理正在使用的资源
        /// </summary>
        /// <param name="disposing">指示是否要执行清理</param>
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (imageListInterface != null)
                    {
                        Marshal.ReleaseComObject(imageListInterface);
                    }
                    imageListInterface = null;
                }
            }
            disposed = true;
        }
        /// <summary>
        /// 终结器
        /// </summary>
        ~ImageList()
        {
            Dispose(false);
        }


        #endregion
    }
}
