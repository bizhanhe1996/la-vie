namespace LaVie.Extras.Utils;

using LaVie.Controllers;

public class Paginator
{
    public Paginator(BaseController owner)
    {
        baseController = owner;
    }

    private BaseController baseController;
    private int _page;
    private int _size;
    private int _totalCount;
    private int _pagesCount;
    public int SkipCount { set; get; }
    public int TakeCount { set; get; }

    public Paginator SetTotalCount(int totalCount)
    {
        _totalCount = totalCount;
        return this;
    }

    public Paginator SetPage(int page)
    {
        _page = page;
        return this;
    }

    public Paginator SetSize(int size)
    {
        _size = size;
        return this;
    }

    private void _allRecords()
    {
        baseController.ViewBag.Pages = 1;
        baseController.ViewBag.Page = 1;
        baseController.ViewBag.PageSize = -1;
        SkipCount = 0;
        TakeCount = _totalCount;
    }

    private void _limitedRecords()
    {
        float pagesCountFloat = (float)_totalCount / _size;

        if (pagesCountFloat < 1)
        {
            _pagesCount = 1;
        }
        else if (Math.Abs(pagesCountFloat - 1) < 0.00001f)
        {
            _pagesCount = 1;
        }
        else if (pagesCountFloat > 1)
        {
            _pagesCount = (int)Math.Ceiling(pagesCountFloat);
        }

        if (_pagesCount == 0)
        {
            _pagesCount = 1;
        }
        if (_page <= 1)
        {
            _page = 1;
        }
        if (_page > _pagesCount)
        {
            _page = _pagesCount;
        }
        baseController.ViewBag.Pages = _pagesCount;
        baseController.ViewBag.Page = _page;
        baseController.ViewBag.PageSize = _size;
        SkipCount = (_page - 1) * _size;
        TakeCount = _size;
    }

    public void Run()
    {
        if (_size == -1)
        {
            _allRecords();
        }
        else if (_size >= 10 && _size <= 50)
        {
            _limitedRecords();
        }
    }
}
