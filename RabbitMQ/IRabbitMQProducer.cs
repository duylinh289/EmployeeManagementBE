namespace EmployeeManagementBE.RabbitMQ
{
    public interface IRabbitMQProducer
    {
        public void SendEmployeeMessage<T>(T message);
    }
}
