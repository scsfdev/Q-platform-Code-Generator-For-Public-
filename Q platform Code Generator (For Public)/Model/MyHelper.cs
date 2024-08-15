using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;


namespace QP_Code_Generator.Model
{
    internal enum Msg
    {
        VM,
        VIEW,
        MODEL
    }

    internal class StaticVar
    {
        internal const string ERROR = "[ ERROR ]";
        internal const string WARNING = "[ WARNING ]";
        internal const string INFO = "[ INFO ]";

        internal const string TITLE = "Q platform Code Generator Demo";

        internal const string OK = "OK";
        internal const string CANCEL = "CANCEL";
    }

    public enum CodeType
    {
        FQR,
        FQRK,
        SQRC,
        SQRCBULK,
        FQRBULK,
        DESQRC,
        DEFQR,

    }

    public class MyReplyJson
    {
        public string srvApp { get; set; }
        public string Dcd_Symbol { get; set; }
        public string Dcd_PublicData { get; set; }
        public int Dcd_PublicDataLen { get; set; }
        public string Dcd_PrivateData { get; set; }
        public int Dcd_PrivateDataLen { get; set; }
        public string Dcd_Result { get; set; }
    }


    public class MyReply : MyReplyJson
    {
        public bool IsOk { get; set; }
        public string Msg { get; set; }
        public string ErrMsg { get; set; }
        public string WarnMsg { get; set; }
        public string ImgPath { get; set; }
        public string publicData { get; set; }
        public string privateData { get; set; }
        public byte[] dataArray { get; set; }

        public MyReply()
        {
            IsOk = false;
            Msg = "";
            ErrMsg = "";
            WarnMsg = "";
            ImgPath = "";
            publicData = "";
            privateData = "";
        }
    }
    
    

    public class QpeHelper
    {
        QPlatform2Point0 qServer;

        /// <summary>
        /// Q platform Helper Class.
        /// </summary>
       
        public QpeHelper(CodeType type)
        {
            if (!Directory.Exists(Properties.Settings.Default.SaveTo))
                Directory.CreateDirectory(Properties.Settings.Default.SaveTo);

            qServer = new QPlatform2Point0(type);
        }

        public MyReply GenerateSQRC(string strPublic, string strPrivate, string fileType)
        {
            MyReply mR = qServer.GenerateSQRC(strPublic, strPrivate, fileType);

            return mR;
        }
        
        // Shape --> 0 - Rectangle, 2 - Circle, 3 - Pentagon, 4 - Hexagon, 5 - Octagon
        public MyReply GenerateFrameQR(bool? fQr, string data, string cellSize, string fileType, string fqrVer, string shapeNo = "0")
        {
            bool kangaroo = false;
            if (fQr == true)
                kangaroo = true;

            MyReply myFQR = qServer.GenerateFrameQr(kangaroo, data, cellSize, fileType, fqrVer, shapeNo);

            return myFQR;
        }

        
        

        private sealed class QPlatform2Point0
        {
            string jwtUrl = "";
            string jwtAccessKey = "";
            string serverUrl = "";
            string generateHost = "";


            public QPlatform2Point0(CodeType type)
            {
                jwtUrl = Properties.Settings.Default.APIURL;
                jwtAccessKey = Properties.Settings.Default.APIKEY;

                switch (type)
                {
                    case CodeType.FQR:
                    case CodeType.FQRK: serverUrl = Properties.Settings.Default.FQR; break;
                    case CodeType.SQRC: serverUrl = Properties.Settings.Default.SQRC; break;
                }

                generateHost = Properties.Settings.Default.HOSTURL;
            }

            private MyReply AuthenticatJWT()
            {
                MyReply myR = new MyReply();

                try
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                     | SecurityProtocolType.Tls11
                                     | SecurityProtocolType.Tls12;

                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(jwtUrl);
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = "{\"accessKey\":\"" + jwtAccessKey + "\"}";

                        streamWriter.Write(json);
                    }

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        myR.IsOk = true;
                        myR.Msg = result;
                    }
                }
                catch (Exception e)
                {
                    myR.IsOk = false;
                    myR.WarnMsg = "Q platform authentication failed!" + Environment.NewLine + "Please check your Setting config file for necessary API and URL parameters.";
                    myR.ErrMsg = "Exception error: " + e.Message;
                }

                return myR;
            }



            /// <summary>
            /// Shape --> 0 - Rectangle, 2 - Circle, 3 - Pentagon, 4 - Hexagon, 5 - Octagon
            /// </summary>
            /// <param name="fQr"></param>
            /// <param name="data"></param>
            /// <param name="cellSize"></param>
            /// <param name="fileType"></param>
            /// <param name="shapeNo"></param>
            /// <returns></returns>
            public MyReply GenerateFrameQr(bool fQr, string data, string cellSize, string fileType, string fqrVer, string shapeNo)
            {
                MyReply myFrame = new MyReply();

                try
                {
                    myFrame = AuthenticatJWT();
                    if (myFrame.IsOk)
                    {
                        string jwtAccessKey = myFrame.Msg;

                        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(serverUrl);
                        myHttpWebRequest.ContentType = "application/json";
                        myHttpWebRequest.Accept = "application/json";
                        myHttpWebRequest.Host = generateHost;
                        myHttpWebRequest.Headers.Add("Authorization", "Bearer " + jwtAccessKey);
                        myHttpWebRequest.Headers.Add("accept-encoding", "gzip,deflate");
                        myHttpWebRequest.Headers.Add("accept-language", "ja-JP");
                        myHttpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(myHttpWebRequest.GetRequestStream()))
                        {
                            string json = new JavaScriptSerializer().Serialize(new
                            {
                                optd = "FrameQR",
                                txt_Input = data,
                                dd_ver = fqrVer,      // 0 = Auto, 10 or 15,etc for large data.
                                dd_EccLvl = "1",
                                dd_canvas_mode = "0",
                                dd_CellSize = cellSize,
                                dd_CellShape = "0",
                                dd_margin = "4",
                                bgimage = "",
                                bgimg_trim_X = "0",
                                bgimg_trim_Y = "0",
                                bgimg_trim_Len = "500",
                                txt_FpCell_W = "#FFFFFF",
                                txt_FpCell_B = "#000000",
                                txt_DataCell_W = "#FFFFFF",
                                txt_DataCell_B = "#000000",
                                txt_CnvsCell = "#FFFFFF",
                                data_alpha_FPW = "255",
                                data_alpha_FPB = "255",
                                dd_alpha_DTW = "255",
                                dd_alpha_DTB = "255",
                                dd_alpha_CVS = "255",
                                dd_ShapeNo = shapeNo,  
                                dd_HeaderArea = "0",
                                dd_CenterX = "-10000",
                                dd_CenterY = "-10000",
                                dd_SizeX = "-1",
                                dd_SizeY = "-1",
                                dd_Angle = "-1",
                                opformat = fileType,
                                codeRotate = "0",
                                dd_TrimingMode = "1",
                                dd_StretchMode = "1",
                                txt_CnvsBase = "#FFFFFF",
                                dd_alpha_CVSB = "10",
                                txt_Margin = "#FFFFFF",
                                dd_alpha_Margin = "255",
                                KngrMode = fQr == true ? "1" : "0",     // FrameQR or FrameQR-K
                                num_knglsize = "10",
                                kngl_rt = "0",
                                kngl_cvs_size = "35",
                                num_kngl_mgn_in = "4",
                                num_kngl_mgn_out = "4",
                                txt_QRCell_W = "#FFFFFF",
                                txt_QRCell_B = "#000000",
                                dd_alpha_QRW = "255",
                                dd_alpha_QRB = "255",
                                txt_kngl_Url = "02",   // 00 - QRQR wifi intro, 01 - MapQR, 02 - ARAR, 03 - Toyota, 04 - NEC, 05 - QRQR, 06 - DNWA, 07 - AKiotoyoda X page, 08 - Nitento, 09 = 00
                                model = "0",
                                codeCheck = "1"
                            }
                            );

                            streamWriter.Write(json);
                        }

                        var httpResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                        myFrame.Msg = "Status Code: " + httpResponse.StatusCode;

                        var headerReply = httpResponse.Headers.Keys[2];
                        string headerMsg = httpResponse.GetResponseHeader(headerReply);
                        if(headerMsg.ToUpper() != "OK" && httpResponse.ContentLength == 0)
                        {
                            myFrame.IsOk = false;
                            myFrame.WarnMsg = "Generating FrameQR/FramQR-K failed!" + Environment.NewLine +"Try to increase Version or reduce data size.";

                            return myFrame;
                        }

                        using (var binaryReader = new BinaryReader(httpResponse.GetResponseStream()))
                        {
                            myFrame.IsOk = true;
                            myFrame.dataArray = binaryReader.ReadBytes(Convert.ToInt32(httpResponse.ContentLength));
                        }

                        // Save to Image file.
                        string fileExt = "";
                        if (httpResponse.ContentType.Contains("jpg"))
                            fileExt = ".jpg";
                        else if (httpResponse.ContentType.Contains("png"))
                            fileExt = ".png";
                        else if (httpResponse.ContentType.Contains("bmp"))
                            fileExt = ".bmp";
                        else
                            fileExt = ".jpg";

                        string fileName = DateTime.Now.ToString("yyyyMMdd_hhmmss") + fileExt;
                        string filePath = Path.Combine(Properties.Settings.Default.SaveTo, fileName);

                        SaveSqrcImage(myFrame.dataArray, filePath);
                        myFrame.ImgPath = filePath;
                        myFrame.Msg = fileName;
                    }
                }
                catch (Exception e)
                {
                    myFrame.IsOk = false;
                    myFrame.WarnMsg = "Generating FrameQR/FramQR-K failed!";
                    myFrame.ErrMsg = "Exception error: " + e.Message;
                }

                return myFrame;
            }
            
            
            public MyReply GenerateSQRC(string strPublic, string strPrivate, string fileType)
            {
                MyReply myR = new MyReply();

                try
                {
                    myR = AuthenticatJWT();
                    if (myR.IsOk)
                    {
                        string jwtAccessKey = myR.Msg;

                        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(serverUrl);
                        myHttpWebRequest.ContentType = "application/json";
                        myHttpWebRequest.Accept = "application/json";
                        myHttpWebRequest.Host = generateHost;
                        myHttpWebRequest.Headers.Add("Authorization", "Bearer " + jwtAccessKey);
                        myHttpWebRequest.Headers.Add("accept-encoding", "gzip,deflate");
                        myHttpWebRequest.Headers.Add("accept-language", "ja-JP");
                        myHttpWebRequest.Method = "POST";

                        using (var streamWriter = new StreamWriter(myHttpWebRequest.GetRequestStream()))
                        {
                            string json = new JavaScriptSerializer().Serialize(new
                            {
                                optd = "SQRC",
                                qrText = strPublic,
                                qrTextMode = "0",
                                InputMode = "0",
                                sqrcText = strPrivate,
                                sqrcTextMode = "0",
                                sqrcKey = Properties.Settings.Default.SQRCKEY,
                                qrModel = "2",
                                fpPosition = Properties.Settings.Default.FP_POSITION,      // Rotation of the Marker Square.
                                qrversion = "0",
                                EccLevel = "1",
                                cellshape = Properties.Settings.Default.CELL_SHAPE,        // 0 = dotted pattern, 1 = Normal pattern.
                                cellsize = "25",
                                margin = "4",
                                CellAdjust = "0",      
                                opformat = fileType,                               
                                codeCheck = "0"
                            });

                            streamWriter.Write(json);
                        }

                        var httpResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                        myR.Msg = "Status Code: " + httpResponse.StatusCode;

                        var headerReply = httpResponse.Headers.Keys[2];
                        string headerMsg = httpResponse.GetResponseHeader(headerReply);
                        if (headerMsg.ToUpper() != "OK" && httpResponse.ContentLength == 0)
                        {
                            myR.IsOk = false;
                            myR.WarnMsg = "Generating SQRC failed!";

                            return myR;
                        }


                        using (var binaryReader = new BinaryReader(httpResponse.GetResponseStream()))
                        {
                            myR.IsOk = true;
                            myR.dataArray = binaryReader.ReadBytes(Convert.ToInt32(httpResponse.ContentLength));
                        }

                        var hexString = string.Join("", myR.dataArray.Select(x => x.ToString("X2")));

                        string fileExt = "";
                        if (httpResponse.ContentType.Contains("jpg"))
                            fileExt = ".jpg";
                        else if (httpResponse.ContentType.Contains("png"))
                            fileExt = ".png";
                        else if (httpResponse.ContentType.Contains("bmp"))
                            fileExt = ".bmp";
                        else
                            fileExt = ".jpg";

                        // Save to Image file.
                        string fileName = DateTime.Now.ToString("yyyyMMdd_hhmmss") + fileExt;
                        string filePath = Path.Combine(Properties.Settings.Default.SaveTo, fileName);

                        SaveSqrcImage(myR.dataArray, filePath);
                        myR.ImgPath = filePath;
                        myR.Msg = fileName;
                    }
                }
                catch (Exception e)
                {
                    myR.IsOk = false;
                    myR.WarnMsg = "Generating SQRC failed!";
                    myR.ErrMsg = "Exception error: " + e.Message;
                }

                return myR;
            }
            
           
            private void SaveSqrcImage(byte[] byteIn, string filePath)
            {
                using (MemoryStream ms = new MemoryStream(byteIn, 0, byteIn.Length))
                {
                    ms.Write(byteIn, 0, byteIn.Length);
                    Image img = Image.FromStream(ms, true);
                    img.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                }
            }
        }
    }
}
