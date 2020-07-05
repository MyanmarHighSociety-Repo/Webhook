using FacebookMessenger;
using FacebookMessenger.Enums;
using FacebookMessenger.Helper;
using FacebookMessenger.Models;
using FacebookMessenger.Personalization;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading;

namespace FlowController.PageFlows
{
    internal class MHSDemoFlow : PageFlow
    {


        internal MHSDemoFlow(PageModel page, String hostURL) : base(page, hostURL) {

        }

        #region "Init Flow"
        internal override void Init()
        {
            Create_GetStartButton();          
        }

        private void Create_GetStartButton()
        {
            var getStartRequest = new GetStartRequestModel()
            {
                GetStart = new GetStartModel()
                {
                    Payload = "get-started"
                }
            };
            MessageHandler.ResponseMessage(getStartRequest, this.Page.Token, FacebookApiURL.Profile_V70URL);

        }

        #endregion "Init Flow"

        #region "Process Flow"
        internal override void ProcessFlow(RequestMessagingModel messaging, Action<ResponseModel, String> actResponse)
        {

            ResponseModel response = new ResponseModel();
            try
            {
                Log.Information("Message " + messaging.Message?.Text);

                response.Receiver = messaging.Sender;
                response.MessageType = MessageType.RESPONSE;
                //getting some text

                if (messaging.Postback != null)
                {
                    Log.Information("This is postback call");
                    Log.Information(JsonConvert.SerializeObject(messaging.Postback));

                    if (messaging.Postback.Payload == "get-started")
                    {
                        Create_PersistenceMenu(response.Receiver.ID);
                        Shopping(ref response);
                        actResponse(response, FacebookApiURL.Message_V70URL);
                    }
                    else if (messaging.Postback.Payload == "view-more")
                    {
                        ViewMore(ref response);
                        actResponse(response, FacebookApiURL.Message_V26URL);

                    }
                }

                else if (messaging.Message != null)
                {
                    if (messaging.Message.Text != null &&
                        (messaging.Message.Text.ToLower().Contains("hello") ||
                        messaging.Message.Text.ToLower().Contains("hi")))
                    {
                        Msg_Greeting(ref response);
                        actResponse(response, FacebookApiURL.Profile_V70URL);

                        Thread.Sleep(1000);

                        PickColor(ref response);
                        actResponse(response, FacebookApiURL.Message_V70URL);
                    }
                    else if (messaging.Message.QuickReply != null)
                    {
                        process_quickReply(messaging.Message.QuickReply, ref response);
                        actResponse(response, FacebookApiURL.Message_V70URL);
                    }
                    //else
                    //{
                    //    response.Message = new ResponseMessageModel
                    //    {
                    //        Text = "Please try to type hi or hello to start conversation."
                    //    };
                    //    actResponse(response);
                    //}
                }



            }
            catch (Exception ex)
            {
                Log.Error("fn-ProcessFlow => " + ex.Message);
            }

        }

        private void Msg_Greeting(ref ResponseModel response)
        {
           
            Log.Information("This is gretting message");
            response.Message = new ResponseMessageModel()
            {
                Text = String.Format("Hi {0}, what can I help you?", MessageHandler.GetProfileInfo(response.Receiver.ID, this.Page).Result?.FirstName)
            };
        }

        private void process_quickReply(QuickReplyRequestModel quickReply, ref ResponseModel response)
        {
            if (quickReply.Payload == "pick-red")
            {
                response.Message = new ResponseMessageModel
                {
                    Text = "Red is the color of extremes. It's the color of passionate love, seduction, violence, danger, anger, and adventure. Our prehistoric ancestors saw red as the color of fire and blood – energy and primal life forces – and most of red's symbolism today arises from its powerful associations in the past."
                };
            }

            else if (quickReply.Payload == "pick-green")
            {
                response.Message = new ResponseMessageModel
                {
                    Text = "Green has strong emotional correspondence with safety. Dark green is also commonly associated with money. Green has great healing power. It is the most restful color for the human eye; it can improve vision."
                };
            }
            else if (quickReply.Payload == "pick-shopping")
            {
                Shopping(ref response);
            }
        }

        private void PickColor(ref ResponseModel response)
        {
            response.Message = new QuickReplyMessageModel()
            {
                Text = String.Format(@"Hi {0}, please pick a color :", MessageHandler.GetProfileInfo(response.Receiver.ID, this.Page).Result?.FirstName),

                QickReplies = new List<QuickReplyResponseModel>() {
                        new QuickReplyResponseModel()
                        {
                            ContentType = ContentType.Text,
                            Title = "Red",
                            Payload = "pick-red",
                            Image = HostURL + "/image/buttons/red.png"

                        },
                        new QuickReplyResponseModel()
                        {
                            ContentType = ContentType.Text,
                            Title = "Green",
                            Payload = "pick-green",
                            Image = HostURL + "/image/buttons/green.jpg"

                        },
                        new QuickReplyResponseModel()
                        {
                            ContentType = ContentType.Text,
                            Title = "Shopping",
                            Payload = "pick-shopping",
                            Image = HostURL + "/image/buttons/shopping.jpg"

                        }
                }

            };
        }

        private void Shopping(ref ResponseModel response)
        {
            var item0 = new OrderModel()
            {
                ItemID = "1234567890",
                PageID = this.Page.ID,
                RecipentID = response.Receiver.ID
            };


            var item0Str = Cryptography.encrypt(JsonConvert.SerializeObject(item0));

            response.Message = new TemplateMessageModel()
            {
                Attachment = new AttachmentModel()
                {
                    Type = AttachmentType.template,
                    Payload = new AttachmentPayloadModel()
                    {
                        Type = TemplateType.generic,
                        Elements = new List<ElementModel>()
                        {
                            new ElementModel() {
                                Title = "Element 1",
                                ImageURL = HostURL + "/image/shirts/shirt1.jpg",
                                SubTitle = "This is subtitle. It is useful to describe what you want customer to know about your chatbot.",
                                DefaultAction = new ActionModel()
                                {
                                    Type = ActionType.web_url,
                                    URL =  HostURL + "/shopping?data=" + item0Str,
                                    WebviewHeight = WebviewHeightRatio.full
                                },
                                Buttons = new List<ButtonModel>()
                                {
                                    new ButtonModel() {
                                        Title = "View More",
                                        Type = ActionType.postback,
                                        Payload = "view-more"
                                    },
                                    new ButtonModel() {
                                        Title = "Order",
                                        Type = ActionType.web_url,
                                        URL =  HostURL + "/shopping?data=" + item0Str,
                                    }
                                }
                            }
                        }

                    }
                }
            };
        }

        private void ViewMore(ref ResponseModel response)
        {
           var item1 = new OrderModel()
            {
                ItemID = "1234567891",
                PageID = this.Page.ID,
                RecipentID = response.Receiver.ID
            } ;

            var item2 = new OrderModel()
            {
                ItemID = "1234567892",
                PageID = this.Page.ID,
                RecipentID = response.Receiver.ID
            };

            var item3 = new OrderModel()
            {
                ItemID = "1234567893",
                PageID = this.Page.ID,
                RecipentID = response.Receiver.ID
            };


            var item1Str = Cryptography.encrypt(JsonConvert.SerializeObject( item1));
            var item2Str = Cryptography.encrypt(JsonConvert.SerializeObject(item2));
            var item3Str = Cryptography.encrypt(JsonConvert.SerializeObject(item3));



            response.Message = new TemplateMessageModel()
            {


                Attachment = new AttachmentModel()
                {
                    Type = AttachmentType.template,
                    Payload = new AttachmentPayloadModel()
                    {

                        Type = TemplateType.generic,
                        Elements = new List<ElementModel>()
                        {
                            new ElementModel() {
                                Title = "Element 1",
                                ImageURL = HostURL + "/image/shirts/shirt1.jpg",
                                SubTitle = "This is subtitle. It is useful to describe what you want customer to know about your chatbot.",
                                DefaultAction = new ActionModel()
                                {
                                    Type = ActionType.web_url,
                                    URL = HostURL+"/shopping?data=" + item1Str,
                                    WebviewHeight = WebviewHeightRatio.compact
                                },
                                Buttons = new List<ButtonModel>()
                                {
                                    new ButtonModel() {
                                        Title = "Order",
                                        Type = ActionType.web_url,
                                        URL = HostURL+"/shopping?data=" + item1Str
                                    }
                                }
                            },new ElementModel() {
                                Title = "Element 2",
                                ImageURL = HostURL + "/image/shirts/shirt2.jpg",
                                SubTitle = "This is subtitle. It is useful to describe what you want customer to know about your chatbot.",
                                DefaultAction = new ActionModel()
                                {
                                    Type = ActionType.web_url,
                                    WebviewHeight = WebviewHeightRatio.compact,
                                    URL = HostURL+"/shopping?data=" + item2Str,

                                },
                                Buttons = new List<ButtonModel>()
                                {
                                    new ButtonModel() {
                                        Title = "Order",
                                        Type = ActionType.web_url,
                                        URL = HostURL+"/shopping?data=" + item2Str,
                                    }
                                }
                            },
                            new ElementModel() {
                                Title = "Element 3",
                                ImageURL = HostURL + "/image/shirts/shirt3.png",
                                SubTitle = "This is subtitle. It is useful to describe what you want customer to know about your chatbot.",
                                DefaultAction = new ActionModel()
                                {
                                    Type = ActionType.web_url,
                                    WebviewHeight = WebviewHeightRatio.compact,
                                    URL = HostURL+"/shopping?data=" + item3Str
                                },
                                Buttons = new List<ButtonModel>()
                                {
                                    new ButtonModel() {
                                        Title = "Order",
                                        Type = ActionType.web_url,
                                        WebviewHeight = WebviewHeightRatio.compact,
                                        URL = HostURL+"/shopping?data=" + item3Str
                                    }
                                }
                            }

                        }

                    }
                }
            };
        }

        //deprecate at 4.0
        private void ViewMore1(ref ResponseModel response)
        {
            response.Message = new TemplateMessageModel()
            {
                Attachment = new AttachmentModel()
                {
                    Type = AttachmentType.template,
                    Payload = new AttachmentPayloadModel()
                    {
                        Type = TemplateType.list,
                        TopElementStyle = WebviewHeightRatio.compact,

                        Elements = new List<ElementModel>()
                        {
                            new ElementModel() {
                                Title = "Classic T-Shirt Collection",
                                ImageURL = HostURL + "/image/shirts/shirt3.png",
                                SubTitle = "See all our colors.",
                                Buttons = new List<ButtonModel>()
                                {
                                    new ButtonModel() {
                                        Title = "View",
                                        Type = ActionType.web_url,
                                        URL = HostURL + "/collection",
                                        MessengerExtensions = true,
                                        WebviewHeight = WebviewHeightRatio.tall,
                                        FallbackURL = HostURL + "/collection"
                                    }
                                }
                            },

                             new ElementModel() {
                                Title = "Classic White T-Shirt",
                                ImageURL = HostURL + "/image/shirts/shirt3.png",
                                SubTitle = "See all our colors",
                                DefaultAction = new ActionModel()
                                {
                                        Type = ActionType.web_url,
                                        URL = HostURL + "/view?item=100",
                                        MessengerExtensions = false,
                                        WebviewHeight = WebviewHeightRatio.tall

                               }
                            },

                            new ElementModel() {
                                Title = "Classic Blue  T-Shirt",
                                ImageURL = HostURL + "/image/shirts/shirt3.png",
                                SubTitle = "100% Cotton, 200% Comfortable",
                                DefaultAction = new ActionModel()
                                {
                                        Type = ActionType.web_url,
                                        URL = HostURL + "/view?item=101",
                                        MessengerExtensions = true,
                                        WebviewHeight = WebviewHeightRatio.tall

                               },
                                Buttons = new List<ButtonModel>(){

                                    new ButtonModel()
                                    {
                                        Title =  "Shop Now",
                                        Type =  ActionType.web_url,
                                        URL =HostURL + "/shop?item=101",
                                        MessengerExtensions = true,
                                        WebviewHeight = WebviewHeightRatio.tall,
                                        FallbackURL =  HostURL + "/fallback"
                                    }

                                }

                            },

                        },
                        Buttons = new List<ButtonModel>() {
                         new ButtonModel()
                         {
                             Title = "View More",
                             Type = ActionType.postback,
                             Payload = "view-more"
                         }
                        }

                    }
                }
            };
        }


        private void Create_PersistenceMenu(string psid)
        {
            var menuRequest = new PersistentMenuRequestModel()
            {
                PSID = psid,

                Menu = new List<PersistentMenuItemModel>()
                {

                    new PersistentMenuItemModel()
                    {
                        Locale = "default",
                        ComposerInputDisabled = false,
                        Actions = new List<MenuActionModel>()
                        {
                            new MenuActionModel()
                            {
                                Type = ActionType.postback,
                                Title = "Talk to an agent",
                                Payload = "CARE_HELP"
                            },
                             new MenuActionModel()
                            {
                                Type = ActionType.postback,
                                Title = "View More",
                                Payload = "view-more"
                            },
                              new MenuActionModel()
                            {
                                Type = ActionType.web_url,
                                Title = "Visit to Website",
                                URL = this.HostURL
                            }

                        }
                    }
                }
            };

            MessageHandler.ResponseMessage(menuRequest, this.Page.Token, FacebookApiURL.CustomUserSettings_V70URL);
        }

        #endregion "Process Flow"

    }

}
 
