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
        public int code { get; set; }   
        public ModelBase Base{get;set;}
    }


    public class ModelBase
    {
        public List<OpenEventState> openeventstate { get; set; }
        public List<ButtonInfo> buttoninfo { get; set; }
        public List<ImageInfo> imageinfo { get; set; }
        public List<TextInfo> textinfo { get; set; }
        public List<ModelInfo> modelInfo { get; set; }
        public List<CameraInfo> cameraInfo { get; set;}
        public List<AudioInfo> audioinfo { get; set; }


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
            dic = new List<Dic>();
        }
        public OffsetXY pos { get; set; }
        public OffsetXY size { get; set; }
    
        public string defaulttext { get; set; }
        public List<Dic> dic { get; set; }
    }


    public class ModelInfo:BaseModel
    {

        public ModelInfo()
        {
            pos = new OffsetXYZ();
            rot = new OffsetXYZ();         
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
        }
        public OffsetXYZ pos { get; set; }
        public OffsetXYZ rot { get; set; }   
        public OffsetXYZ scale { get; set; }
    }
    public class AudioInfo
    {
        public List<Dic> clips { get; set; }
    }

}
