namespace Lab4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Стратегия");
            Order order = new Order(5, new CourierDelivery());
            Console.WriteLine("Курьер: " + order.GetDeliveryPrice());

            order.SetDelivery(new PostDelivery());
            Console.WriteLine("Почта: " + order.GetDeliveryPrice());

            order.SetDelivery(new PickupDelivery());
            Console.WriteLine("Самовывоз: " + order.GetDeliveryPrice());

            Console.WriteLine();
            Console.WriteLine("цепочка обязанностей");
            Manager foreman = new Foreman("Иванов");
            foreman.SetNext(new Boss("Петров")).SetNext(new Director("Сидоров"));

            foreman.Handle(2);
            foreman.Handle(10);
            foreman.Handle(25);
            foreman.Handle(45);

            Console.WriteLine();
            Console.WriteLine("Итератор");
            Library library = new Library();
            library.Add(new Book("Мастер и Маргарита", "Булгаков"));
            library.Add(new Book("Война и мир", "Толстой"));
            library.Add(new Book("Преступление и наказание", "Достоевский"));

            Console.WriteLine("Прямой порядок:");
            IBookIterator it = library.GetIterator();
            while (it.HasNext())
            {
                Book b = it.Next();
                Console.WriteLine(b.Title + " - " + b.Author);
            }

            Console.WriteLine("Обратный порядок:");
            it = library.GetReverseIterator();
            while (it.HasNext())
            {
                Book b = it.Next();
                Console.WriteLine(b.Title + " - " + b.Author);
            }

       



        }
    }

 //СТРАТЕГИЯ 
 // Способ доставки можно менять на лету не трогая класс заказа

        interface IDeliveryStrategy
        {
            double Calculate(double weight);
        }

        class CourierDelivery : IDeliveryStrategy
        {
            public double Calculate(double weight)
            {
                return 200 + weight * 30;
            }
        }

        class PostDelivery : IDeliveryStrategy
        {
            public double Calculate(double weight)
            {
                return 100 + weight * 10;
            }
        }

        class PickupDelivery : IDeliveryStrategy
        {
            public double Calculate(double weight)
            {
                return 0;
            }
        }

        class Order
        {
            public double Weight;
            private IDeliveryStrategy delivery;

            public Order(double weight, IDeliveryStrategy delivery)
            {
                Weight = weight;
                this.delivery = delivery;
            }

            public void SetDelivery(IDeliveryStrategy newDelivery)
            {
                delivery = newDelivery;
            }

            public double GetDeliveryPrice()
            {
                return delivery.Calculate(Weight);
            }
        }

 // 2 ЦЕПОЧКА ОБЯЗАННОСТЕЙ 
 // Заявка на отпуск (дни) идёт по цепочке: мастер -  начальник -  директор

        abstract class Manager
        {
            protected Manager next;
            protected string name;

            public Manager(string name)
            {
                this.name = name;
            }

            public Manager SetNext(Manager next)
            {
                this.next = next;
                return next;
            }

            public abstract void Handle(int days);
        }

        class Foreman : Manager
        {
            public Foreman(string name) : base(name) { }

            public override void Handle(int days)
            {
                if (days <= 3)
                    Console.WriteLine($"Мастер {name} одобрил отпуск на {days} дн.");
                else if (next != null)
                    next.Handle(days);
            }
        }

        class Boss : Manager
        {
            public Boss(string name) : base(name) { }

            public override void Handle(int days)
            {
                if (days <= 14)
                    Console.WriteLine($"Начальник {name} одобрил отпуск на {days} дн.");
                else if (next != null)
                    next.Handle(days);
            }
        }

        class Director : Manager
        {
            public Director(string name) : base(name) { }

            public override void Handle(int days)
            {
                if (days <= 30)
                    Console.WriteLine($"Директор {name} одобрил отпуск на {days} дн.");
                else
                    Console.WriteLine($"Отпуск на {days} дн. никто не может одобрить :(");
            }
        }

        // 3. ИТЕРАТОР
        // Своя коллекция книг и свой итератор для обхода
        

    class Book
    {
        public string Title;
        public string Author;

        public Book(string title, string author)
        {
            Title = title;
            Author = author;
        }
    }

        interface IBookIterator
        {
            bool HasNext();
            Book Next();
        }

        class Library
        {
            private List<Book> books = new List<Book>();

            public void Add(Book book)
            {
                books.Add(book);
            }

            public int Count
            {
                get { return books.Count; }
            }

            public Book this[int index]
            {
                get { return books[index]; }
            }

            // обход с начала
            public IBookIterator GetIterator()
            {
                return new LibraryIterator(this);
            }

            // обход с конца
            public IBookIterator GetReverseIterator()
            {
                return new ReverseIterator(this);
            }
        }

        class LibraryIterator : IBookIterator
        {
            private Library library;
            private int position = 0;

            public LibraryIterator(Library library)
            {
                this.library = library;
            }


            public bool HasNext()
            {
                return position < library.Count;
            }

            public Book Next()
            {
                Book book = library[position];
                position++;
                return book;
            }
        }

        class ReverseIterator : IBookIterator
        {
            private Library library;
            private int position;

            public ReverseIterator(Library library)
            {
                this.library = library;
                position = library.Count - 1;
            }

            public bool HasNext()
            {
                return position >= 0;
            }

            public Book Next()
            {
                Book book = library[position];
                position--;
                return book;
            }
        }








}
