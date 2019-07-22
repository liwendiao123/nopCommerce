using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


/// <summary>
/// 该模型主要用于对应 json字段 故属性对应为 小写 
/// 请勿随意更改 大小写 
/// author lwd
/// </summary>

namespace Nop.Web.Models.Api.BookNode
{
    public class BookNodeRoot
    {


        public BookNodeRoot()
        {
            Base = new ModelBase();
        }
        public int code { get; set; }   
        public ModelBase Base{get;set;}
    }


    public class ModelBase
    {


        public ModelBase()
        {
            closeeventstate = new List<OpenEventState>();
            openeventstate = new List<OpenEventState>();
            buttoninfo = new List<ButtonInfo>();
            imageinfo = new List<ImageInfo>();
            textinfo = new List<TextInfo>();
            modelInfo = new List<ModelInfo>();
            cameraInfo = new List<CameraInfo>();
            audioinfo = new List<AudioInfo>();
            videoinfo = new List<VideoInfo>();
            clickinfo = new List<ClickInfo>();


        }

        /// <summary>
        ///  关闭事件
        /// </summary>
        public List<OpenEventState> closeeventstate { get; set; }

        /// <summary>
        /// 打开事件
        /// </summary>
        public List<OpenEventState> openeventstate { get; set; }

        /// <summary>
        /// 按钮信息
        /// </summary>
        public List<ButtonInfo> buttoninfo { get; set; }

        /// <summary>
        /// 图片信息
        /// </summary>
        public List<ImageInfo> imageinfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<TextInfo> textinfo { get; set; }
        public List<ModelInfo> modelInfo { get; set; }
        public List<CameraInfo> cameraInfo { get; set;}
        public List<AudioInfo> audioinfo { get; set; }

        public List<VideoInfo> videoinfo { get; set; }

        public List<ClickInfo> clickinfo { get; set; }

    }


    public class BaseModel
    {
        public string id { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public string path { get; set; }
    }


    public class OpenEventState
    {

        public OpenEventState()
        {
            objectids = new List<string>();
        }
        public int enventid { get; set; }
        public List<string> objectids { get; set; }
    }


    public class OffsetXY
    {
        public int x { get; set; }

        public int y { get; set; }
    }


    public class OffsetXYZ
    {
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
    }
    public class ButtonInfo:BaseModel
    {

        public ButtonInfo()
        {
            pos = new OffsetXY();
            size = new OffsetXY();
        }

        public OffsetXY pos { get; set; }
        public OffsetXY size { get; set; }
        public string eventid { get; set; }
        public string bg { get; set; }
        public string text { get; set; }
    }

    public class Dic {
        public string key { get; set; }
        public string val { get; set; }
    }


    public class TextDic:BaseModel
    {

        public TextDic()
        {
            dic = new List<RichText>();
        }

        public string key { get; set; }

        public List<RichText> dic { get; set; }
    }
    public class ImageInfo:BaseModel
    {
        public ImageInfo()
        {
            pos = new OffsetXY();
           size = new OffsetXY();
            dic = new List<Dic>();
        }
        public OffsetXY pos { get; set; }
        public OffsetXY size { get; set; }
        public string defaultURL { get; set; }  
        public List<Dic> dic { get; set; }
    }
    public class TextInfo : BaseModel
    {
        public TextInfo()
        {
            pos = new OffsetXY();
            size = new OffsetXY();
            dic = new List<TextDic>();
        }
        public OffsetXY pos { get; set; }
        public OffsetXY size { get; set; }
        public string defaulttext { get; set; }
        public List<TextDic> dic { get; set; }
    }
    public class ModelInfo:BaseModel
    {
        public ModelInfo()
        {
            pos = new OffsetXYZ();
            rot = new OffsetXYZ();
            scale = new OffsetXYZ();
            clips = new List<Dic>();
        }
        public OffsetXYZ pos { get; set; }
        public OffsetXYZ rot { get; set; }
        public OffsetXYZ scale { get; set; }
        public List<Dic> clips { get; set; }
    }


    public class CameraInfo:BaseModel
    {
        public CameraInfo()
        {
            pos = new OffsetXYZ();
            rot = new OffsetXYZ();
            scale = new OffsetXYZ();
            centerpos = new OffsetXYZ();
            rect = new Rect();
        }
        public OffsetXYZ pos { get; set; }
        public OffsetXYZ rot { get; set; }   
        public OffsetXYZ scale { get; set; }
        public OffsetXYZ centerpos { get; set; }
        public Rect rect { get; set; }
    }
    public class AudioInfo:BaseModel
    {

        public AudioInfo()
        {
            clips = new List<Dic>();
        }
        public List<Dic> clips { get; set; }
    }
    public class RichText {
        public bool b { get; set; }
        public bool i { get; set; }
        public int size { get; set; }
        public string color { get; set; }
        public string val { get; set;}

       
        public int sort { get; set; }
    }


    public class Rect
    {
        public int x { get; set; }

        public int y { get; set; }

        public int w { get; set; }
        public int h { get; set; }

    }


    public class VideoInfo
    {

        public VideoInfo()
        {
            pos = new OffsetXYZ();
            size = new OffsetXY();
            dic = new List<Dic>();
        }

        public OffsetXYZ pos { get; set; }
        public OffsetXY size { get; set; }   

        public string path { get; set; }

        public string id { get; set; }

        public List<Dic> dic { get; set; }

    }

    public class ClickInfo {
        public string name { get; set; }

        public string id { get; set; }
    }


   
}
