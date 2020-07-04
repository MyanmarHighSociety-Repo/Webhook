using FacebookMessenger.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FlowController.PageFlows
{
    public abstract class PageFlow
    {
        public abstract void ProcessFlow(RequestMessagingModel pEntry, PageModel page, Action<ResponseModel,String> actResponse);
    }
}
