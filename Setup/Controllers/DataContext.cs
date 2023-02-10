namespace Setup.Controllers;
using Microsoft.AspNetCore.Mvc;
public  class DataContext : Controller
{
   private const string PageViews = "PageViews";
   public int views { get; set; }
}
      
