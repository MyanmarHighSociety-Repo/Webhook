using FacebookMessenger;
using FacebookMessenger.Enums;
using FacebookMessenger.Models;
using FlowController.PageFlows;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FlowController
{
    public class FlowManager
    {
        private string Host = "https://api.shopdoora.com/webhook";

        private List<PageModel> Pages;              
        private List<PageFlow> Flows;

        public FlowManager()
        {
            Flows = new List<PageFlow>();
            Pages = new List<PageModel>();
        }

        public FlowManager(List<PageModel> pages) : this()
        {
            this.Pages = pages;
        }

        public void AddPage(PageModel page)
        {
            Pages.Add(page);
        }

        public bool RemovePage(PageModel page)
        {
            return Pages.Remove(page);
        }

        public bool RemovePage(string pageId)
        {
            var page = Pages.FirstOrDefault(p => p.ID == pageId);

            if (page != null)
            {
                return Pages.Remove(page);
            }

            return false;
        }

        public void ProcessFlow(RequestModel request)
        {
            Log.Information("Start processing message.");

            foreach (var entry in request.Entries)
            {
                Log.Information("Entry " + entry.ID);
                var pageflow = GetPageFlow(entry.ID);

                if (pageflow != null)
                {
                    var requestMessaging = entry.Messaging.FirstOrDefault();
                    Log.Information("Flow type is " + pageflow.GetType().Name);
                    
                    pageflow.ProcessFlow(requestMessaging,(response, api) =>
                    {
                        Log.Information("Response Type" + response?.Message.GetType().Name);
                        MessageHandler.ResponseMessage(response, pageflow.Page.Token, api);
                    });

                }
            }
        }

        public void SendReceiptToChat(string pageID, string recipentID, ReceiptAttachmentPayloadModel receipt)
        {
            Log.Information("Send receipt to FB Chat.");
            var pageflow = GetPageFlow(pageID);             

            if (pageflow != null)
            {
                Log.Information("Flow type is " + pageflow.GetType().Name); 

               var personAwaiter = MessageHandler.GetProfileInfo(recipentID, GetPage(pageID)).GetAwaiter();
                personAwaiter.OnCompleted(() =>
                { 
                    var person = personAwaiter.GetResult();
                    if(person != null)
                    {
                        receipt.RecipientName = person.FirstName + " " + person.LastName;
                    }                     
                    var response = new ResponseModel()
                    {
                        Receiver = new PersonModel()
                        {
                            ID = recipentID
                        },
                        MessageType = MessageType.RESPONSE,
                        Message = new TemplateMessageModel()
                        {
                            Attachment = new AttachmentModel()
                            {
                                Type = AttachmentType.template,
                                Payload = receipt
                            }
                        }
                    };
                    MessageHandler.ResponseMessage(response, pageflow.Page.Token, FacebookApiURL.Message_V70URL);
                });
              
            }
        }

        private PageModel GetPage(string id)
        {
            return Pages.FirstOrDefault(x => x.ID == id);
        }
               
        private PageFlow GetPageFlow(string id)
        {
            var flow = Flows.FirstOrDefault(p => p.Page.ID == id);

            if(flow != null)
            {
                return flow;
            }
            else
            {
               var newFlow =  CreatePageFlow(id);
                Flows.Add(newFlow);
                return newFlow;
            }

        }

        private PageFlow CreatePageFlow(string id)
        {
            PageModel page = GetPage(id);
            //Need to update this if new page is added
            //These page ids are constant
            return id switch
            {
                "112932287133096" => new MHSDemoFlow(page, Host),
                "416991435163889" => new YaypansarFlow(page, Host),
                _ => null,
            };
        }
    }

    
}
