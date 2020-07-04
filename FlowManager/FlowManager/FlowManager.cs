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
        private List<PageModel> Pages;

        //Need to update this if new page is added
        private PageFlow MHSDemoFlow;
        private PageFlow YaypansarFlow;

        public FlowManager()
        {
            Pages = new List<PageModel>();
            MHSDemoFlow = new MHSDemoFlow();
            YaypansarFlow = new YaypansarFlow();
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

        public void ProcessFlow(RequestModel request, Action<ResponseModel, PageModel, String> callback)
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
                    var page = GetPage(entry.ID);
                    pageflow.ProcessFlow(requestMessaging, page, (response, api) =>
                     {
                         callback(response, page, api);
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
            //Need to update this if new page is added
            //These page ids are constant
            return id switch
            {
                "112932287133096" => MHSDemoFlow,
                "416991435163889" => YaypansarFlow,
                _ => null,
            };
        }
    }

    
}
