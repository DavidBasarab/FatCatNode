using System.ServiceModel;

namespace Tester
{
    [ServiceContract(Namespace = "http://fatcatnode.com/")]
    public interface ITestNode
    {
        [OperationContract]
        void Message(string nodeId, string message, params object[] args);
    }
}