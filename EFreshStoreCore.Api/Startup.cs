using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Timers;
using System.Web.Http;
using EFreshStoreCore.Api.Utility;
using EFreshStoreCore.Manager;
using EFreshStoreCore.Model.Context;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(EFreshStoreCore.Api.Startup))]

namespace EFreshStoreCore.Api
{
    //public partial class Startup
    //{
    //    public void Configuration(IAppBuilder app)
    //    {
    //        ConfigureAuth(app);

    //    }
    //}
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            OAuthAuthorizationServerOptions options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,

                //The Path For generating the Toekn
                TokenEndpointPath = new PathString("/api/token"),

                //Setting the Token Expired Time (24 hours)
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),

                //MyAuthorizationServerProvider class will validate the user credentials
                Provider = new AuthorizationServiceProvider()
            };

            //Token Generations
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);

            Timer t = new Timer(TimeSpan.FromDays(1).TotalMilliseconds); // Set the time (5 mins in this case)
            t.AutoReset = true;
            t.Elapsed += new System.Timers.ElapsedEventHandler(Mail);
            t.Start();
        }

        public async void Mail(object sender, ElapsedEventArgs e)
        {
            try
            {
                OrderManager orderManager = new OrderManager();
                MasterDepotManager masterDepotManager = new MasterDepotManager();
                List<MasterDepot> masterDepots = masterDepotManager.GetAll().ToList();

                foreach (var depot in masterDepots)
                {
                    var ordersCount = orderManager.CountDailyOrders(depot.Id);
                    string subject = "[Meghna e-Commerce] Placed Orders";
                    string body = "Dear " + depot.ContactPerson + Environment.NewLine;
                    body += Environment.NewLine;
                    body += "You have " + ordersCount + " pending orders" + Environment.NewLine;
                    body += "Please check the the order below: " + Environment.NewLine;
                    body += Environment.NewLine;
                    body += "Regards" + Environment.NewLine;
                    body += "Meghna Group";
                    MailAddress mailAddress = new MailAddress(depot.Email, depot.Name);
                    if (!string.IsNullOrWhiteSpace(depot.Email) && UtilityClass.CheckForInternetConnection())
                    {
                        Email.SendEmail(subject, body, mailAddress);
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}