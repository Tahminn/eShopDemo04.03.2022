using Microsoft.AspNetCore.Http;

namespace eShopDemo.Utilities.File
{
    public static class Extension
    {
        public static bool CheckFileType(this IFormFile file, string type)
        {
            return file.ContentType.Contains(type);
        }

        public static bool CheckFileSize(this IFormFile file, long size)
        {
            return file.Length / 1048576 < size;
        }
    }
}
