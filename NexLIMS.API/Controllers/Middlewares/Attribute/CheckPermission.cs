
namespace NexLIMS.API.Middlewares
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class CheckPermissionAttribute : Attribute
    {
        public string Permission { get; }

        public CheckPermissionAttribute(string permission)
        {
            Permission = permission;
        }
    }
}