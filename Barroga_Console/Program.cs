using System;

namespace SolidPrinciplesExample
{
    public class Order
    {
        public string OrderId { get; set; }
        public string ProductName { get; set; }
    }

    public interface IOrderRepository
    {
        void SaveOrder(Order order);
    }

    public class SqlOrderRepository : IOrderRepository
    {
        public void SaveOrder(Order order)
        {
            Console.WriteLine($"[SQL] Order {order.OrderId} for {order.ProductName} saved to database.");
        }
    }

    public class FileOrderRepository : IOrderRepository
    {
        public void SaveOrder(Order order)
        {
            Console.WriteLine($"[FILE] Order {order.OrderId} for {order.ProductName} saved to a file.");
        }
    }

    public interface INotificationService
    {
        void SendNotification(string message);
    }

    public class EmailNotification : INotificationService
    {
        public void SendNotification(string message)
        {
            Console.WriteLine($"[EMAIL] Notification sent: {message}");
        }
    }

    public class SmsNotification : INotificationService
    {
        public void SendNotification(string message)
        {
            Console.WriteLine($"[SMS] Notification sent: {message}");
        }
    }

    public interface IOrderProcessor
    {
        void ProcessOrder(Order order);
    }

    public class OrderProcessor : IOrderProcessor
    {
        private readonly IOrderRepository _orderRepository;
        private readonly INotificationService _notificationService;

        public OrderProcessor(IOrderRepository orderRepository, INotificationService notificationService)
        {
            _orderRepository = orderRepository;
            _notificationService = notificationService;
        }

        public void ProcessOrder(Order order)
        {
            _orderRepository.SaveOrder(order);
            _notificationService.SendNotification($"Order {order.OrderId} processed successfully.");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Order Processing System ===");

            Console.Write("Enter Order ID: ");
            string orderId = Console.ReadLine();

            Console.Write("Enter Product Name: ");
            string productName = Console.ReadLine();

            Order newOrder = new Order { OrderId = orderId, ProductName = productName };

            Console.WriteLine("\nChoose Storage Method:");
            Console.WriteLine("1. SQL Database");
            Console.WriteLine("2. File Storage");
            Console.Write("Enter choice (1/2): ");
            string storageChoice = Console.ReadLine();

            IOrderRepository orderRepository = (storageChoice == "2")
                ? (IOrderRepository)new FileOrderRepository()
                : new SqlOrderRepository();

            Console.WriteLine("\nChoose Notification Method:");
            Console.WriteLine("1. Email");
            Console.WriteLine("2. SMS");
            Console.Write("Enter choice (1/2): ");
            string notificationChoice = Console.ReadLine();

            INotificationService notificationService = (notificationChoice == "2")
                ? (INotificationService)new SmsNotification()
                : new EmailNotification();


            IOrderProcessor orderProcessor = new OrderProcessor(orderRepository, notificationService);
            orderProcessor.ProcessOrder(newOrder);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
