

Publisher publisher1 = new Publisher();
SubscriberA subscriberA = new SubscriberA(publisher1);
SubscriberB subscriberB = new SubscriberB(publisher1);
//SubscriberAが先に登録していますから、
//順番としては、必ず SubscriberA_Publisher_MyEvent メソッドが先に実行されます。
publisher1.TriggerEvent("Name");
//Output:
//Name: SubscriberA
//Name:SubscriberB
//999


//delegateの定義、delegateの命名は基本的にHandlerがついています。
public delegate int MyEventHandler(string text);
//一般的にはPublisherに返信する場合がなく、intではなくvoidを使いますが、//全面的な説明したいため、敢えてintにしています。


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
        var _delegate = new MyEventHandler(SubscriberA_Publisher_MyEvent);//Execution。
                                                                                      //MyEventHandlerのシグネチャに相応しいメソッドをExecutionとしてMyEventHandlerに渡します
        publisher.MyEvent += _delegate;//イベントに登録。
                                       //もし、MyEventがInvokeしたら、_delegateのExecutionを実行します。
                                       //即ちSubscriberA_Publisher_MyEventメソッドをCallします。
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
