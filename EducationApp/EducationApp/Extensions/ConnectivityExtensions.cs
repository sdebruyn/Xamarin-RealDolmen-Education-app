using System.Linq;
using Plugin.Connectivity.Abstractions;

namespace EducationApp.Extensions
{
    public static class ConnectivityExtensions
    {
        public static bool IsFast(this IConnectivity connection) => connection.IsConnected &&
                                                                    connection.ConnectionTypes.Any(
                                                                        ct =>
                                                                            Constants.Internet.FastConnectionTypes
                                                                                .Contains(ct));

        public static bool IsSlow(this IConnectivity connection) => connection.IsConnected &&
                                                                    !connection.ConnectionTypes.Any(
                                                                        ct =>
                                                                            Constants.Internet.FastConnectionTypes
                                                                                .Contains(ct));
    }
}