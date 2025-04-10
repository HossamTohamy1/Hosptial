using Microsoft.AspNetCore.Http;

public class MyClass
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MyClass(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void MyMethod()
    {
        var user = _httpContextAccessor.HttpContext.User.Identity.Name;
    }
}
