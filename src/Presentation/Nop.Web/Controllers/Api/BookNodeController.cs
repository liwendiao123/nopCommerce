using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nop.Services.AIBookModel;
using Nop.Services.TableOfContent;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Framework.Controllers;
using Nop.Web.Models.Api.BookNode;

namespace Nop.Web.Controllers.Api
{
    public class BookNodeController : BaseController
    {
        private readonly IAiBookService _aiBookService;
        private readonly IBookDirService _bookDirService;
        private readonly IBookNodeFactory _bookNodeFactory;
        public BookNodeController(
                    IAiBookService   aiBookService,
                    IBookDirService  bookDirService,
                    IBookNodeFactory bookNodeFactory)
        {
            _aiBookService = aiBookService;
            _bookNodeFactory = bookNodeFactory;
            _bookDirService = bookDirService;
        }
        public IActionResult Index()
        {

            var model = _bookNodeFactory.PrepareBookNodeListModel(new Areas.Admin.Models.AiBook.AiBookSearchModelView());

            return View(model);
        }
        public IActionResult GetData(int id)
        {

            var result = _aiBookService.GetAiBookModelById(id);
            if (result == null)
            {
                return Json( new List<string>()
                );
            }
            if (!string.IsNullOrEmpty(result.StrJson))
            {

                try
                {
                    var je = Newtonsoft.Json.JsonConvert.DeserializeObject(result.StrJson);
                    return Json(je);
                }
                catch (Exception ex)
                {
                    return Json( new List<string>());
                }

         

              

            }
            else
            {
                return Json(new
                {
                    //code =-1,
                    //msg = "json解析失败",
                    data = new List<string>()
                });

            }
          
        }
        public IActionResult GetJsonData(int id)
        {

           var result = _aiBookService.SearchAiBookModels(null,0,2,null,0,id).FirstOrDefault();

            if (result == null || (result.ComplexLevel == 0&& string.IsNullOrEmpty(result.UnityStrJson)))
            {
                return Json(new
                {
                    code = -1,
                    msg = "未对知识点进行泛化",
                    data = new object()
                });
            }

            if (result != null && result.BookDirID > 0)
            {
                var bookdir = _bookDirService.GetBookDirById(result.BookDirID);

                result.BookDir = bookdir;
            }

            // JsonConvert.DeserializeObject<BookNodeRoot>(result.UnityStrJson);

            return Json(new
            {
                code = 0,
                msg = "已成功",
                data = new
                { 
                     complexLevel = 0,
                    BookID = result.BookDir == null?-1: result.BookDir.BookID,
                     appointStrJson = new {

                        keyname = "ZiXingChe",
                        head = "127.0.0.1/LuaUpdata/",
                        lua = "127.0.0.1/LuaUpdata/LuaScripts/ZiXingChe.lua",
                        assetbundle = new List<string> {

                            "127.0.0.1/LuaUpdata/assetbundle/front.unity3d",
                            "127.0.0.1/LuaUpdata/assetbundle/zixingche.unity3d"
                        }
                    },
                    strJson = JsonConvert.DeserializeObject(result.UnityStrJson)
                }
            });
        }
        public BookNodeRoot Init()
        {

            var root = new BookNodeRoot();
            root.code = "1";
            // if (root.Base == null)
            //{
            root.Base = new ModelBase
            {
                openeventstate = new List<OpenEventState> {

                         new OpenEventState{
                              enventid = "0",
                              objectids = new List<string>{

                               "1001", "1002", "1003", "2000", "4001"
                              }
                         },  new OpenEventState{
                              enventid = "1",
                              objectids = new List<string>{

                                "2001"
                              }
                         }, new OpenEventState{
                              enventid = "2",
                              objectids = new List<string>{
                                "2002"
                              }
                         }, new OpenEventState{
                            enventid ="3",
                            objectids = new List<string>{

                               "2003"
                            }
                        },
                    },
                closeeventstate = new List<OpenEventState> {
                            new OpenEventState{
                              enventid = "1",
                              objectids = new List<string>{
                               "2000", "2002", "2003"
                              }
                         },  new OpenEventState{
                              enventid ="2",
                              objectids = new List<string>{
                               "2000", "2001", "2002"
                              }
                         }

                    },
                buttoninfo = new List<ButtonInfo> {
                          new ButtonInfo{
                              pos = new OffsetXY{
                                  x = "750",
                                 y = "430"
                              },
                              size = new OffsetXY{
                                  x = "160",
                                  y = "30"
                              },
                              path = "K/Button/Button",
                              url = string.Empty,
                              name = string.Empty,
                              eventid = "1",
                              id = "1001",
                              bg = string.Empty,
                              text = "海陆间循环"
                          },
                          new ButtonInfo{
                              pos = new OffsetXY{
                                  x = "750",
                                 y = "0"
                              },
                              size = new OffsetXY{
                                  x = "200",
                                  y = "100"
                              },
                              path = "K/Button/Button",
                              url = string.Empty,
                              name = string.Empty,
                              eventid = "2",
                              id = "1002",
                              bg = string.Empty,
                              text = "海上内循环"
                          },
                           new ButtonInfo{
                              pos = new OffsetXY{
                                  x = "750",
                                 y ="-150"
                              },
                              size = new OffsetXY{
                                  x = "200",
                                  y = "100"
                              },
                              path = "K/Button/Button",
                              url = string.Empty,
                              name = string.Empty,
                              eventid = "3",
                              id = "1003",
                              bg = string.Empty,
                              text = "内陆循环"
                          }

                     },
                    modelinfo = new List<ModelInfo> {
                        new ModelInfo{
                             pos = new OffsetXYZ{

                                x="0",
                                y="-2",
                                z="10"
                             },
                            scale = new OffsetXYZ{
                                x="1",
                                y="1",
                                z="1"
                            },
                            rot = new OffsetXYZ{
                                x="0",
                                y="0",
                                z="0"
                            },
                             clips = new List<Dic>{
                                  new Dic{
                                      key= "0",
                                      val= "初始动画"
                                  },
                                   new Dic {
                                      key= "1",
                                      val= "海陆间循环"
                                   },
                                    new Dic {
                                      key= "2",
                                      val= "海上内循环"
                                   },
                                     new Dic {
                                      key= "3",
                                      val= "内陆循环"
                                   },
                             },
                            path= "",
                            url= "http://arbookresouce.73data.cn/test/sxh.unity3d",
                            name= "SXH",
                            id= "6"

                        }

                    },
                    imageinfo = new List<ImageInfo> {
                        


                     },
                    textinfo = new List<TextInfo> {
                       new TextInfo{
                           pos = new OffsetXY{
                               x= "-167",
                               y="-412"
                           },
                           size = new OffsetXY{
                               x="584",
                               y ="219"
                           },
                            path= "K/Text/Text",
                            url= "",
                            name= "",
                           // defaulttext= "/K/Text/Text",
                            id= "2000",
                            dic = new List<TextDic>{
                            new TextDic{
                                     key= "0",
                                     //val= "<color=#123456>天霸动霸tua</color>"
                                     dic = new List<RichText>{
                                          new RichText{
                                               b = false,
                                               i = false,
                                               size="32",
                                               color ="000000",
                                               val = "水循环是指自然界的水在水圈、大气圈、岩石圈、生物圈四大圈层中通过各个环节连续运动的过程。自然界的水循环运动时刻都在全球范围内进行着，它发生的领域有：海洋与陆地之间，陆地与陆地上空之间，海洋与海洋上空之间。"
                                          }                                      
                                     }
                                 }                       
                          
                            }
                       },
                          new TextInfo{
                          pos = new OffsetXY{
                               x="-617",
                               y="-412"
                           },
                           size = new OffsetXY{
                               x="584",
                               y ="219"
                           },
                            path= "K/Text/Text",
                            url= "",
                            name= "",
                           // defaulttext= "/K/Text/Text",
                            id= "2001",
                            dic = new List<TextDic>{
                            new TextDic{
                                     key= "1",
                                     //val= "<color=#123456>天霸动霸tua</color>"
                                     dic = new List<RichText>{
                                          new RichText{
                                               b = false,
                                               i = false,
                                               size="32",
                                               color ="000000",
                                               val = "在太阳辐射能的作用下，从海陆表面蒸发的水分，上升到大气中；随着大气的运动和在一定的热力条件下，水汽凝结为液态水降落至地球表面；一部分降水可被植被拦截或被植物散发，降落到地面的水可以形成地表径流；渗入地下的水一部分从表层壤中流和地下径流形式进入河道，成为河川径流的一部分；贮于地下的水，一部分上升至地表供蒸发，一部分向深层渗透，在一定的条件下溢出成为不同形式的泉水；地表水和返回地面的地下水，最终都流入海洋或蒸发到大气中。"
                                          }
                                     }
                                 }

                            }
                       },

                       new TextInfo{
                        pos= new OffsetXY {
                                x= "-617",
                                y= "-412"
                            },
                            size= new OffsetXY{
                                x="-584",
                                y= "219"                               
                            },
                            path = "K/Text/Text",
                            url= "",
                            name= "",
                            //defaulttext= "我是天才",
                            id= "2002",
                            dic = new List<TextDic>{
                            new TextDic{
                                     key= "2",
                                    //val= "<color=#123456>天霸动霸tua</color>"
                                     dic = new List<RichText>{
                                          new RichText{
                                               b = false,
                                               i = false,
                                               size="32",
                                               color ="000000",
                                               val = "海上内循环是指海洋面上的水蒸发形成水汽，进入大气后在海洋上空凝结，形成降水，又降落到海面的过程。"
                                          }
                                     }
                                 }
                            }
                       }
                    },
                    camerainfo = new List<CameraInfo> {
                         new CameraInfo{
                                path= "K/Camera/DefaultCamera",
                                url="",
                                name= "",
                                id="10086",
                                 pos = new OffsetXYZ{
                                        x= "0",
                                        y= "0",
                                        z= "0"
                                 },
                                rot = new OffsetXYZ{
                                        x= "0",
                                        y= "0",
                                        z= "0"
                                },
                                scale = new OffsetXYZ{
                                       x= "0",
                                       y= "0",
                                        z= "0"
                                },
                                centerpos = new OffsetXYZ{
                                      x= "0",
                                      y= "0",
                                      z= "0"
                                },
                                rect = new Rect{
                                         x = "0",
                                         y="0",
                                         h = "0",
                                          w = "0"
                                }
                         }
                    },
                    audioinfo = new List<AudioInfo>() {
                          //new AudioInfo{
                          //     clips = new List<Dic>{
                          //          new Dic{
                          //             key="1",
                          //             val="http://gylm.73cloud.top/html/library/thirdjs/audio/fdjj.mp3"
                          //          },
                          //         new Dic{
                          //             key= "2",
                          //             val= "http://gylm.73cloud.top/html/library/thirdjs/audio/fhyf.mp3"
                          //         }
                          //     },
                          //}

                     }
                };
           // }
            return root;
            // return View();

        }


        public BookNodeNewRoot NewInit()
        {

            var newnode = new BookNodeNewRoot();

            newnode.code = "0";
            newnode.Base = new ModelNewBase
            {

                closeeventstate = new List<OpenEventState> {
                     new OpenEventState{
                              enventid = "1",
                              name = "隐藏事件1",
                              objectids = new List<string>{

                               "2000", "2001", "2003"
                              }
                         },  new OpenEventState{
                              enventid = "2",
                              name = "隐藏事件2",
                              objectids = new List<string>{

                                 "2000", "2001", "2002"
                              }
                         }

                    },
                openeventstate = new List<OpenEventState> {

                         new OpenEventState{
                              enventid = "0",
                              name = "初始",
                              objectids = new List<string>{
                               "1001", "1002", "1003", "2000"
                              }
                         },  new OpenEventState{
                              enventid = "1",
                              name = "显示事件1",
                              objectids = new List<string>{
                                     "2001"
                              }
                         }, new OpenEventState{
                              enventid = "2",
                               name = "显示事件2",
                              objectids = new List<string>{
                                "2002"
                              }
                         }, new OpenEventState{
                            enventid ="3",
                            name =  "显示事件3",
                            objectids = new List<string>{

                               "2003"
                            }
                        },
                    },

                buttoninfo = new List<ButtonInfo> {
                          new ButtonInfo{
                              pos = new OffsetXY{
                                  x = "750",
                                 y = "430"
                              },
                              size = new OffsetXY{
                                  x = "160",
                                  y = "160"
                              },
                              path = "K/Button/Button",
                              url = string.Empty,
                              name = string.Empty,
                              eventid = "1",
                              id = "1001",
                              bg = string.Empty,
                              text = "海陆间循环"
                          },
                          new ButtonInfo{
                              pos = new OffsetXY{
                                  x = "750",
                                 y = "0"
                              },
                              size = new OffsetXY{
                                  x = "200",
                                  y = "200"
                              },
                              path = "K/Button/Button",
                              url = string.Empty,
                              name = string.Empty,
                              eventid = "2",
                              id = "1002",
                              bg = string.Empty,
                              text = "海上内循环"
                          },
                           new ButtonInfo{
                              pos = new OffsetXY{
                                  x = "750",
                                 y ="-150"
                              },
                              size = new OffsetXY{
                                  x = "200",
                                  y = "200"
                              },
                              path = "K/Button/Button",
                              url = string.Empty,
                              name = string.Empty,
                              eventid = "3",
                              id = "1003",
                              bg = string.Empty,
                              text = "内陆循环"
                          }

                     },
                modelinfo = new List<ModelInfo> {
                        new ModelInfo{
                             pos = new OffsetXYZ{

                                x="0",
                                y="-2",
                                z="10"
                             },
                            scale = new OffsetXYZ{
                                x="1",
                                y="1",
                                z="1"
                            },
                            rot = new OffsetXYZ{
                                x="0",
                                y="0",
                                z="0"
                            },
                             clips = new List<Dic>{
                                  new Dic{
                                      key= "0",
                                      val= "初始动画"
                                  },
                                   new Dic {
                                      key= "1",
                                      val= "海陆间循环"
                                   },
                                    new Dic {
                                      key= "2",
                                      val= "海上内循环"
                                   },
                                     new Dic {
                                      key= "3",
                                      val= "内陆循环"
                                   },
                             },
                            path= "",
                            url= "http://arbookresouce.73data.cn/test/sxh.unity3d",
                            name= "SXH",
                            id= "6"

                        }

                    },
                imageinfo = new List<ImageNewInfo>
                {

                    new ImageNewInfo{
                       
                        defaultURL = string.Empty,
                        dic = new List<NewDic>{
                             new NewDic{
                               pos = new OffsetXY{
                                   x ="0",
                                    y="0"
                                },
                                size = new OffsetXY{
                                    x = "0",
                                    y="0"
                                },
                                 key = "0",
                                 val = string.Empty,
                             }
                        },
                         id  = "6767001",
                        url= "",
                        name= "test",
                        path= "K/Image/Image"

                    }




                },
                textinfo = new List<TextNewInfo> {
                       new TextNewInfo{
                        
                            path= "K/Text/Text",
                            url= "",
                            name= "",
                           // defaulttext= "/K/Text/Text",
                            id= "2000",
                            dic = new List<TextNewDic>{
                            new TextNewDic{
                                     key= "0",
                                    pos = new OffsetXY{
                                         x= "-167",
                                         y="-412"
                                            },
                                       size = new OffsetXY{
                                          x="584",
                                          y ="219"
                                       },
                                     //val= "<color=#123456>天霸动霸tua</color>"
                                     dic = new List<RichText>{
                                          new RichText{
                                               b = false,
                                               i = false,
                                               size="32",
                                               color ="000000",
                                               val = "水循环是指自然界的水在水圈、大气圈、岩石圈、生物圈四大圈层中通过各个环节连续运动的过程。自然界的水循环运动时刻都在全球范围内进行着，它发生的领域有：海洋与陆地之间，陆地与陆地上空之间。"
                                          }
                                     }
                                 },
                                new TextNewDic{
                                     key= "1",
                                    pos = new OffsetXY{
                                         x= "167",
                                         y="412"
                                            },
                                       size = new OffsetXY{
                                          x="684",
                                          y ="219"
                                       },
                                     //val= "<color=#123456>天霸动霸tua</color>"
                                     dic = new List<RichText>{
                                          new RichText{
                                               b = false,
                                               i = false,
                                               size="32",
                                               color ="000000",
                                               val = "海洋与海洋上空之间。"
                                          }
                                     }
                                 }

                            }
                       },
                       new TextNewInfo{
                       
                            path= "K/Text/Text",
                            url= "",
                            name= "",
                           // defaulttext= "/K/Text/Text",
                            id= "2001",
                            dic = new List<TextNewDic>{
                            new TextNewDic{
                                     key= "1",
                                     pos = new OffsetXY{
                                           x="617",
                                           y="412"
                                       },
                                     size = new OffsetXY{
                                           x="584",
                                           y ="219"
                                       },
                                     //val= "<color=#123456>天霸动霸tua</color>"
                                     dic = new List<RichText>{
                                          new RichText{
                                               b = false,
                                               i = false,
                                               size="32",
                                               color ="000000",
                                               val = "在太阳辐射能的作用下，从海陆表面蒸发的水分，上升到大气中；随着大气的运动和在一定的热力条件下，水汽凝结为液态水降落至地球表面；一部分降水可被植被拦截或被植物散发，降落到地面的水可以形成地表径流；渗入地下的水一部分从表层壤中流和地下径流形式进入河道，成为河川径流的一部分；贮于地下的水，一部分上升至地表供蒸发，一部分向深层渗透，在一定的条件下溢出成为不同形式的泉水；地表水和返回地面的地下水，最终都流入海洋或蒸发到大气中。"
                                          }
                                     }
                                 }

                            }
                       },

                       new TextNewInfo{                 
                            path = "K/Text/Text",
                            url= "",
                            name= "",
                            //defaulttext= "我是天才",
                            id= "2002",
                            dic = new List<TextNewDic>{
                            new TextNewDic{
                                     key= "2",
                                      pos= new OffsetXY {
                                        x= "-617",
                                        y= "-412"
                                    },
                                    size= new OffsetXY{
                                        x="584",
                                        y="219"
                                    },
                                    //val= "<color=#123456>天霸动霸tua</color>"
                                     dic = new List<RichText>{
                                          new RichText{
                                               b = false,
                                               i = false,
                                               size="32",
                                               color ="000000",
                                               val = "海上内循环是指海洋面上的水蒸发形成水汽，进入大气后在海洋上空凝结，形成降水，又降落到海面的过程。"
                                          }
                                     }
                                 }
                            }
                       },
                       new TextNewInfo{

                            path = "K/Text/Text",
                            url= "",
                            name= "",
                            //defaulttext= "我是天才",
                            id= "2003",
                            dic = new List<TextNewDic>{
                            new TextNewDic{
                                     key= "3",
                                      pos= new OffsetXY {
                                        x= "-617",
                                        y= "-412"
                                    },
                                    size= new OffsetXY{
                                        x="584",
                                        y= "219"
                                    },
                                    //val= "<color=#123456>天霸动霸tua</color>"
                                     dic = new List<RichText>{
                                          new RichText{
                                               b = true,
                                               i = true,
                                               size="32",
                                               color ="000000",
                                               val = "降落到大陆上的水，其中一部分通过陆面、水面蒸发和植物蒸腾形成水汽，被气流带到上空，冷却凝结形成降水，仍降落到大陆上，这就是陆地内循环。"
                                          }
                                     }
                                 }
                            }
                       }
                    },
                camerainfo = new List<CameraInfo> {
                         new CameraInfo{
                                path= "K/Camera/DefaultCamera",
                                url="",
                                name= "",
                                id="10086",
                                pos = new OffsetXYZ
                                {
                                    x= "0",
                                    y= "0",
                                    z= "0"
                                },
                                rot = new OffsetXYZ
                                {
                                    x= "0",
                                    y= "0",
                                    z= "0"
                                },
                                scale = new OffsetXYZ{
                                    x= "0",
                                    y= "0",
                                    z= "0"
                                },
                                centerpos = new OffsetXYZ{
                                    x= "0",
                                    y= "0",
                                    z= "0"
                                },
                                rect = new Rect{
                                    x=  "0",
                                    y=  "0",
                                    h=  "1",
                                    w = "1"
                                }
                         }
                    },
                audioinfo = new List<AudioInfo>()
                {
                    new AudioInfo{
                         clips = new List<Dic>{
                              new Dic{
                                 key="0",
                                 val="http://arbookresouce.73data.cn/11.ogg"
                              },
                             new Dic{
                                 key= "1",
                                 val= "http://arbookresouce.73data.cn/13.ogg"
                             }
                         },

                    id= "6001",
                    url= "",
                    name= "",
                    path= "K/Audio/CKAudio"
                    }
                },
                clickinfo = new List<ClickInfo> {
                    new ClickInfo{
                        name= "test",
                        eventid= "9001"
                    }
                },
                videoinfo = new List<VideoNewInfo> {
                    new VideoNewInfo{                
                    path= "K/Video/VideoPlayer",
                    id= "10001",
                    dic=new List<NewDic> {
                        new NewDic
                        {
                            pos=new  OffsetXY
                            {
                                x= "0",
                                y= "0"
                            },
                            size= new OffsetXY
                            {
                                x= "500",
                                y= "500"
                            },
                            key= "1",
                            val= "http://arbookresouce.73data.cn/1434.mp4"
                        },
                    }
                    }
               
                }

            };

            return newnode;

        }
    }
}