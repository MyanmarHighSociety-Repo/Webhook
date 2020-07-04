using FacebookMessenger.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlowController.PageFlows
{
    internal abstract class PageFlow
    {
        internal PageModel Page;
        internal String HostURL;

        internal PageFlow(PageModel page, String hostURL)
        {
            Page = page;
            HostURL = hostURL;
            this.Init();
        }
        internal abstract void Init();
        internal abstract void ProcessFlow(RequestMessagingModel pEntry, Action<ResponseModel,String> actResponse);

       

    }
}
