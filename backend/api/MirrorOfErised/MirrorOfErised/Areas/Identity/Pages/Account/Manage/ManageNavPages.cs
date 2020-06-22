using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MirrorOfErised.Areas.Identity.Pages.Account.Manage
{
    public static class ManageNavPages
    {
        public static string Index => "Index";

        public static string Photos => "Photos";
        
        public static string UploadPhotos => "UploadPhotos";
        
        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string PhotosNavClass(ViewContext viewContext) => PageNavClass(viewContext, Photos);

        public static string UploadPhotosNavClass(ViewContext viewContext) => PageNavClass(viewContext, UploadPhotos);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}
