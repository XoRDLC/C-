using System;
using static Learning.Statics;

namespace Learning
{
    public class Class1 {
        public void Main() {
            UserEventHandler();
            UserEventArgs();
        }

        private static void UserEventHandler() {
            Publisher publisher = new Publisher();
            Subscriber subscriber = new Subscriber(publisher);

            publisher.Calculate();
            Console.WriteLine("13th: {0}", subscriber.ThCounter);
            Console.ReadLine();
        }

        private static void UserEventArgs() {
            PublisherGeneric publisher = new PublisherGeneric();
            SubscriberGeneric subscriber = new SubscriberGeneric(publisher);

            publisher.Calculate();
            Console.WriteLine("Counted in User EventArgs: {0}", subscriber.ThCounter);
            Console.ReadLine();
        }
    }


    #region Event, reassign accessor; not implemented
    class UserAccessorsEvent
    {
        public event EventHandler Eve {
            add {  }
            remove {  }
        }
    }
    #endregion

    #region Events, user EventArgs type 
    public class IncrementerEventArgs : EventArgs {
        public int IncStorageP { get; set; }
        public int IncNotStored { get; set; }
    }

    public class PublisherGeneric {
        public event EventHandler<IncrementerEventArgs> EventHadlerGeneric;
        public void Calculate() {
            IncrementerEventArgs args = new IncrementerEventArgs();
            for (int i = 1; i < 1000; i++) {
                if (i % 13 == 0)
                {
                    args.IncStorageP = i;
                    EventHadlerGeneric?.Invoke(this, args);
                }
                else args.IncNotStored++;
            }
        }
    }

    public class SubscriberGeneric {
        public int ThCounter { get; private set; }
        public SubscriberGeneric(PublisherGeneric incrementer) {
            ThCounter = 0;
            incrementer.EventHadlerGeneric += Counter;
        }

        public void Counter(object source, IncrementerEventArgs args) {
            Console.WriteLine($"Incrementer: {args.IncStorageP} in {source.ToString()}, not stored {args.IncNotStored}");
            ThCounter++;
        }
    }
    #endregion

    #region Events, user event type
    delegate void Handler();
    class Publisher {
        public event Handler EventHandler;
        public void Calculate() {
            for (int i = 1; i < 1000; i++)
            {
                if (i % 13 == 0) EventHandler();
            }
        }
    }

    class Subscriber {
        public int ThCounter { get; private set; }
        public Subscriber(Publisher publisher) {
            ThCounter = 0;
            publisher.EventHandler += Counter;
        }

        void Counter() {
            ThCounter++;
        }
    }
    #endregion
}
