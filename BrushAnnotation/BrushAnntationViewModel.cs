using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace BrushAnnotation
{
    public class BrushAnntationViewModel : ObservableObject
    {
        #region Properties
        private Dictionary<string, string> _picturePathDic = new Dictionary<string, string>();
        public Dictionary<string, string> PicturePathDic
        {
            get { return _picturePathDic; }
            set { _picturePathDic = value; RaisePropertyChanged(nameof(PicturePathDic)); }
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
        #endregion

        #region Methods
        internal void SearchPictures(string dPath)
        {
            if (Directory.Exists(dPath))
            {
                DirectoryInfo dir = new DirectoryInfo(dPath);
                Dictionary<string, string> tempDic = new Dictionary<string, string>();
                foreach (FileInfo fi in dir.GetFiles("*.*"))
                {
                    if (fi.Name.ToLower().EndsWith(".jpg") || fi.Name.ToLower().EndsWith(".png"))
                    {
                        tempDic.Add(fi.Name, fi.FullName);
                    }
                }
                PicturePathDic = tempDic;
                if (tempDic.Count > 0)
                {
                    SelectedPicture = tempDic.FirstOrDefault().Key;
                }
            }
        }

        
        #endregion
    }
}