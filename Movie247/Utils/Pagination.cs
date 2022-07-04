using Movie247.Helpers;

namespace Movie247.Utils
{
    public class Pagination
    {
        public static PagingModel Paging(string currentUrl, string currentQueryString, int currentPage, int countPage)
        {
            PagingModel pagingModel = new();
            pagingModel.CurrentPage = currentPage;
            pagingModel.CountPage = countPage;
            if (currentQueryString.Length > 0)
            {
                if (currentQueryString.Contains("?page="))
                {
                    currentQueryString = currentQueryString.Replace("?page=" + currentPage, "");
                }
                else
                {
                    currentQueryString = currentQueryString.Replace("&page=" + currentPage, "");
                }

                pagingModel.GenerateUrl = (int? p) =>
                {
                    if (currentQueryString.Length > 0)
                    {
                        return currentQueryString + "&page=" + p;
                    }
                    else
                    {
                        return currentUrl + "?page=" + p;
                    }
                };
            }
            else
            {
                pagingModel.GenerateUrl = (int? p) =>
                {
                    return currentUrl + "?page=" + p;
                };
            }
            return pagingModel;
        }
    }
}
