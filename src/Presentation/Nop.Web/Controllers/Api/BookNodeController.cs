using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.AIBookModel;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Framework.Controllers;
using Nop.Web.Models.Api.BookNode;

namespace Nop.Web.Controllers.Api
{
    public class BookNodeController : BaseController
    {
        private readonly IAiBookService _aiBookService;
        private readonly IBookNodeFactory _bookNodeFactory;
        public BookNodeController(IAiBookService aiBookService, IBookNodeFactory bookNodeFactory)
        {
            _aiBookService = aiBookService;
            _bookNodeFactory = bookNodeFactory;
        }

        public IActionResult Index()
        {

            var model = _bookNodeFactory.PrepareBookNodeListModel(new Areas.Admin.Models.AiBook.AiBookSearchModelView());

            return View(model);
        }

        public IActionResult GetData(int id)
        {

            var result = _aiBookService.GetAiBookModelById(id);
            return Json(new
            {
                code = 0,
                msg = "已成功",
                data = new
                {                 
                    strJson = result.StrJson
                }
            });
        }


        public IActionResult GetJsonData(int id)
        {

        //    var result = _aiBookService.GetAiBookModelById(id);
            return Json(new
            {
                code = 0,
                msg = "已成功",
                data = new
                {
                    strJson = Init()
                }
            });
        }

        public BookNodeRoot Init()
        {

            var root = new BookNodeRoot();
            root.code = 1;
            if (root.Base == null)
            {
                root.Base = new ModelBase
                {
                    openeventstate = new List<OpenEventState> {

                         new OpenEventState{
                              enventid = 0,
                              objectids = new List<string>{

                                  "1","4","5","6"
                              }
                         },  new OpenEventState{
                              enventid = 1,
                              objectids = new List<string>{

                                  "2", "3", "4"
                              }
                         }, new OpenEventState{
                              enventid = 2,
                              objectids = new List<string>{
                                  "1"
                              }
                         }, new OpenEventState{
                            enventid =3,
                            objectids = new List<string>{

                                "4"
                            }
                        },
                    },
                    closeeventstate = new List<OpenEventState> {

                            new OpenEventState{
                              enventid = 0,
                              objectids = new List<string>{
                                 "2","3"
                              }
                         },  new OpenEventState{
                              enventid = 1,
                              objectids = new List<string>{
                                "1"
                              }
                         }, new OpenEventState{
                              enventid = 2,
                              objectids = new List<string>{
                                    "2", "4"
                              }
                         }

                    },
                    modelInfo = new List<ModelInfo> {
                        new ModelInfo{
                             pos = new OffsetXYZ{

                                x=0,
                                y=-2,
                                z=10
                             },
                            scale = new OffsetXYZ{
                                x=1,
                                y=1,
                                z=1
                            },
                            rot = new OffsetXYZ{
                                 x=0,
                                y=0,
                                z=0
                            },
                             clips = new List<Dic>{
                                  new Dic{
                                      key= "1",
                                      val= "1"
                                  },
                                   new Dic {
                                      key= "2",
                                      val= "2"
                                   }
                             },
                            path= "K/Image",
                            url= "/",
                            name= "dixing",
                            id= "6"

                        }

                    },
                    imageinfo = new List<ImageInfo> {
                         new ImageInfo{
                              pos = new OffsetXY{
                                  x= -800,
                                  y= 250
                              },
                              size =  new OffsetXY{
                                  x= 250,
                                  y= 300
                              },
                            defaultURL= "K/sy_img_04",
                            path= "K/Image",
                            url= "",
                            name= "",
                            id= "4",
                            dic = new List<Dic>{

                                 new Dic{
                                     key= "1",
                                     val= "K/sy_img_03"
                                 },
                                 new Dic{
                                     key= "2",
                                     val= "K/sy_img_02"
                                 }
                            }


                         }


                     },
                    textinfo = new List<TextInfo> {
                       new TextInfo{
                            path= "K/Image",
                            url= "",
                            name= "",
                            defaulttext= "我是天才",
                            id= "5",
                            dic = new List<TextDic>{

                            new TextDic{
                                     key= "0",
                                     //val= "<color=#123456>天霸动霸tua</color>"
                                     dic = new List<RichText>{
                                          new RichText{
                                               b = false,
                                               i = false,
                                               size=11,
                                               color ="000000",
                                               val = "我"
                                          },
                                          new RichText{
                                               b = true,
                                               i = false,
                                               size=16,
                                               color ="ffffff",
                                               val = "是"
                                          },
                                          new RichText{
                                               b = true,
                                               i = true,
                                               size=32,
                                               color ="cacaca",
                                               val = "天\n才"
                                          }


                                     }

                                 },

                                 new TextDic{
                                     key= "1",
                                     //val= "<color=#123456>天霸动霸tua</color>"
                                     dic = new List<RichText>{
                                          new RichText{
                                                b= true,
                                                i= false,
                                                size= 11,
                                                color= "000000",
                                                val= "天霸"
                                          },
                                          new RichText{
                                                b= false,
                                                i= false,
                                                size= 16,
                                                color= "ffffff",
                                                val= "动//n霸"
                                          }


                                     }
                                      
                                 },
                                 new TextDic{
                                     key= "2",
                                     dic= new List<RichText>{// "<color=#ffffff>天上地下唯我独尊</color>"
                                         new RichText{
                                                b= false,
                                                i= false,
                                                size= 32,
                                                color= "cacaca",
                                                val= "tua"
                                         },
                                           new RichText{
                                                b= false,
                                                i= false,
                                                size= 16,
                                                color= "ffffff",
                                                val= "要"
                                         },
                                         new RichText{
                                                b= true,
                                                i= true,
                                                size= 32,
                                                color= "cacaca",
                                                val= "转正"
                                         }
                                     }
                                 }
                            }
                       },

                    },
                    cameraInfo = new List<CameraInfo> {


                         new CameraInfo{

                                path= "k/Camera",
                                url="",
                                name= "",
                                id="10086",
                                 pos = new OffsetXYZ{
                                        x= 0,
                                        y= 0,
                                        z= 0
                                 },
                                rot = new OffsetXYZ{
                                        x= 0,
                                        y= 0,
                                        z= 0
                                },
                                scale = new OffsetXYZ{
                                       x= 0,
                                       y= 0,
                                        z= 0
                                }
                         }


                    },

                    audioinfo = new List<AudioInfo>() {
                          new AudioInfo{
                               clips = new List<Dic>{
                                    new Dic{
                                       key="1",
                                       val="http://gylm.73cloud.top/html/library/thirdjs/audio/fdjj.mp3"
                                    },
                                   new Dic{
                                       key= "2",
                                       val= "http://gylm.73cloud.top/html/library/thirdjs/audio/fhyf.mp3"
                                   }
                               },


                          }

                     }

                };
            }


            return root;
            // return View();

        }




    }
}