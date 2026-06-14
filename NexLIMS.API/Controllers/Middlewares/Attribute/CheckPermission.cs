
namespace NexLIMS.API.Middlewares
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class CheckPermission : Attribute
    {
        public string Permission { get; }

        public CheckPermission(string permission)
        {
            Permission = permission;
        }
    }
}