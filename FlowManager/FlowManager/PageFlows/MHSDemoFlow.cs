using FacebookMessenger;
using FacebookMessenger.Enums;
using FacebookMessenger.Models;
using FacebookMessenger.Personalization;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FlowController.PageFlows
{
    public class MHSDemoFlow : PageFlow
    {
        private string Host = "https://api.shopdoora.com/webhook";

        public override void ProcessFlow(RequestMessagingModel messaging, PageModel page, Action<ResponseModel, String> actResponse)
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
                        Shopping(ref response);
                        actResponse(response, FacebookURL.Message_V70URL);
                    }
                    else if (messaging.Postback.Payload == "view-more")
                    {
                        ViewMore(ref response);
                        actResponse(response,FacebookURL.Message_V26URL);
                         
                    }
                }

                else if (messaging.Message != null)
                {
                    if (messaging.Message.Text != null &&
                        (messaging.Message.Text.ToLower().Contains("hello") ||
                        messaging.Message.Text.ToLower().Contains("hi")))
                    {
                        Msg_Greeting(ref response, page);
                        actResponse(response,FacebookURL.Profile_V70URL);

                        Thread.Sleep(1000);

                        PickColor(ref response,page);
                        actResponse(response, FacebookURL.Message_V70URL);
                    }
                    else if (messaging.Message.QuickReply != null)
                    {
                        process_quickReply(messaging.Message.QuickReply, ref response);
                        actResponse(response, FacebookURL.Message_V70URL);
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

        private void Msg_Greeting(ref ResponseModel response, PageModel page)
        {
            Log.Information("This is gretting message");
            response.Message = new ResponseMessageModel()
            {
                Text = String.Format("Hi {0}, what can I help you?", MessageHandler.GetProfileInfo(response.Receiver.ID, page).Result?.FirstName)
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

        private void PickColor(ref ResponseModel response, PageModel page)
        {           
            response.Message = new QuickReplyMessageModel()
            {
                Text = String.Format(@"Hi {0}, please pick a color :", MessageHandler.GetProfileInfo(response.Receiver.ID, page).Result?.FirstName),

                QickReplies = new List<QuickReplyResponseModel>() {
                        new QuickReplyResponseModel()
                        {
                            ContentType = ContentType.Text,
                            Title = "Red",
                            Payload = "pick-red",
                            Image = Host + "/image/buttons/red.png"

                        },
                        new QuickReplyResponseModel()
                        {
                            ContentType = ContentType.Text,
                            Title = "Green",
                            Payload = "pick-green",
                            Image = Host + "/image/buttons/green.jpg"

                        },
                        new QuickReplyResponseModel()
                        {
                            ContentType = ContentType.Text,
                            Title = "Shopping",
                            Payload = "pick-shopping",
                            Image = Host + "/image/buttons/shopping.jpg"

                        }
                }

            };
        }

        private void Shopping(ref ResponseModel response)
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
                                Title = "Element 1",
                                ImageURL = Host + "/image/shirts/shirt1.jpg",
                                SubTitle = "This is subtitle. It is useful to describe what you want customer to know about your chatbot.",
                                DefaultAction = new ActionModel()
                                {
                                    Type = ActionType.web_url,
                                    URL =  Host + "?item=12312323",
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
                                        URL = "https://api.shopdoora.com/webhook?item=12312323"
                                    }
                                }
                            },new ElementModel() {
                                Title = "Element 2",
                                ImageURL = Host + "/image/shirts/shirt2.jpg",
                                SubTitle = "This is subtitle. It is useful to describe what you want customer to know about your chatbot.",
                                DefaultAction = new ActionModel()
                                {
                                    Type = ActionType.web_url,
                                    URL =  Host + "?item=12312323",
                                    WebviewHeight = WebviewHeightRatio.compact
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
                                        URL = "https://api.shopdoora.com/webhook?item=12312323"
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
                                ImageURL = Host + "/image/shirts/shirt1.jpg",
                                SubTitle = "This is subtitle. It is useful to describe what you want customer to know about your chatbot.",
                                DefaultAction = new ActionModel()
                                {
                                    Type = ActionType.web_url,
                                    URL =  Host + "?item=12312323",
                                    WebviewHeight = WebviewHeightRatio.compact
                                },
                                Buttons = new List<ButtonModel>()
                                { 
                                    new ButtonModel() {
                                        Title = "Order",
                                        Type = ActionType.web_url,
                                        URL = "https://api.shopdoora.com/webhook?item=12312323"
                                    }
                                }
                            },new ElementModel() {
                                Title = "Element 2",
                                ImageURL = Host + "/image/shirts/shirt2.jpg",
                                SubTitle = "This is subtitle. It is useful to describe what you want customer to know about your chatbot.",
                                DefaultAction = new ActionModel()
                                {
                                    Type = ActionType.web_url,
                                    URL =  Host + "?item=12312323",
                                    WebviewHeight = WebviewHeightRatio.compact
                                },
                                Buttons = new List<ButtonModel>()
                                { 
                                    new ButtonModel() {
                                        Title = "Order",
                                        Type = ActionType.web_url,
                                        URL = "https://api.shopdoora.com/webhook?item=12312323"
                                    }
                                }
                            },
                            new ElementModel() {
                                Title = "Element 3",
                                ImageURL = Host + "/image/shirts/shirt3.png",
                                SubTitle = "This is subtitle. It is useful to describe what you want customer to know about your chatbot.",
                                DefaultAction = new ActionModel()
                                {
                                    Type = ActionType.web_url,
                                    URL =  Host + "?item=12312323",
                                    WebviewHeight = WebviewHeightRatio.compact
                                },
                                Buttons = new List<ButtonModel>()
                                {
                                    new ButtonModel() {
                                        Title = "Order",
                                        Type = ActionType.web_url,
                                        URL = "https://api.shopdoora.com/webhook?item=12312323"
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
                                ImageURL = Host + "/image/shirts/shirt3.png",
                                SubTitle = "See all our colors.",
                                Buttons = new List<ButtonModel>()
                                {
                                    new ButtonModel() {
                                        Title = "View",
                                        Type = ActionType.web_url,
                                        URL = Host + "/collection",
                                        MessengerExtensions = true,
                                        WebviewHeight = WebviewHeightRatio.tall,
                                        FallbackURL = Host + "/collection"
                                    }
                                }
                            },

                             new ElementModel() {
                                Title = "Classic White T-Shirt",
                                ImageURL = Host + "/image/shirts/shirt3.png",
                                SubTitle = "See all our colors",
                                DefaultAction = new ActionModel()
                                {
                                        Type = ActionType.web_url,
                                        URL = Host + "/view?item=100",
                                        MessengerExtensions = false,
                                        WebviewHeight = WebviewHeightRatio.tall

                               }
                            },

                            new ElementModel() {
                                Title = "Classic Blue  T-Shirt",
                                ImageURL = Host + "/image/shirts/shirt3.png",
                                SubTitle = "100% Cotton, 200% Comfortable",
                                DefaultAction = new ActionModel()
                                {
                                        Type = ActionType.web_url,
                                        URL = Host + "/view?item=101",
                                        MessengerExtensions = true,
                                        WebviewHeight = WebviewHeightRatio.tall

                               },
                                Buttons = new List<ButtonModel>(){

                                    new ButtonModel()
                                    {
                                        Title =  "Shop Now",
                                        Type =  ActionType.web_url,
                                        URL =Host + "/shop?item=101",
                                        MessengerExtensions = true,
                                        WebviewHeight = WebviewHeightRatio.tall,
                                        FallbackURL =  Host + "/fallback"
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
 
