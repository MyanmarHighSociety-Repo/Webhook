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

        public void ProcessFlow(RequestModel request, Action<ResponseModel, String, String> callback)
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
                         callback(response, pageflow.Page.Token, api);
                     });

                }
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
