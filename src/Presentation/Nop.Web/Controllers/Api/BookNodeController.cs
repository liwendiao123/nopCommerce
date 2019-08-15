using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Nop.Core.Configuration;

using Nop.Core.Domain.TableOfContent;
using Nop.Services.AIBookModel;
using Nop.Services.Catalog;
using Nop.Services.Media;
using Nop.Services.TableOfContent;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Framework.Controllers;
using Nop.Web.Models.Api.BookNode;

namespace Nop.Web.Controllers.Api
{
    public class BookNodeController : BasePublicController
    {
        private readonly IAiBookService _aiBookService;
        private readonly IBookDirService _bookDirService;
        private readonly IBookNodeFactory _bookNodeFactory;
        private readonly IBookNodeTagService _bookNodeTagService;
        private readonly IProductService _productService;
        private readonly IPictureService _pictureService;
        private readonly NopConfig _config;
        public BookNodeController(
                    IAiBookService   aiBookService,
                    IBookDirService  bookDirService,
                    IProductService productService,
                    NopConfig config,
                    IBookNodeTagService bookNodeTagService,
                    IPictureService pictureService,
        IBookNodeFactory bookNodeFactory)
        {
            _aiBookService = aiBookService;
            _bookNodeFactory = bookNodeFactory;
            _bookDirService = bookDirService;
            _productService = productService;
            _bookNodeTagService = bookNodeTagService;
            _config = config;
            _pictureService = pictureService;
        }
        public IActionResult Index()
        {
            var searchmodel = new Areas.Admin.Models.AiBook.AiBookSearchModelView();
            searchmodel.SetGridPageSize(int.MaxValue);
            var model = _bookNodeFactory.PrepareBookNodeListModel(searchmodel);
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
        public IActionResult GetJsonData(int id,string platformtype)
        {
            string platformtypepath = "windows";
            switch (platformtype)
            {
                case "1":
                    platformtypepath = "ios";
                    break;
                case "2":
                    platformtypepath = "android";
                    break;
                case "3":
                    platformtypepath = "windows";
                    break;
            }


           var result = _aiBookService.SearchAiBookModels("",0,int.MaxValue,null,0,id).FirstOrDefault();
            if (result == null)
            {
                return Json(new
                {
                    code = -1,
                    msg = "未找到知识点",
                    data = new object()
                });
            }
            if (result != null && !string.IsNullOrEmpty(result.UniqueID))
            {
                result.ComplexLevel = 1;
            }
            if (result == null || (result.ComplexLevel == 0&& string.IsNullOrEmpty(result.UnityStrJson)))
            {
                return Json(new
                {
                    code = -1,
                    msg = "未对知识点进行泛化",
                    data = new object()
                });
            }
            BookDir bookdir = null;
            if (result != null && result.BookDirID > 0)
            {
                 bookdir = _bookDirService.GetBookDirById(result.BookDirID);          
            }
            var ttbookdir = bookdir == null ? -1 : bookdir.BookID;
            var bookid = ttbookdir;
            var product = _productService.GetProductById(bookid);
            BookNodeNewRoot jsonresult = new BookNodeNewRoot();
            try
            {
                jsonresult = JsonConvert.DeserializeObject<BookNodeNewRoot>(result.UnityStrJson);
                jsonresult.Base.buttoninfo = jsonresult.Base.buttoninfo.Where(x => !string.IsNullOrEmpty(x.id)).ToList();
                jsonresult.Base.textinfo = jsonresult.Base.textinfo.Where(x => !string.IsNullOrEmpty(x.id)).ToList();
                jsonresult.Base.imageinfo = jsonresult.Base.imageinfo.Where(x => !string.IsNullOrEmpty(x.id)).ToList();
                jsonresult.Base.videoinfo = jsonresult.Base.videoinfo.Where(x => !string.IsNullOrEmpty(x.id)).ToList();
                jsonresult.Base.audioinfo = jsonresult.Base.audioinfo.Where(x => !string.IsNullOrEmpty(x.id)).ToList();
                jsonresult.Base.camerainfo = jsonresult.Base.camerainfo.Where(x => !string.IsNullOrEmpty(x.id)).ToList();
                jsonresult.Base.clickinfo = jsonresult.Base.clickinfo.Where(x => !string.IsNullOrEmpty(x.eventid)).ToList();
                jsonresult.Base.modelinfo = jsonresult.Base.modelinfo.Where(x => !string.IsNullOrEmpty(x.id)).ToList();
                jsonresult.Base.openeventstate = jsonresult.Base.openeventstate.Where(x => !string.IsNullOrEmpty(x.enventid)).ToList();
                jsonresult.Base.closeeventstate = jsonresult.Base.closeeventstate.Where(x => !string.IsNullOrEmpty(x.enventid)).ToList();
            }
            catch (Exception ex)
            {
                jsonresult = null;
            }
            //var ttbookdir = bookdir == null ? -1 : bookdir.BookID;
            //var bookid = ttbookdir;
            //var product = _productService.GetProductById(bookid);
            result.WebModelUrl = (result.WebModelUrl ?? "").Replace("，", ",");
            var arr = result.WebModelUrl.Split(",").ToList().Where(x=>!string.IsNullOrEmpty(x)).ToList();
            List<string> allList = new List<string>();
            arr.ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x))
                {
                    x =( _config.HostLuaResource ?? "") + platformtypepath + "/"  + x + "?v=" + DateTime.Now.Ticks;

                    allList.Add(x);
                }
            });
            var luaurl = _config.HostLuaResource ?? "" + platformtypepath + "/"  + result.WebGltfUrl ?? "";
            return Json(new
            {
                code = 0,
                msg = "已成功",
                data = new
                {
                    complexLevel = string.IsNullOrEmpty(result.UniqueID) ? 0 : 1,
                    BookID = bookdir == null ? -1 : bookdir.BookID,
                    BookNodeName = result.Name,
                    BookName = product == null?"":product.Name,
                    IsLock = false,
                    appointStrJson = new {
                        keyname = result.UniqueID??"",
                        head = _config.HostLuaResource ?? "",
                        lua = (_config.HostLuaResource ?? "") +platformtypepath +"/" + (result.WebGltfUrl ?? "") +"?v="+ DateTime.Now.Ticks,
                        assetbundle = allList
                    },
                    strJson = jsonresult==null?new object(): jsonresult
                }
            });
        }
        public IActionResult GetKnowledgeById(int id)
        {
            var result = _aiBookService.GetAiBookModelById(id);
            if (result!= null && !string.IsNullOrEmpty(result.UniqueID))
            {
                result.ComplexLevel = 1;
            }
            if (result == null || (result.ComplexLevel == 0 && string.IsNullOrEmpty(result.UnityStrJson)))
            {
                return Json(new
                {
                    code = -1,
                    msg = "未对知识点进行泛化",
                    data = new object()
                });
            }
            BookDir bookdir = null;
            if (result != null && result.BookDirID > 0)
            {
                bookdir = _bookDirService.GetBookDirById(result.BookDirID);  
            }
            BookNodeNewRoot jsonresult = new BookNodeNewRoot();
            try
            {

                jsonresult = JsonConvert.DeserializeObject<BookNodeNewRoot>(result.UnityStrJson);
                jsonresult.Base.buttoninfo = jsonresult.Base.buttoninfo.Where(x => !string.IsNullOrEmpty(x.id)).ToList();
                jsonresult.Base.textinfo = jsonresult.Base.textinfo.Where(x => !string.IsNullOrEmpty(x.id)).ToList();
                jsonresult.Base.imageinfo = jsonresult.Base.imageinfo.Where(x => !string.IsNullOrEmpty(x.id)).ToList();
                jsonresult.Base.videoinfo = jsonresult.Base.videoinfo.Where(x => !string.IsNullOrEmpty(x.id)).ToList();
                jsonresult.Base.audioinfo = jsonresult.Base.audioinfo.Where(x => !string.IsNullOrEmpty(x.id)).ToList();
                jsonresult.Base.camerainfo = jsonresult.Base.camerainfo.Where(x => !string.IsNullOrEmpty(x.id)).ToList();
                jsonresult.Base.clickinfo = jsonresult.Base.clickinfo.Where(x => !string.IsNullOrEmpty(x.eventid)).ToList();
                jsonresult.Base.modelinfo = jsonresult.Base.modelinfo.Where(x => !string.IsNullOrEmpty(x.id)).ToList();
                jsonresult.Base.openeventstate = jsonresult.Base.openeventstate.Where(x => !string.IsNullOrEmpty(x.enventid)).ToList();
                jsonresult.Base.closeeventstate = jsonresult.Base.closeeventstate.Where(x => !string.IsNullOrEmpty(x.enventid)).ToList();
            }
            catch (Exception ex)
            {
                jsonresult = null;
            }
            var ttbookdir = bookdir==null ? -1 : bookdir.BookID;
            var bookid = ttbookdir;
            var product =  _productService.GetProductById(bookid);

            result.WebModelUrl = (result.WebModelUrl ?? "").Replace("，", ",");
            var arr = result.WebModelUrl.Split(",").ToList().Where(x => !string.IsNullOrEmpty(x)).ToList();
            arr.ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x))
                {
                    x = _config.HostLuaResource ?? "" + x;
                }

            });
            return Json(new
            {
                code = 0,
                msg = "已成功",
                data = new
                {
                    complexLevel = result.ComplexLevel,
                    BookID = bookdir == null ? -1 : bookdir.BookID,
                    BookNodeName = result.Name,
                    BookName=product==null? "未知":product.Name,
                    appointStrJson = new
                    {
                        keyname = result.UniqueID??"",
                        head = _config.HostLuaResource ?? "",
                        lua = _config.HostLuaResource ?? "" + result.WebGltfUrl ?? "",
                        assetbundle = arr
                    },
                    strJson = jsonresult == null ?new object(): jsonresult
                }
            });
        } 
        public IActionResult GetKnowledgeByImgName(string imgName)
        {
            if (string.IsNullOrEmpty(imgName))
            {
                return Json(new
                {
                    code =-1,
                    msg ="识别图名称不能为空",
                    data = new object()
                });
            }

            var result = _aiBookService.GetAiBookModelByArImgName(imgName);
            if (result == null)
            {
                return Json(new
                {
                    code = -1,
                    msg = "知识点不存在",
                    data = new object()
                });                   
            }
            else
            {
                if (result != null && !string.IsNullOrEmpty(result.UniqueID))
                {
                    result.ComplexLevel = 1;
                }
                if (result == null || (result.ComplexLevel == 0 && string.IsNullOrEmpty(result.UnityStrJson)))
                {
                    return Json(new
                    {
                        code = -1,
                        msg = "未对知识点进行泛化",
                        data = new object()
                    });
                }
                BookDir bookdir = null;
                if (result != null && result.BookDirID > 0)
                {
                    bookdir = _bookDirService.GetBookDirById(result.BookDirID);                
                }
                var ttbookdir = bookdir == null ? -1 : bookdir.BookID;
                var bookid = ttbookdir;
                var product = _productService.GetProductById(bookid);

                //BookNodeNewRoot jsonresult = new BookNodeNewRoot();

                //try
                //{

                //    jsonresult = JsonConvert.DeserializeObject<BookNodeNewRoot>(result.UnityStrJson);
                //    jsonresult.Base.buttoninfo = jsonresult.Base.buttoninfo.Where(x => !string.IsNullOrEmpty(x.id)).ToList();
                //    jsonresult.Base.textinfo = jsonresult.Base.textinfo.Where(x => !string.IsNullOrEmpty(x.id)).ToList();
                //    jsonresult.Base.imageinfo = jsonresult.Base.imageinfo.Where(x => !string.IsNullOrEmpty(x.id)).ToList();
                //    jsonresult.Base.videoinfo = jsonresult.Base.videoinfo.Where(x => !string.IsNullOrEmpty(x.id)).ToList();
                //    jsonresult.Base.audioinfo = jsonresult.Base.audioinfo.Where(x => !string.IsNullOrEmpty(x.id)).ToList();
                //    jsonresult.Base.camerainfo = jsonresult.Base.camerainfo.Where(x => !string.IsNullOrEmpty(x.id)).ToList();
                //    jsonresult.Base.clickinfo = jsonresult.Base.clickinfo.Where(x => !string.IsNullOrEmpty(x.eventid)).ToList();
                //    jsonresult.Base.modelinfo = jsonresult.Base.modelinfo.Where(x => !string.IsNullOrEmpty(x.id)).ToList();
                //    jsonresult.Base.openeventstate = jsonresult.Base.openeventstate.Where(x => !string.IsNullOrEmpty(x.enventid)).ToList();
                //    jsonresult.Base.closeeventstate = jsonresult.Base.closeeventstate.Where(x => !string.IsNullOrEmpty(x.enventid)).ToList();
                //}
                //catch (Exception ex)
                //{
                //    jsonresult = null;
                //}
                //result.WebModelUrl = (result.WebModelUrl ?? "").Replace("，", ",");
                //var arr = result.WebModelUrl.Split(",").ToList().Where(x => !string.IsNullOrEmpty(x)).ToList();
                //List<string> allList = new List<string>();
                //arr.ForEach(x =>
                //{
                //    if (!string.IsNullOrEmpty(x))
                //    {
                //        x = (_config.HostLuaResource ?? "") + x;

                //        allList.Add(x);
                //    }

                //});
                //var luaurl = _config.HostLuaResource ?? "" + result.WebGltfUrl ?? "";

                return Json(new
                {
                    code = 0,
                    msg = "已成功",
                    data = new
                    {
                        Id = result.Id,
                        BookID = bookdir == null ? -1 : bookdir.BookID,
                        BookNodeName = result.Name,
                        BookName = product == null ?"":product.Name
                        //appointStrJson = new
                        //{
                        //    keyname = result.UniqueID??"",
                        //    head = _config.HostLuaResource ?? "",
                        //    lua = _config.HostLuaResource ?? "" + result.WebGltfUrl ?? "",
                        //    assetbundle = allList
                        //},
                        //strJson = jsonresult == null ? new object() : jsonresult
                    }
                });
            }
        }
        public IActionResult GetAllBookNodesARInfo(string userName)
        {
          var result =  _aiBookService.GetAllAiBookModels().Where(x=>!x.Deleted).OrderBy(x=>x.DisplayOrder).ToList();
            return Json(new
            {
               code = 0,
               msg = "获取成功",
               data = result.Where(x=>!string.IsNullOrEmpty(x.AbUrl)).Select(x=>new {
                   path = x.WebBinUrl??"",
                   kid = x.BookDirID,
                   name = x.AbUrl??""
                 
               })
            });
        }
        public IActionResult GetBookNodeKeyName(string keyname)
        {

          var result=  _bookNodeTagService.GetAllBookNodeTagsByBookNodeBySearchName(keyname);


            List<Core.Domain.AIBookModel.AiBookModel> aibookList = new List<Core.Domain.AIBookModel.AiBookModel>();



            result.ToList().ForEach(x => 
            {
                x.BookNodeBookNodeTagMappings.ToList()
                .ForEach(y =>
                {

                 // var oldaibook = aibookList.FirstOrDefault(y=>y.Id == x.)

                    aibookList.Add(y.BookNode);
                });

            });
            var ids = aibookList.Select(x => x.BookDirID).ToList();

           var listBookDir = _bookDirService.GetChildBookDirItems(ids);

            var pids = listBookDir.Select(x => x.BookID).ToList();

            ///书籍集合
           var bresult = _productService.GetProductsByIds(pids.ToArray());
            //result.BookNodeBookNodeTagMappings.Select(x=>x.BookNodeTagId)
            //var booknode = _aiBookService.ge


            var ksts = aibookList.Select(x =>
            {

                var bookId = string.Empty;
                var bookName = string.Empty;

              var res =   listBookDir.FirstOrDefault(y=>y.Id==x.BookDirID);

                if(res != null)
                {
                    var book = bresult.FirstOrDefault(y => y.Id == res.BookID);

                    if (book != null)
                    {
                        bookId = book.Id.ToString();
                        bookName = (book.Name??"");
                    }
                }
                return new
                {
                    bookid = bookId,
                    bookname = bookName,
                    kid = x.BookDirID,
                    name = x.Name,
                    imgurl =_pictureService.GetPictureUrl(int.Parse(x.ImgUrl??"0")),
                };

            }).Distinct().ToList();

            
            return Json(new
            {
                code = 0,
                msg = "获取成功",
                data = ksts
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
        [HttpPost]
        public IActionResult SubmitComment(AddBookNodeCommentModel request)
        {

            var bookNode = _aiBookService.GetAiBookModelById(request.BookNodeID);          
            if (bookNode == null)
            {
                return Json(new
                {
                    code = -1,
                    msg = "知识点不存在",
                    data = false
                });
            }
            var comment = new Nop.Core.Domain.AIBookModel.BookNodeComment
            {
                BookNodeId = bookNode.Id,
                CustomerId = request.CustomerId,
                CommentTitle = request.CommentTitle,
                CommentText = request.CommentText,
                IsApproved = false,
                StoreId = 1,
                CreatedOnUtc = DateTime.UtcNow,
            };
            bookNode.UpdatedOnUtc = DateTime.Now;
            bookNode.BookNodesComments.Add(comment);
       
           var result = _aiBookService.UpdateAiBookModel(bookNode);

            if (result > 0)
            {
                return Json(new
                {
                    code = 0,
                    msg = "评论提交成功",
                    data = true

                });
            }
            return Json(new {
                code = 0,
                msg = "未知错误",
                data = true

            });
        }
    }
}