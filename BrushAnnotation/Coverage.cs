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
        #region Properties
        private DrawingAttributes _drawingAttributes = new DrawingAttributes();
        public DrawingAttributes DrawingAttributes
        {
            get { return _drawingAttributes; }
            set { _drawingAttributes = value; RaisePropertyChanged(nameof(DrawingAttributes)); }
        }

        private bool _isVisible = true;
        public bool IsVisible
        {
            get { return _isVisible; }
            set { _isVisible = value; RaisePropertyChanged(nameof(IsVisible)); }
        }
        #endregion
    }
}