using FacebookMessenger;
using FacebookMessenger.Enums;
using FacebookMessenger.Models;
using FacebookMessenger.Personalization;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FlowController.PageFlows
{
    internal class YaypansarFlow : PageFlow
    {
        internal YaypansarFlow(PageModel page, String hostURL): base(page, hostURL)
        {

        }

        internal override void Init()
        {
          
        }

        internal override void ProcessFlow(RequestMessagingModel messaging, Action<ResponseModel,String> actResponse)
        {

            ResponseModel response = new ResponseModel();
            try
            {
                Log.Information("Message " + messaging.Message?.Text);

                response.Receiver = messaging.Sender;
                response.MessageType = MessageType.RESPONSE;
                //getting some text

                if (messaging.Message != null)
                {
                    if (messaging.Message.Text != null &&
                        (messaging.Message.Text.ToLower().Contains("hello") ||
                        messaging.Message.Text.ToLower().Contains("hi")))
                    {
                        GetStarted(ref response);
                        actResponse(response,FacebookMessenger.FacebookApiURL.Message_V70URL);

                        Thread.Sleep(2000);

                        ViewMore(ref response);
                        actResponse(response, FacebookMessenger.FacebookApiURL.Message_V70URL);
                    } 
                    else
                    {
                        response.Message = new ResponseMessageModel
                        {
                            Text = "Hi, " + UserInfo.FirstName + ", \r\n <br/> Please try to type hi or hello to start conversation."
                        };
                    }
                }



            }
            catch (Exception ex)
            {
                Log.Error("fn-ProcessFlow => " + ex.Message);
            }

        }


        private void GetStarted(ref ResponseModel response)
        {
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
                                Title = "Welcome " + UserInfo.FirstName + ",",
                                ImageURL = HostURL + "/image/shirts/shirt1.jpg",
                                SubTitle = "This is subtitle. It is useful to describe what you want customer to know about your chatbot.",
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
                                        URL = "https://api.shopdoora.com/webhook?item=12312323"
                                    }
                                }
                            },
                            new ElementModel() {
                                Title = "This is seconde element for testing,",
                                ImageURL = HostURL + "/image/shirts/shirt2.jpg",
                                SubTitle = "Second element subtitle",
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
                                        URL = "https://api.shopdoora.com/webhook?item=12312323"
                                    }
                                }
                            },

                        }

                    }
                }
            };
        }



        private void ViewMore(ref ResponseModel response)
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
                                        URL = "https://peterssendreceiveapp.ngrok.io/collection",
                                        MessengerExtensions = true,
                                        WebviewHeight = WebviewHeightRatio.tall
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
                                        URL = "https://peterssendreceiveapp.ngrok.io/view?item=100",
                                        MessengerExtensions = false,
                                        WebviewHeight = WebviewHeightRatio.tall

                               }
                            },

                            new ElementModel() {
                                Title = "Classic Blue  T-Shirt",
                                ImageURL = "https://peterssendreceiveapp.ngrok.io/img/blue-t-shirt.pngs",
                                SubTitle = "100% Cotton, 200% Comfortable",
                                DefaultAction = new ActionModel()
                                {
                                        Type = ActionType.web_url,
                                        URL = "https://peterssendreceiveapp.ngrok.io/view?item=101",
                                        MessengerExtensions = true,
                                        WebviewHeight = WebviewHeightRatio.tall,
                                        FallbackURL = "https://peterssendreceiveapp.ngrok.io/"

                               },
                                Buttons = new List<ButtonModel>(){

                                    new ButtonModel()
                                    {
                                        Title =  "Shop Now",
                                        Type =  ActionType.web_url,
                                        URL ="https://peterssendreceiveapp.ngrok.io/shop?item=101",
                                        MessengerExtensions = true,
                                        WebviewHeight = WebviewHeightRatio.tall,
                                        FallbackURL =  "https://peterssendreceiveapp.ngrok.io/"
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


    }
}
