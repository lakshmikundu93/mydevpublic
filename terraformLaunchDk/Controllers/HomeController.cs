using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using terraformLaunchDk.Models;
using LaunchDarkly.Sdk;
using LaunchDarkly.Sdk.Server;

namespace terraformLaunchDk.Controllers
{
    public class HomeController : Controller
    {
        public const string SdkKey = "sdk-ae0659d7-c855-4eea-b292-f655620a5e59";
        public const string FeatureFlagKey = "DemoFlag";
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var ldConfig = Configuration.Default(SdkKey);
            var client = new LdClient(ldConfig);

            if (client.Initialized)
            {
                Console.WriteLine("SDK successfully initialized!");
            }
            else
            {
                Console.WriteLine("SDK failed to initialize");
            }
            //ff_demo_002 //example-user-key
            var user = LaunchDarkly.Sdk.User.WithKey("example-user-key");
            Console.WriteLine(user.Name);
            var flagValue = client.BoolVariation(FeatureFlagKey, user, false);
            Console.WriteLine(flagValue);

            if (flagValue)
            {
                ViewBag.buttonTxt = "Implemented LaunchDarkly (feature flag enabled for '" + user.Name + "')";
                ViewBag.buttonUrl = "https://apidocs.launchdarkly.com/tag/Feature-flags#operation/getFeatureFlag";
            }
            else
            {
                ViewBag.buttonTxt = "Feature flag disabled for '" + user.Name ;
                ViewBag.buttonUrl = "https://www.youtube.com/watch?v=se4ktNJubjQ";
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
    }
}