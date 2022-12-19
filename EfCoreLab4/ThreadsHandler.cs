namespace EfCoreLab4;

public sealed class ThreadsHandler
{
	private ExampleContext Context { get; set; }

	AutoResetEvent AutoResetEventHandler = new AutoResetEvent(true);

    Mutex Mutex = new();

    Semaphore Semaphore = new(0, 1);

    public ThreadsHandler(ExampleContext context)
	{
		Context = context;
	}

	public async Task LockExample()
	{
		for (int i = 0; i < 10; i++)
		{
			Thread myThread = new(AddExampleEntityWithLock);
			myThread.Name = $"Thread {i}";
			myThread.Start(i);
		}
	}

	public void AddExampleEntityWithLock(object? threadNumber)
	{
		lock (this)
		{
			Context.Examples.Add(new ExampleEntity { SomeField = (int)threadNumber });
			Context.SaveChanges();
		}
	}

	public async Task MonitorExample()
	{
		for (int i = 0; i < 10; i++)
		{
			Thread myThread = new(AddExampleEntityWithMonitor);
			myThread.Name = $"Thread {i}";
			myThread.Start(i);
		}
	}

	public void AddExampleEntityWithMonitor(object? threadNumber)
	{
		bool lockTaken = false;
		try
		{
			Monitor.Enter(this, ref lockTaken);
			Context.Examples.Add(new ExampleEntity { SomeField = (int)threadNumber });
			Context.SaveChanges();
		}
		finally
		{
			if (lockTaken) Monitor.Exit(this);
		}
	}

	public async Task AutoResetEventExample()
	{
		for (int i = 0; i < 10; i++)
		{
			Thread myThread = new(AddExampleEntityWithAutoResetEvent);
			myThread.Name = $"Thread {i}";
			myThread.Start(i);
		}
	}

	public void AddExampleEntityWithAutoResetEvent(object? threadNumber)
	{
		AutoResetEventHandler.WaitOne();

		Context.Examples.Add(new ExampleEntity { SomeField = (int)threadNumber });
		Context.SaveChanges();

		AutoResetEventHandler.Set();
	}
	
	public async Task MutexExample()
	{
		for (int i = 0; i < 10; i++)
		{
			Thread myThread = new(AddExampleEntityWithMutex);
			myThread.Name = $"Thread {i}";
			myThread.Start(i);
		}
	}

	public void AddExampleEntityWithMutex(object? threadNumber)
	{
        Mutex.WaitOne();

		Context.Examples.Add(new ExampleEntity { SomeField = (int)threadNumber });
		Context.SaveChanges();

        Mutex.ReleaseMutex();
	}

	public async Task SemaphoreExample()
	{
		for (int i = 0; i < 10; i++)
		{
            Thread myThread = new(AddExampleEntityWithSemaphore);
            myThread.Name = $"Thread {i}";
            myThread.Start(i);
		}

        Semaphore.Release(1);
    }

	public void AddExampleEntityWithSemaphore(object? threadNumber)
	{
        Semaphore.WaitOne();

		Context.Examples.Add(new ExampleEntity { SomeField = (int)threadNumber });
		Context.SaveChanges();

        Semaphore.Release();
	}
}
