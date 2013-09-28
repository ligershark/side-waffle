using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Editor.DragDrop;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.IO;
using System.Windows;

namespace $rootnamespace$
{
    [Export(typeof(IDropHandlerProvider))]
    [DropFormat("CF_VSSTGPROJECTITEMS")]
    [DropFormat("FileDrop")]
    [Name("$safeitemname$")]
    [ContentType("XML")]
    [Order(Before = "DefaultFileDropHandler")]
    internal class $safeitemname$Provider : IDropHandlerProvider
    {
        public IDropHandler GetAssociatedDropHandler(IWpfTextView view)
        {
            return view.Properties.GetOrCreateSingletonProperty<$safeitemname$>(() => new $safeitemname$(view));
        }
    }

    internal class $safeitemname$ : IDropHandler
    {
        private IWpfTextView _view;
        private string _draggedFilename;
        private string _format = Environment.NewLine + "<file>{0}</file>";

        public $safeitemname$(IWpfTextView view)
        {
            this._view = view;
        }

        public DragDropPointerEffects HandleDataDropped(DragDropInfo dragDropInfo)
        {
            int position = dragDropInfo.VirtualBufferPosition.Position.Position;
            string text = string.Format(_format, _draggedFilename);
            _view.TextBuffer.Insert(position, text);

            return DragDropPointerEffects.Copy;
        }

        public void HandleDragCanceled()
        { }

        public DragDropPointerEffects HandleDragStarted(DragDropInfo dragDropInfo)
        {
            return DragDropPointerEffects.All;
        }

        public DragDropPointerEffects HandleDraggingOver(DragDropInfo dragDropInfo)
        {
            return DragDropPointerEffects.All;
        }

        public bool IsDropEnabled(DragDropInfo dragDropInfo)
        {
            _draggedFilename = GetImageFilename(dragDropInfo);

            return File.Exists(_draggedFilename);
        }

        private static string GetImageFilename(DragDropInfo info)
        {
            DataObject data = new DataObject(info.Data);

            if (info.Data.GetDataPresent("FileDrop"))
            {
                // The drag and drop operation came from the file system
                StringCollection files = data.GetFileDropList();

                if (files != null && files.Count == 1)
                {
                    return files[0];
                }
            }
            else if (info.Data.GetDataPresent("CF_VSSTGPROJECTITEMS"))
            {
                // The drag and drop operation came from the VS solution explorer
                return data.GetText();
            }

            return null;
        }
    }
}