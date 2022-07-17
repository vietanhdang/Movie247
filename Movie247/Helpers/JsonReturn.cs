using Microsoft.AspNetCore.Mvc;

namespace Movie247.Helpers
{
    public class JsonObject
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
    public class JsonReturn
    {
        public static JsonResult Success(string message, object data)
        {
            return new JsonResult(new JsonObject()
            {
                Success = true,
                Message = message,
                Data = data
            });
        }
        public static JsonResult Success(string message)
        {
            return new JsonResult(new JsonObject()
            {
                Success = true,
                Message = message
            });
        }
        public static JsonResult Success()
        {
            return new JsonResult(new JsonObject()
            {
                Success = true
            });
        }
        public static JsonResult Error(string message)
        {
            return new JsonResult(new JsonObject()
            {
                Success = false,
                Message = message
            });
        }
        public static JsonResult Error(string message, object data)
        {
            return new JsonResult(new JsonObject()
            {
                Success = false,
                Message = message,
                Data = data
            });
        }
    }
}
