using System;
using System.Collections.Generic;
using System.Text;

namespace Nop.Core.Domain.AIBookModel
{

    public class BookNodeRoot
    {


        public BookNodeRoot()
        {
            Base = new ModelBase();
        }
        public string code { get; set; }
        public ModelBase Base { get; set; }
    }
    public class BookNodeDomainNewRoot
    {
        public BookNodeDomainNewRoot()
        {
            code = string.Empty;
            Base = new ModelNewBase();
        }
        public string code { get; set; }
        public ModelNewBase Base { get; set; }
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
            modelinfo = new List<ModelInfo>();
            camerainfo = new List<CameraInfo>();
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
        public List<ModelInfo> modelinfo { get; set; }
        public List<CameraInfo> camerainfo { get; set; }
        public List<AudioInfo> audioinfo { get; set; }

        public List<VideoInfo> videoinfo { get; set; }

        public List<ClickInfo> clickinfo { get; set; }

    }
    public class ModelNewBase
    {
        public ModelNewBase()
        {
            closeeventstate = new List<OpenEventState>();
            openeventstate = new List<OpenEventState>();
            buttoninfo = new List<ButtonInfo>();
            imageinfo = new List<ImageNewInfo>();
            textinfo = new List<TextNewInfo>();
            modelinfo = new List<ModelInfo>();
            camerainfo = new List<CameraInfo>();
            audioinfo = new List<AudioInfo>();
            videoinfo = new List<VideoNewInfo>();
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
        public List<ImageNewInfo> imageinfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<TextNewInfo> textinfo { get; set; }
        public List<ModelInfo> modelinfo { get; set; }
        public List<CameraInfo> camerainfo { get; set; }
        public List<AudioInfo> audioinfo { get; set; }
        public List<VideoNewInfo> videoinfo { get; set; }
        public List<ClickInfo> clickinfo { get; set; }
    }
    public class BaseModel
    {
        public BaseModel()
        {
            id = string.Empty;
            url = string.Empty;
            name = string.Empty;
            path = string.Empty;

        }
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
            enventid = string.Empty;
            name = string.Empty;
        }
        public string enventid { get; set; }

        public string name { get; set; }
        public List<string> objectids { get; set; }
    }
    public class OffsetXY
    {

        public OffsetXY()
        {
            x = string.Empty;
            y = string.Empty;
            
        }
        public string x { get; set; }

        public string y { get; set; }
    }
    public class OffsetXYZ
    {

        public OffsetXYZ()
        {
            x = string.Empty;
            y = string.Empty;
            z = string.Empty;
        }

        public string x { get; set; }
        public string y { get; set; }
        public string z { get; set; }
    }
    public class ButtonInfo : BaseModel
    {

        public ButtonInfo()
        {
            pos = new OffsetXY();
            size = new OffsetXY();
            highlightedurl = string.Empty;
            pressedurl = string.Empty;
            eventid = string.Empty;
            bg = string.Empty;
            text = string.Empty;
        }

        public OffsetXY pos { get; set; }
        public OffsetXY size { get; set; }
        public string eventid { get; set; }

        public string highlightedurl { get; set; }

        public string pressedurl { get; set; }
        public string bg { get; set; }
        public string text { get; set; }
    }
    public class Dic
    {

        public Dic()
        {
            key = string.Empty;
            val = string.Empty;
        }
        public string key { get; set; }
        public string val { get; set; }
    }
    public class NewDic
    {

        public NewDic()
        {
            pos = new OffsetXY();
            size = new OffsetXY();
            key = string.Empty;
            val = string.Empty;
        }

        public OffsetXY pos { get; set; }
        public OffsetXY size { get; set; }
        public string key { get; set; }
        public string val { get; set; }
    }
    public class TextDic
    {

        public TextDic()
        {
            dic = new List<RichText>();
            key = string.Empty;
        }

        public string key { get; set; }

        public List<RichText> dic { get; set; }
    }
    public class ImageInfo : BaseModel
    {
        public ImageInfo()
        {
            pos = new OffsetXY();
            size = new OffsetXY();
            dic = new List<Dic>();
            defaultURL = string.Empty;
        }
        public OffsetXY pos { get; set; }
        public OffsetXY size { get; set; }
        public string defaultURL
        {
            get; set;
        }
        public List<Dic> dic { get; set; }
    }
    public class ImageNewInfo : BaseModel
    {
        public ImageNewInfo()
        {
            //pos = new OffsetXY();
            //size = new OffsetXY();
            dic = new List<NewDic>();
            defaultURL = string.Empty;
        }
        //public OffsetXY pos { get; set; }
        //public OffsetXY size { get; set; }
        public string defaultURL
        {
            get; set;
        }
        public List<NewDic> dic { get; set; }
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
        //  public string defaulttext { get; set; }
        public List<TextDic> dic { get; set; }
    }
    public class TextNewInfo : BaseModel
    {
        public TextNewInfo()
        {
            //pos = new OffsetXY();
            // size = new OffsetXY();
            dic = new List<TextNewDic>();
        }

        //  public string defaulttext { get; set; }
        public List<TextNewDic> dic { get; set; }
    }
    public class TextNewDic
    {


        public TextNewDic()
        {
            pos = new OffsetXY();
            size = new OffsetXY();
            dic = new List<RichText>();
            key = string.Empty;
        }


        public string key { get; set; }

        public List<RichText> dic { get; set; }
        public OffsetXY pos { get; set; }
        public OffsetXY size { get; set; }
    }
    public class ModelInfo : BaseModel
    {
        public ModelInfo()
        {
            pos = new OffsetXYZ();
            rot = new OffsetXYZ();
            scale = new OffsetXYZ();
            clips = new List<Dic>();
            type = new List<TypeEvents>();
        }
        public OffsetXYZ pos { get; set; }
        public OffsetXYZ rot { get; set; }
        public OffsetXYZ scale { get; set; }
        public List<Dic> clips { get; set; }

        public List<TypeEvents> type { get; set; }
    }

    public class TypeEvents
    {
        public TypeEvents()
        {
            key = string.Empty;
            pos = new OffsetXYZ();
            rot = new OffsetXYZ();
            scale = new OffsetXYZ();
        }
        public OffsetXYZ pos { get; set; }
        public OffsetXYZ rot { get; set; }
        public OffsetXYZ scale { get; set; }
        public string key { get; set; }
    }

    public class CameraInfo : BaseModel
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
    public class AudioInfo : BaseModel
    {

        public AudioInfo()
        {
            clips = new List<Dic>();
        }
        public List<Dic> clips { get; set; }
    }
    public class RichText
    {

        public RichText()
        {
            sort = "0";
            b = false;
            i = false;
            size = "20";
            color = "#333";
            val = string.Empty;
        }
        public bool b { get; set; }
        public bool i { get; set; }
        public string size { get; set; }
        public string color { get; set; }
        public string val { get; set; }


        public string sort { get; set; }
    }
    public class Rect
    {
        public Rect()
        {
            x = string.Empty;
            y = string.Empty;
            w = string.Empty;
            h = string.Empty;
        }
        public string x { get; set; }

        public string y { get; set; }

        public string w { get; set; }
        public string h { get; set; }

    }
    public class VideoInfo
    {

        public VideoInfo()
        {
            pos = new OffsetXY();
            size = new OffsetXY();
            dic = new List<Dic>();
            path = string.Empty;
            id = string.Empty;
        }

        public OffsetXY pos { get; set; }
        public OffsetXY size { get; set; }

        public string path { get; set; }

        public string id { get; set; }

        public List<Dic> dic { get; set; }

    }
    public class VideoNewInfo
    {

        public VideoNewInfo()
        {

            dic = new List<NewDic>();
            path = string.Empty;
            id = string.Empty;
        }



        public string path { get; set; }

        public string id { get; set; }

        public List<NewDic> dic { get; set; }

    }
    public class ClickInfo
    {
        public ClickInfo()
        {
            name = string.Empty;
            eventid = string.Empty;
        }
        public string name { get; set; }

        public string eventid { get; set; }
    }
}
