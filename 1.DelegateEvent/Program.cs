

Publisher publisher1 = new Publisher();
SubscriberA subscriberA = new SubscriberA(publisher1);
SubscriberB subscriberB = new SubscriberB(publisher1);
publisher1.TriggerEvent("Name");
//Output:
//Name: SubscriberA
//Name:SubscriberB
//999

//SubscriberAが先に登録していますから、
//順番としては、必ずSubscriberA_Publisher_MyEventメソッドが先に実行されます。





//一般的にはPublisherに返信する場合がなく、intではなくvoidを使いますが、
//全面的な説明したいため、敢えてintにしています。
public delegate int MyEventHandler(string text);


internal class Publisher
{
    public event MyEventHandler? MyEvent;

    public void TriggerEvent(string message)
    {
        //Publish。Subscriberがいないと例外が出ますので、“？”を使います。
        // Invokeの意味：MyEventに登録したdelegateを順番で実行します
        var result = MyEvent?.Invoke(message);
        Console.WriteLine(result);//Publisher　は最後のSubscriberの返信だけもらえます。
    }
}

internal class SubscriberA
{
    public SubscriberA(Publisher publisher)
    {
        publisher.MyEvent += new MyEventHandler(SubscriberA_Publisher_MyEvent);//イベントに登録。MyDelegateのシグネチャに相応しいメソッドをMyDelegateにする、
                                                                               //もし、MyEventがInvokeしたら、SubscriberA_Publisher_MyEventメソッドをCallします。
    }

    private int SubscriberA_Publisher_MyEvent(string text)
    {
        Console.WriteLine(text + ":SubscriberA");
        return 0;
    }
}

internal class SubscriberB
{
    public SubscriberB(Publisher publisher)
    {
        publisher.MyEvent += SubscriberB_Publisher_MyEvent;//糖衣構文,実際SubscriberAと同じです
    }

    private int SubscriberB_Publisher_MyEvent(string text)
    {
        Console.WriteLine(text + ":SubscriberB");
        return 999;
    }
}
