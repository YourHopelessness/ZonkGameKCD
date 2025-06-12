using ZonkGame.DB.Entites.Auth;

namespace ZonkGame.DB.Utils
{
    public class ApiResourceComparer : IEqualityComparer<ApiResource>
    {
        public bool Equals(ApiResource x, ApiResource y)
        {
            if (x == null || y == null) return false;

            return x.Controller == y.Controller &&
                   x.Action == y.Action &&
                   x.HttpMethod == y.HttpMethod &&
                   x.Route == y.Route &&
                   x.ApiName == y.ApiName;
        }

        public int GetHashCode(ApiResource obj)
        {
            return HashCode.Combine(obj.Controller, obj.Action, obj.HttpMethod, obj.Route, obj.ApiName);
        }
    }
}
