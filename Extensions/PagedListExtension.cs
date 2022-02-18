namespace GoldPocket.Extensions
{
    public class PagedList<T> 
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int count { get; set; }
        public int pages { get; set; }
        public bool sortable { get; set; }
        public string sortby { get; set; }
        public string search { get; set; }
        public List<PageStatus> pageList {get; set; } = new List<PageStatus>();
        public List<T> data { get; set; }

        public string sortName(string n) {
            var s = sortby ?? $"{n}-a";
            if(s == $"{n}-a"){
                return $"{n}-d";
            }
            return $"{n}-a";
        }

        public string sortClass(string n) {
            var s = sortby ?? "";
            if(s == $"{n}-a")
            {
                return $"sortby des";
            }
            if(s == $"{n}-d") 
            {
                return $"sortby asc";
            }
                
            return $"sortby";
        }
    }

    public class PageStatus {
        public int page { get; set; }
        public string href { get; set; }
        public string Lasthref { get; set; }
        public string Nexthref { get; set; }
        public bool active { get; set; }
        public bool hasNext { get; set; }
        public bool hasPrevious { get; set; }
        public int showing { get; set; }
        public int total { get; set; }
    }

    public static class PagedListExtension
    {
        public static int pageSize = 10;
        public static int shownPages = 2;
        public static PagedList<T> ToPagedList<T>(this List<T> list, int pageNumber, string param = "page", bool sortable = false, string search = "", string sortby = "")        
        {
            sortby = sortby ?? "";
            search = search ?? "";
            PagedList<T> pagedList = new PagedList<T>();
            pagedList.pages = Convert.ToInt32(Math.Ceiling((double) list.Count() / pageSize));
            pagedList.count = list.Count();
            pagedList.sortable = sortable;
            pagedList.sortby = sortby;
            pagedList.search = search;

            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageNumber = pageNumber > pagedList.pages  ? pagedList.pages : pageNumber;

            pagedList.pageNumber = pageNumber;
            pagedList.pageSize = pageSize;
            pagedList.data = list.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
            

            var startpage = pageNumber - shownPages < 1? 1 : pageNumber - shownPages;
            var endpage = pageNumber + shownPages > pagedList.pages? pagedList.pages : pageNumber + shownPages;
            for(var i = startpage; i <= endpage; i++){
                pagedList.pageList.Add(new PageStatus {
                    page = i,
                    href = $"?{param}={i}",
                    Lasthref = $"?{param}={i-1}",
                    Nexthref = $"?{param}={i+1}",
                    active = i == pageNumber,
                    hasPrevious = i != 1,
                    hasNext = i != pagedList.pages,
                    showing = pageSize,
                    total = pagedList.count,
                });
            }
            return pagedList;
        }
        public static PagedList<T> ToPagedList<T>(this List<T> list)        
        {
            PagedList<T> pagedList = new PagedList<T>();
            pagedList.count = list.Count();
            pagedList.data = list.ToList();
            pagedList.pageList.Add(new PageStatus{
                page = 0,
                active = true,
                showing = pageSize,
                total = pagedList.count
            });
            return pagedList;
        }
    }
    
}