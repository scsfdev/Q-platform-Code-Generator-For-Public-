using GalaSoft.MvvmLight;
using QP_Code_Generator.Model;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Windows.Forms;

namespace QP_Code_Generator.ViewModel
{
    
    public class MainViewModel : ViewModelBase
    {
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { Set(ref _Title, value); }
        }

        private string _TitleVersion;
        public string TitleVersion
        {
            get { return _TitleVersion; }
            set { Set(ref _TitleVersion, value); }
        }

        private bool _generateSqrc;
        public bool GenerateSqrc
        {
            get { return _generateSqrc; }
            set { Set(ref _generateSqrc, value); }
        }


        private bool _isFrameQR;
        public bool IsFrameQR
        {
            get { return _isFrameQR; }
            set { Set(ref _isFrameQR, value); }
        }



        private string _CodeType;
        public string CodeType
        {
            get { return _CodeType; }
            set {
                Set(ref _CodeType, value);

                if (value.ToUpper().Contains("SQRC"))
                    GenerateSqrc = true;
                else
                    GenerateSqrc = false;

                if (value.ToUpper() == "FRAMEQR")
                {
                    IsFrameQR = true;
                }
                else
                    IsFrameQR = false;
            }
        }


        private string _ImgType;
        public string ImgType
        {
            get { return _ImgType; }
            set { Set(ref _ImgType, value); }
        }

        private string _shapeType;
        public string ShapeType
        {
            get { return _shapeType; }
            set { Set(ref _shapeType, value); }
        }
        


        private string _SaveTo;
        public string SaveTo
        {
            get { return _SaveTo; }
            set { Set(ref _SaveTo, value); }
        }


        private string _PublicData;
        public string PublicData
        {
            get { return _PublicData; }
            set { Set(ref _PublicData, value); }
        }

        private string _PrivateData;
        public string PrivateData
        {
            get { return _PrivateData; }
            set { Set(ref _PrivateData, value); }
        }


        public ICommand CmdBrowse { get; private set; }
        public ICommand CmdClear { get; private set; }
        public ICommand CmdClose { get; private set; }
        public ICommand CmdJob { get; private set; }
        public ICommand CmdTest { get; private set; }


        private List<string> _ListCodeType;
        public List<string> ListCodeType
        {
            get { return _ListCodeType; }
            set { Set(ref _ListCodeType, value); }
        }

        private List<string> _ListImgType;
        public List<string> ListImgType
        {
            get { return _ListImgType; }
            set { Set(ref _ListImgType, value); }
        }


        private List<string> _ListShapeType;
        public List<string> ListShapeType
        {
            get { return _ListShapeType; }
            set { Set(ref _ListShapeType, value); }
        }

        


        private BitmapImage _OutputImage;
        public BitmapImage OutputImage
        {
            get { return _OutputImage; }
            set { Set(ref _OutputImage, value); }
        }


        public MainViewModel()
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetEntryAssembly();
            var version = asm.GetName().Version.Major.ToString() + "." + asm.GetName().Version.Minor.ToString() + "." + asm.GetName().Version.Revision.ToString();

            Title = "Q platform Code Generator";
            TitleVersion = Title + "    { Ver: " + version + " }";

            CmdBrowse = new RelayCommand(Action_Browse);
            CmdClear = new RelayCommand<object>(Action_Clear);
            CmdClose = new RelayCommand<object>(Action_Close);
            CmdJob = new RelayCommand<object>(Action_Job);
            CmdTest = new RelayCommand<object>(Action_Test);

            ListCodeType = new List<string>() { "FrameQR", "FrameQR-K", "SQRC" };
            ListImgType = new List<string>() { "JPG", "PNG", "BMP" };

            ListShapeType = new List<string>() { "Rectangle", "Circle", "Pentagon", "Hexagon", "Octagon" };

            Init_Form();
        }

        private void Action_Test(object obj)
        {
            // For Testing.
        }

        private void Action_Job(object obj)
        {
            MyReply myR = new MyReply();

            if (_CodeType.ToUpper() == "FRAMEQR-K")
            {
                myR = new QpeHelper(Model.CodeType.FQRK).GenerateFrameQR(true, _PublicData, "10", _ImgType);
            }
            else if (_CodeType.ToUpper() == "FRAMEQR")
            {
                // Shape --> 0 - Rectangle, 2 - Circle, 3 - Pentagon, 4 - Hexagon, 5 - Octagon

                string shapeNo = "";
                switch (_shapeType.ToUpper())
                {
                    case "RECTANGLE": shapeNo = "0"; break;
                    case "CIRCLE": shapeNo = "2"; break;
                    case "PENTAGON": shapeNo = "3"; break;
                    case "HEXAGON": shapeNo = "4"; break;
                    case "OCTAGON": shapeNo = "5"; break;
                    default: shapeNo = "0"; break;
                }
                myR = new QpeHelper(Model.CodeType.FQR).GenerateFrameQR(false, _PublicData, "10", _ImgType, shapeNo);
            }
            else if (_CodeType.ToUpper() == "SQRC")
            {
                myR = new QpeHelper(Model.CodeType.SQRC).GenerateSQRC(_PublicData, _PrivateData, _ImgType);
            }

            

            if (myR.IsOk)
            {
                var uri = new Uri(myR.ImgPath);
                var bitmap = new BitmapImage(uri);
                OutputImage = bitmap;
            }
        }

        private void Action_Close(object obj)
        {
          //  throw new NotImplementedException();
        }

        private void Action_Clear(object obj)
        {
            Init_Form();
        }

        private void Init_Form()
        {
            CodeType = "FrameQR";
            SaveTo = Properties.Settings.Default.SaveTo;
            ImgType = "JPG";
            ShapeType = "Rectangle";
            PublicData = "";
            PrivateData = "";
            OutputImage = null;
        }

        private void Action_Browse()
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "Export the result Code to...";
            folder.ShowNewFolderButton = true;

            folder.ShowDialog();

            if (string.IsNullOrEmpty(folder.SelectedPath))
                SaveTo = "";
            else
            {
                SaveTo = folder.SelectedPath;
                Properties.Settings.Default.SaveTo = folder.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}