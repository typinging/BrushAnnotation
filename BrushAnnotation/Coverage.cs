using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Ink;

namespace BrushAnnotation
{
    public class Coverage : ObservableObject
    {
        private static readonly string coverageName = "图层";


        #region Properties
        private DrawingAttributes _drawingAttributes = new DrawingAttributes();
        public DrawingAttributes DrawAttributes
        {
            get { return _drawingAttributes; }
            set { _drawingAttributes = value; RaisePropertyChanged(nameof(DrawAttributes)); }
        }

        private bool _isVisible = true;
        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; RaisePropertyChanged(nameof(IsVisible)); }
        }

        private int _zIndex = 2;
        public int ZIndex
        {
            get { return _zIndex; }
            set { _zIndex = value; RaisePropertyChanged(nameof(ZIndex)); }
        }

        private bool _isCheck = true;
        public bool IsCheck
        {
            get { return _isCheck; }
            set
            {
                _isCheck = value;
                RaisePropertyChanged(nameof(IsCheck));
            }
        }

        private string _name = string.Empty;
        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(nameof(Name)); }
        }

        private int _index;
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                RaisePropertyChanged(nameof(Index));
                SetName();
            }
        }

        private int _brushSize = 5;
        public int BrushSize
        {
            get { return _brushSize; }
            set
            {
                _brushSize = value;
                RaisePropertyChanged(nameof(BrushSize));
                SetBrushSize();
            }
        }

        #endregion

        private void SetBrushSize()
        {
            this.DrawAttributes.Height = (double)BrushSize;
            this.DrawAttributes.Width = (double)BrushSize;
        }

        private void SetName()
        {
            this.Name = coverageName + this.Index.ToString();
        }

    }
}