using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Media;

namespace BrushAnnotation
{
    public class BrushAnntationViewModel : ObservableObject
    {
        #region Fields
        private int lowZIndex = 2, highZIndex = 3;
        private static readonly string buttonName1 = "擦除";
        private static readonly string buttonName2 = "标注";

        private Color currentBurshColor = Colors.Green;
        private Style inkcanvasStyle;
        private Grid CoverageContainer;
        
        #endregion

        #region Properties
        private Dictionary<string, string> _picturePathDic = new Dictionary<string, string>();
        public Dictionary<string, string> PicturePathDic
        {
            get { return _picturePathDic; }
            set { _picturePathDic = value; RaisePropertyChanged(nameof(PicturePathDic)); }
        }

        private Dictionary<string, List<Coverage>> _picsCoverages = new Dictionary<string, List<Coverage>>();
        public Dictionary<string, List<Coverage>> PicsCoverages
        {
            get { return _picsCoverages; }
            set { _picsCoverages = value; RaisePropertyChanged(nameof(PicsCoverages)); }
        }

        private ObservableCollection<Coverage> _coverageCollection = new ObservableCollection<Coverage>();
        public ObservableCollection<Coverage> CoverageCollection
        {
            get { return _coverageCollection; }
            set { _coverageCollection = value; RaisePropertyChanged(nameof(CoverageCollection)); }
        }

        private string _selectedPicture = string.Empty;
        public string SelectedPicture
        {
            get { return _selectedPicture; }
            set { _selectedPicture = value; RaisePropertyChanged(nameof(SelectedPicture)); }
        }

        private string _selectedPicturePath = string.Empty;
        public string SelectedPicturePath
        {
            get { return _selectedPicturePath; }
            set { _selectedPicturePath = value; RaisePropertyChanged(nameof(SelectedPicturePath)); }
        }

        private Coverage _selectedCoverage;
        public Coverage SelectedCoverage
        {
            get { return _selectedCoverage; }
            set
            {
                _selectedCoverage = value;
                RaisePropertyChanged(nameof(SelectedCoverage));
                SelectedCoverageChanged();
            }
        }


        private InkCanvas _currentInkCanvas;
        public InkCanvas CurrentInkCanvas
        {
            get { return _currentInkCanvas; }
            set
            {
                _currentInkCanvas = value;
                CurrentInkCanvasChanged();
            }
        }

        private string _buttonName = buttonName1;
        public string ButtonName
        {
            get { return _buttonName; }
            set
            {
                _buttonName = value;
                RaisePropertyChanged(nameof(ButtonName));
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
                SetCurrentCoverageBrushSize();
            }
        }

        #endregion

        #region Command
        private RelayCommand _addNewCommand;
        public RelayCommand AddNewCommand
        {
            get
            {
                if (_addNewCommand == null)
                {
                    _addNewCommand = new RelayCommand(AddNew);
                }
                return _addNewCommand; }
        }

        private RelayCommand _deleteCommand;
        public RelayCommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand(Delete);
                }
                return _deleteCommand;
            }
        }


        private RelayCommand _wipeStrokesCommand;
        public RelayCommand WipeStrokesCommand
        {
            get
            {
                if (_wipeStrokesCommand == null)
                {
                    _wipeStrokesCommand = new RelayCommand(WipeStrokes);
                }
                return _wipeStrokesCommand;
            }
        }

        #endregion

        #region Methods
        internal void IniContainer(Grid container, object style)
        {
            this.CoverageContainer = container;
            this.inkcanvasStyle = style as Style;

        }

        internal void SearchPictures(string dPath)
        {
            CollectionClear();
            if (Directory.Exists(dPath))
            {
                DirectoryInfo dir = new DirectoryInfo(dPath);
                Dictionary<string, string> tempDic = new Dictionary<string, string>();
                foreach (FileInfo fi in dir.GetFiles("*.*"))
                {
                    if (fi.Name.ToLower().EndsWith(".jpg") || fi.Name.ToLower().EndsWith(".png"))
                    {
                        tempDic.Add(fi.Name, fi.FullName);
                        PicsCoverages.Add(fi.Name, new List<Coverage>());
                    }
                }
                PicturePathDic = tempDic;
                if (tempDic.Count > 0)
                {
                    SelectedPicture = tempDic.FirstOrDefault().Key;
                    SelectedPicturePath = tempDic.FirstOrDefault().Value;
                    CoverageCollection = new ObservableCollection<Coverage>(PicsCoverages[SelectedPicture]);
                    if (CoverageCollection.Count < 1)
                    {
                        AddNew();
                    }
                }
            }
        }

        private void CollectionClear()
        {
            PicturePathDic.Clear();
            PicsCoverages.Clear();
            CoverageCollection.Clear();
            foreach (InkCanvas ink in CoverageContainer.Children.OfType<InkCanvas>())
            {
                CoverageContainer.Children.Remove(ink);
            }
            CurrentInkCanvas = null;
            SelectedCoverage = null;
            SelectedPicturePath = string.Empty;
            SelectedPicture = string.Empty;
        }

        private void AddNew()
        {
            if (CoverageCollection != null)
            {
                foreach (Coverage c in CoverageCollection)
                {
                    c.ZIndex = highZIndex;
                }
                foreach (var ink in CoverageContainer.Children.OfType<InkCanvas>())
                {
                    Panel.SetZIndex(ink, (ink.DataContext as Coverage).ZIndex);
                }
                Coverage tempC = new Coverage();
                tempC.Index = CoverageCollection.Count + 1;
                tempC.DrawAttributes.Color = currentBurshColor;
                tempC.BrushSize = BrushSize;
                CoverageCollection.Add(tempC);

                InkCanvas newInk = new InkCanvas();
                newInk.DataContext = tempC;
                newInk.Style = inkcanvasStyle;
                CoverageContainer.Children.Add(newInk);
                newInk.ApplyTemplate();

                SelectedCoverage = tempC;
                CurrentInkCanvas = newInk;
            }
        }

        private void WipeStrokes()
        {
            if (CurrentInkCanvas == null)
            {
                return;
            }

            if (string.Equals(ButtonName, buttonName1))
            {
                CurrentInkCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
                ButtonName = buttonName2;
            }
            else
            {
                CurrentInkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                ButtonName = buttonName1;
            }
        }

        private void Delete()
        {
            foreach (InkCanvas c in CoverageContainer.Children.OfType<InkCanvas>())
            {
                if ((c.DataContext as Coverage) == SelectedCoverage)
                {
                    CoverageCollection.Remove(SelectedCoverage);
                    CoverageContainer.Children.Remove(c);
                    break;
                }
            }
            CurrentInkCanvas = CoverageContainer.Children.OfType<InkCanvas>().FirstOrDefault();
            SelectedCoverage = CoverageContainer.Children.OfType<InkCanvas>().First().DataContext as Coverage;
        }

        private void SelectedCoverageChanged()
        {
            if (CurrentInkCanvas == null || SelectedCoverage == null)
            {
                return;
            }

            Panel.SetZIndex(CurrentInkCanvas, lowZIndex);
            foreach (Coverage c in CoverageCollection)
            {
                if (c == SelectedCoverage)
                {
                    if (c.IsVisible == false)
                    {
                        c.IsVisible = true;
                    }
                    c.IsCheck = true;
                    c.ZIndex = highZIndex;
                    CurrentInkCanvas = CoverageContainer.Children.OfType<InkCanvas>().Where(ink => (ink.DataContext as Coverage) == c).FirstOrDefault();
                    Panel.SetZIndex(CurrentInkCanvas, c.ZIndex);
                    continue;
                }
                c.IsCheck = false;
                c.ZIndex = lowZIndex;
            }
        }

        private void CurrentInkCanvasChanged()
        {
            if (CurrentInkCanvas == null)
            {
                return;
            }
            Panel.SetZIndex(CurrentInkCanvas, SelectedCoverage.ZIndex);
            if (SelectedCoverage.IsVisible == false)
            {
                SelectedCoverage.IsVisible = true;
            }
        }

        private void SetCurrentCoverageBrushSize()
        {
            if (SelectedCoverage != null)
            {
                if (CurrentInkCanvas.EditingMode == InkCanvasEditingMode.EraseByPoint)
                {
                    CurrentInkCanvas.EraserShape = new EllipseStylusShape(this.BrushSize, this.BrushSize);
                }
                SelectedCoverage.BrushSize = this.BrushSize;
            }
        }
        #endregion
    }
}