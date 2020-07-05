using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FBChat.Models;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Configuration;
using FacebookMessenger;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FacebookMessenger.Models;
using FlowController;

using FBPage = FacebookMessenger.Models.PageModel;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using FacebookMessenger.Helper;
using System.Diagnostics.Eventing.Reader;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using FacebookMessenger.Enums;
using System.Security.AccessControl;
using System.Reflection;

namespace FBChat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> log;       
        private IConfiguration _config;
        private FacebookSettingModel facebookSetting;
        private MessageHandler messenger;
        private FlowManager flowManager;


        public HomeController(IConfiguration config, ILogger<HomeController> logger, IOptions<FacebookSettingModel> fbSetting, IWebHostEnvironment  env)
        {
            _config = config;
            log = logger;
            facebookSetting = fbSetting.Value;
            flowManager = new FlowManager(facebookSetting?.Pages);
            messenger = new MessageHandler();

            if (env.EnvironmentName == "Development")
            {
               // TestMethod();
            }

        }

        #region "Web page methods"
        public IActionResult Index()
        { 
            return View();
        }

        [HttpGet]
        public IActionResult Shopping(string data)
        {
            try
            {
                HttpContext.Session.SetString("order", data);
                var order = JsonConvert.DeserializeObject<OrderModel>(Cryptography.Decrypt(data));                
                return View(new ItemViewModel()
                {
                    ID = order.ItemID,
                    Name = "Item - " + order.ItemID,
                    Descriptions = "The item is just for testing.",
                    Price = 10.05f,
                });
            }catch(Exception ex)
            {
                log.LogError("Shopping => " + ex.Message);
            }

            return View();
        }

        [HttpPost]
        public IActionResult Shopping(ItemViewModel model)
        {
            try
            {
               string hostURL = "https://api.shopdoora.com/webhook";
               string strOrder = HttpContext.Session.GetString("order");
               var order = JsonConvert.DeserializeObject<OrderModel>(Cryptography.Decrypt(strOrder));              
               
                ReceiptAttachmentPayloadModel receipt = new ReceiptAttachmentPayloadModel()
                {
                    Type = TemplateType.receipt,
                    RecipientName = "",
                    OrderNumber = "t12121",
                    CurrencyCode = "USD",
                    PaymentMethod = "Cash",
                    Address = new AddressModel()
                    {
                        Street1 = "Street1",
                        Street2  = "Street2",
                        City = "Yangon",
                        State = "Yangon",
                        PostalCode = "121212",
                        Country = "Myanmar"
                    },

                    Summary = new ReceiptSummaryModel()
                    {
                        Subtotal = model.Price,
                        ShippingCost = 0,
                        TotalTax = 0,
                        TotalCost = model.Price
                    },
                    Elements = new List<ReceiptElementModel>()
                    {
                       new ReceiptElementModel()
                       {
                           Title = model.Name,
                           Subtitle = "Subtitle",
                           Quantity = 1,
                           Price = model.Price,
                           Currency = "MMK",
                           Image =   model.ID switch
                                    {
                                    "1234567890" => hostURL + "/image/shirts/shirt1.jpg",
                                    "1234567891" => hostURL + "/image/shirts/shirt1.jpg",
                                    "1234567892" => hostURL + "/image/shirts/shirt2.jpg",
                                    "1234567893" => hostURL + "/image/shirts/shirt3.png",
                                     _ => "",
                                    }
                        }
                    }
                     
                };

                flowManager.SendReceiptToChat(order.PageID, order.RecipentID, receipt);
                
               return View(model);
            }
            catch (Exception ex)
            {
                log.LogError("Shopping => " + ex.Message); 
            }

            return View();
        }


        public IActionResult Privacy()
        { 
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion "Web page methods"

        #region "Facebook Integration"

        //App verfication
        [HttpGet]
        public string Webhook(
        [FromQuery(Name = "hub.mode")] string mode,
        [FromQuery(Name = "hub.challenge")] string challenge,
        [FromQuery(Name = "hub.verify_token")] string verify_token)
        {
            log.LogInformation($"mode {mode} , challenge {challenge}, verify_token {verify_token}");
             
            if (verify_token.Equals(facebookSetting?.AppToken))
            {
                return challenge;
            }
            else
            {
                return "invalid";
            }
        }


        //FB call this webhook for messages interaction
        [HttpPost]
        public void Webhook()
        {           
            try
            { 
                var awaitMsg = MessageHandler.GetMessage(this.Request.Body).GetAwaiter();
                awaitMsg.OnCompleted(() => {
                    var request  = awaitMsg.GetResult();
                    log.LogInformation("Entry count " + request.Entries.Count);

                    try
                    {
                        flowManager.ProcessFlow(request);
                    }catch(Exception err)
                    {
                        log.LogError("ProcessFlow error." + err.Message);
                    }
                    
                });                
            }
            catch (Exception ex)
            {
                log.LogError("Webhook " + ex.Message);
                return;
            }
        }

        #endregion "Facebook Integration"


        private void TestMethod()
        {
            string data = "{\"object\":\"page\",\"entry\":[{\"id\":\"112932287133096\",\"time\":1593285279652,\"messaging\":[{\"sender\":{\"id\":\"4140978342642540\"},\"recipient\":{\"id\":\"112932287133096\"},\"timestamp\":1593285279393,\"message\":{\"mid\":\"m_a - yC1vWeX2RmkToRLBxEoprGv7qYMWSah2IugA7RZhpFGvL9GoW - mQrtn1jHscI - p9C1aazRxqFRnkc54cd8Yg\",\"text\":\"test\"}}]}]}";
          //hi, hello
              data = "{\"object\":\"page\",\"entry\":[{\"id\":\"112932287133096\",\"time\":1593285279652,\"messaging\":[{\"sender\":{\"id\":\"4140978342642540\"},\"recipient\":{\"id\":\"112932287133096\"},\"timestamp\":1593285279393,\"message\":{\"mid\":\"m_a - yC1vWeX2RmkToRLBxEoprGv7qYMWSah2IugA7RZhpFGvL9GoW - mQrtn1jHscI - p9C1aazRxqFRnkc54cd8Yg\",\"text\":\"Hello\"}}]}]}";
            //quick reply
            // data = "{ \"object\":\"page\",\"entry\":[{\"id\":\"112932287133096\",\"time\":1593488946698,\"messaging\":[{\"sender\":{\"id\":\"4140978342642540\"},\"recipient\":{\"id\":\"112932287133096\"},\"timestamp\":1593488946390,\"message\":{\"mid\":\"m_rAc4cSPd-eycGfFyDibw15rGv7qYMWSah2IugA7RZhppd_i2soytP_mWhF6z9Emlmo3xlNKoOPWhrImZ-uKPrQ\",\"text\":\"Green\",\"quick_reply\":{\"payload\":\"pick-green\"}}}]}]}";
            // data = "{ \"object\":\"page\",\"entry\":[{\"id\":\"112932287133096\",\"time\":1593529265743,\"messaging\":[{\"sender\":{\"id\":\"4140978342642540\"},\"recipient\":{\"id\":\"112932287133096\"},\"timestamp\":1593529265601,\"message\":{\"mid\":\"m_2h1ve684VhmCbnteMD2zhZrGv7qYMWSah2IugA7RZhqxboPwts7PYw7asPAShHUlbtx-rzsGetLZZulhz0GT0g\",\"text\":\"Shopping\",\"quick_reply\":{\"payload\":\"pick-shopping\"}}}]}]}";
            //  data = "{ \"object\":\"page\",\"entry\":[{\"id\":\"112932287133096\",\"time\":1593537641131,\"messaging\":[{\"sender\":{\"id\":\"4140978342642540\"},\"recipient\":{\"id\":\"112932287133096\"},\"timestamp\":1593537640900,\"postback\":{\"title\":\"Get Started\",\"payload\":\"get-started\"}}]}]}";
            //  data = "{\"object\":\"page\",\"entry\":[{\"id\":\"112932287133096\",\"time\":1593538267436,\"messaging\":[{\"sender\":{\"id\":\"4140978342642540\"},\"recipient\":{\"id\":\"112932287133096\"},\"timestamp\":1593538267148,\"postback\":{\"title\":\"Get Started\",\"payload\":\"get-started\"}}]}]}";
              data = "{\"object\":\"page\",\"entry\":[{\"id\":\"112932287133096\",\"time\":1593591614683,\"messaging\":[{\"sender\":{\"id\":\"4140978342642540\"},\"recipient\":{\"id\":\"112932287133096\"},\"timestamp\":1593591614492,\"postback\":{\"title\":\"View More\",\"payload\":\"view-more\"}}]}]}";
            var request = JsonConvert.DeserializeObject<RequestModel>(data);
            log.LogInformation("Entry count " + request.Entries.Count);

            flowManager.ProcessFlow(request); 
        }
    }
}
